// Copyright 2024 Keyfactor
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Keyfactor.Extensions.Orchestrator.FortiWeb.Client;
using Keyfactor.Logging;
using Keyfactor.Orchestrators.Common.Enums;
using Keyfactor.Orchestrators.Extensions;
using Keyfactor.Orchestrators.Extensions.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;


namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Jobs
{
    public class Management : IManagementJobExtension
    {
        private static readonly string certStart = "-----BEGIN CERTIFICATE-----\n";
        private static readonly string certEnd = "\n-----END CERTIFICATE-----";

        private static readonly Func<string, string> Pemify = ss =>
            ss.Length <= 64 ? ss : ss.Substring(0, 64) + "\n" + Pemify(ss.Substring(64));

        private readonly IPAMSecretResolver _resolver;

        private ILogger _logger;

        public Management(IPAMSecretResolver resolver)
        {
            _resolver = resolver;
            _logger = LogHandler.GetClassLogger<Management>();
            _logger.LogTrace("Initialized Management with IPAMSecretResolver.");
        }

        private string ServerPassword { get; set; }

        private JobProperties StoreProperties { get; set; }

        private string ServerUserName { get; set; }

        protected internal virtual AsymmetricKeyEntry KeyEntry { get; set; }

        public string ExtensionName => "FortiWeb";

        public JobResult ProcessJob(ManagementJobConfiguration jobConfiguration)
        {
            _logger = LogHandler.GetClassLogger<Management>();
            _logger.LogTrace($"Processing job with configuration: {JsonConvert.SerializeObject(jobConfiguration)}");
            StoreProperties = JsonConvert.DeserializeObject<JobProperties>(
                jobConfiguration.CertificateStoreDetails.Properties,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });

            return PerformManagement(jobConfiguration);
        }

        private string ResolvePamField(string name, string value)
        {
            _logger.LogTrace($"Attempting to resolved PAM eligible field {name}");

            return _resolver.Resolve(value);
        }

        private JobResult PerformManagement(ManagementJobConfiguration config)
        {
            try
            {
                _logger.MethodEntry();
                ServerPassword = ResolvePamField("ServerPassword", config.ServerPassword);
                ServerUserName = ResolvePamField("ServerUserName", config.ServerUsername);

                _logger.LogTrace("Validating Store Properties for Management Job");

                var (valid, result) = Validators.ValidateStoreProperties(StoreProperties,
                    config.CertificateStoreDetails.StorePath, config.CertificateStoreDetails.ClientMachine,
                    config.JobHistoryId, ServerUserName, ServerPassword);

                _logger.LogTrace($"Validated Store Properties and valid={valid}");

                if (!valid) return result;
                _logger.LogTrace("Validated Store Properties for Management Job");

                var complete = new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Failure,
                    JobHistoryId = config.JobHistoryId,
                    FailureMessage =
                        "Invalid Management Operation"
                };

                if (config.OperationType.ToString() == "Add")
                {
                    _logger.LogTrace("Adding...");
                    _logger.LogTrace($"Add Config Json {JsonConvert.SerializeObject(config)}");
                    complete = PerformAddition(config);
                    _logger.LogTrace("Finished Perform Addition Function");

                }

                return complete;
            }
            catch (Exception e)
            {
                return new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Failure,
                    JobHistoryId = config.JobHistoryId,
                    FailureMessage = $"Unexpected Error Occurred {e.Message}"
                };
            }
        }


        private JobResult PerformAddition(ManagementJobConfiguration config)
        {

            _logger.MethodEntry();
            var warnings = string.Empty;

            if (config.CertificateStoreDetails.StorePath.Length > 0)
            {
                _logger.LogTrace(
                    $"Credentials JSON: Url: {config.CertificateStoreDetails.ClientMachine} Server UserName: {config.ServerUsername}");

                var client =
                    new FortiWebClient(config.CertificateStoreDetails.ClientMachine,
                        ServerUserName, ServerPassword, StoreProperties.ApiKey); //Api base URL Plus Key
                _logger.LogTrace(
                    "FortiWeb Client Created");

                var alias = config.JobCertificate.Alias;
                var duplicate = CheckForDuplicate(config, client, alias);

                _logger.LogTrace($"Duplicate? = {duplicate}");

                //Check for Duplicate already in FortiWeb, if there, make sure the Overwrite flag is checked before replacing
                if (duplicate && config.Overwrite || !duplicate)
                {
                    //1. See if certificate already exists on a binding out there, we will only deal with those in this integration
                    var policyList = client.GetPolicyList();
                    var policy = policyList.Result.results.FindAll(p => p.certificate == alias);
                    if (policy != null && policy.Count >0)
                    {
                        _logger.LogTrace("Either not a duplicate or overwrite was chosen....");
                        _logger.LogTrace($"Found Private Key {config.JobCertificate.PrivateKeyPassword}");

                        if (string.IsNullOrWhiteSpace(config.JobCertificate.Alias))
                            _logger.LogTrace("No Alias Found");

                        //change name because key and cert can't have same name that already exists (no replace)
                        if (duplicate)
                            alias = GenerateCertName(alias);

                        var certPem = GetPemFile(config);
                        _logger.LogTrace($"Got certPem {certPem}");
                        _logger.LogTrace("Found Leaf Certificate");
                        var type = string.IsNullOrWhiteSpace(config.JobCertificate.PrivateKeyPassword) ? "certificate" : "keypair";
                        _logger.LogTrace($"Certificate Type of {type}");
                        var importResult = client.ImportCertificate(alias,
                            config.JobCertificate.PrivateKeyPassword,
                            Encoding.UTF8.GetBytes(certPem[0]), Encoding.UTF8.GetBytes(certPem[1]), "yes", type,
                            config.CertificateStoreDetails.StorePath);
                        _logger.LogTrace("Finished Import About to Log Results...");
                        var content = importResult.Result;
                        LogResponse(content);
                        _logger.LogTrace("Finished Logging Import Results...");

                    }
                    else
                    {
                        return ReturnJobResult(config, warnings, false, "No existing bindings found with this certificate");
                    }

                    //Re-Bind all the policies for this particular certificate, could be more than 1
                    foreach (var pol in policy)
                    {
                        var res = client.SetCertificateBinding(pol.name, alias).Result;
                    }

                }
                if (duplicate && !config.Overwrite)
                {
                    return ReturnJobResult(config, warnings, false, "Duplicate detected, must use the overwrite flag.");
                }



                return ReturnJobResult(config, warnings, true, "");

            }

            return new JobResult
            {
                Result = OrchestratorJobStatusJobResult.Failure,
                JobHistoryId = config.JobHistoryId,
                FailureMessage =
                    $"Duplicate alias {config.JobCertificate.Alias} found in FortiWeb, to overwrite use the overwrite flag."
            };

        }


        private string GenerateCertName(string certName)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            int unixTimestamp = (int)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            certName = RightTrimAfter(unixTimestamp + "_" + certName, 31);
            return certName;
        }

        private bool CheckForDuplicate(ManagementJobConfiguration config, FortiWebClient client, string certificateName)
        {
            _logger.MethodEntry();
            try
            {

                _logger.MethodEntry();
                _logger.LogTrace("Getting list to check for duplicates");
                var certMetaDataList = client.GetCertificateMetaList().Result.results;
                _logger.LogTrace("Got list to check for duplicates");

                if (certMetaDataList == null)
                    return false;

                var certificatesResult = certMetaDataList.FindAll(c => c.name == certificateName);
                _logger.LogTrace("Searched for duplicates in the list");

                _logger.MethodExit();
                return certificatesResult.Count > 0;

            }
            catch (Exception e)
            {
                _logger.LogTrace(
                    $"Error Checking for Duplicate Cert in Management.CheckForDuplicate {LogHandler.FlattenException(e)}");
                throw;
            }
        }

        public static string RightTrimAfter(string input, int maxLength)
        {
            if (input.Length > maxLength)
            {
                // If the input string is longer than the specified length,
                // trim it to the specified length
                return input.Substring(0, maxLength);
            }
            else
            {
                // If the input string is shorter than or equal to the specified length,
                // return the input string unchanged
                return input;
            }
        }

        private static JobResult ReturnJobResult(ManagementJobConfiguration config, string warnings, bool success,
            string errorMessage)
        {
            if (warnings.Length > 0)
                return new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Warning,
                    JobHistoryId = config.JobHistoryId,
                    FailureMessage = warnings
                };

            if (success)
                return new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Success,
                    JobHistoryId = config.JobHistoryId,
                    FailureMessage = ""
                };

            return new JobResult
            {
                Result = OrchestratorJobStatusJobResult.Failure,
                JobHistoryId = config.JobHistoryId,
                FailureMessage = $"Result returned error {errorMessage}"
            };
        }

        private void LogResponse<T>(T content)
        {
            var resWriter = new StringWriter();
            var resSerializer = new XmlSerializer(typeof(T));
            resSerializer.Serialize(resWriter, content);
            _logger.LogTrace($"Serialized Xml Response {resWriter}");
        }

        private List<string> GetPemFile(ManagementJobConfiguration config)
        {
            // Load PFX
            var pfxBytes = Convert.FromBase64String(config.JobCertificate.Contents);
            Pkcs12Store p;

            using (var pfxBytesMemoryStream = new MemoryStream(pfxBytes))
            {
                p = new Pkcs12Store(pfxBytesMemoryStream,
                    config.JobCertificate?.PrivateKeyPassword?.ToCharArray());
            }

            _logger.LogTrace(
                $"Created Pkcs12Store containing Alias {config.JobCertificate.Alias} Contains Alias is {p.ContainsAlias(config.JobCertificate.Alias)}");

            // Extract private key
            string alias;
            string privateKeyString;
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    _logger.LogTrace("Extracting Private Key...");
                    var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(streamWriter);
                    _logger.LogTrace("Created pemWriter...");
                    alias = p.Aliases.Cast<string>().SingleOrDefault(a => p.IsKeyEntry(a));
                    _logger.LogTrace($"Alias = {alias}");
                    var publicKey = p.GetCertificate(alias).Certificate.GetPublicKey();
                    _logger.LogTrace($"publicKey = {publicKey}");
                    KeyEntry = p.GetKey(alias);
                    _logger.LogTrace($"KeyEntry = {KeyEntry}");
                    if (KeyEntry == null) throw new Exception("Unable to retrieve private key");

                    var privateKey = KeyEntry.Key;
                    _logger.LogTrace($"privateKey = {privateKey}");
                    var keyPair = new AsymmetricCipherKeyPair(publicKey, privateKey);

                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();
                    privateKeyString = Encoding.ASCII.GetString(memoryStream.GetBuffer()).Trim()
                        .Replace("\r", "").Replace("\0", "");
                    _logger.LogTrace($"Got Private Key String {privateKeyString}");
                    memoryStream.Close();
                    streamWriter.Close();
                    _logger.LogTrace("Finished Extracting Private Key...");
                }
            }

            var pubCertPem =
                Pemify(Convert.ToBase64String(p.GetCertificate(alias).Certificate.GetEncoded()));


            _logger.LogTrace($"Public cert Pem {pubCertPem}");

            //var certPem = privateKeyString + certStart + pubCertPem + certEnd;
            var results = new List<string>
            {
                BuildCertificate(pubCertPem),
                privateKeyString
            };
            return results;
        }


        private string BuildCertificate(string cert)
        {
            _logger.MethodEntry();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(cert);
            builder.AppendLine("-----END CERTIFICATE-----");
            _logger.LogTrace(builder.ToString());
            _logger.MethodExit();
            return builder.ToString();
        }

    }

}