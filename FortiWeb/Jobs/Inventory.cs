extern alias SSHNet;
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
using System.Linq;
using System.Text;
using Keyfactor.Extensions.Orchestrator.FortiWeb.Client;
using Keyfactor.Logging;
using Keyfactor.Orchestrators.Common.Enums;
using Keyfactor.Orchestrators.Extensions;
using Keyfactor.Orchestrators.Extensions.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Jobs
{
    public class Inventory : IInventoryJobExtension
    {
        private ILogger _logger;

        private readonly IPAMSecretResolver _resolver;

        public Inventory(IPAMSecretResolver resolver)
        {
            _resolver = resolver;
        }

        private string ServerPassword { get; set; }
        private string ServerUserName { get; set; }

        private JobProperties StoreProperties { get; set; }

        public string ExtensionName => "FortiWeb";

        public JobResult ProcessJob(InventoryJobConfiguration jobConfiguration,
            SubmitInventoryUpdate submitInventoryUpdate)
        {
            _logger = LogHandler.GetClassLogger<Inventory>();
            _logger.MethodEntry(LogLevel.Debug);
            StoreProperties = JsonConvert.DeserializeObject<JobProperties>(
                jobConfiguration.CertificateStoreDetails.Properties,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });

            return PerformInventory(jobConfiguration, submitInventoryUpdate);
        }

        public string ResolvePamField(string name, string value)
        {
            _logger.LogTrace($"Attempting to resolved PAM eligible field {name}");
            return _resolver.Resolve(value);
        }

        private JobResult PerformInventory(InventoryJobConfiguration config, SubmitInventoryUpdate submitInventory)
        {
            try
            {
                _logger.MethodEntry(LogLevel.Debug);
                ServerPassword = ResolvePamField("ServerPassword", config.ServerPassword);
                ServerUserName = ResolvePamField("ServerUserName", config.ServerUsername);
                _logger.LogTrace("Got Server User Name and Password");

                var (valid, result) = Validators.ValidateStoreProperties(StoreProperties,
                    config.CertificateStoreDetails.StorePath, config.CertificateStoreDetails.ClientMachine,
                    config.JobHistoryId, ServerUserName, ServerPassword);
                if (!valid) return result;

                _logger.LogTrace("Store Properties are Valid");
                _logger.LogTrace(
                    $"Client Machine: {config.CertificateStoreDetails.ClientMachine}");

                var apiClient = new FortiWebClient();
                var apiKey = apiClient.GenerateApiKey(ServerUserName, ServerPassword, StoreProperties.ADom);

                var client =
                    new FortiWebClient(config.CertificateStoreDetails.ClientMachine,
                        ServerUserName, ServerPassword, apiKey); //Api base URL Plus Key
                _logger.LogTrace("Inventory FotiWeb Client Created");

                var cliCertResults = client.GetCertificateInventory(config.CertificateStoreDetails.ClientMachine, 22, ServerUserName, ServerPassword);

                _logger.LogTrace($"Cli Cert Results Returned {JsonConvert.SerializeObject(cliCertResults)}");

                var warningFlag = false;
                var sb = new StringBuilder();
                sb.Append("");

                var inventoryItems = new List<CurrentInventoryItem>();

                var policyList = client.GetPolicyList().Result;

                _logger.LogTrace($"Policy List Returned Json: {JsonConvert.SerializeObject(policyList)}");

                var certResultsDict = cliCertResults.ToDictionary(c => c.Name);

                foreach (var policy in policyList.results)
                {
                    //Only inventory bound certificates.  Don't care about certs that are not bound
                    _logger.LogTrace($"Looking for matches where items in cli search match policies from API");
                    if (certResultsDict.ContainsKey(policy.certificate) && !inventoryItems.Exists(c => c.Alias == policy.certificate))
                    {
                        _logger.LogTrace($"Match(es) found");
                        inventoryItems.AddRange(cliCertResults.Where(c=>c.Name==policy.certificate).Select(
                            c =>
                            {
                                try
                                {
                                    _logger.LogTrace(
                                        $"Building Cert List Inventory Item Alias: {c.Name} Pem: {c.CertificatePEM} Private Key: {c.HasPrivateKey}");

                                    return BuildInventoryItem(c.Name, c.CertificatePEM, c.HasPrivateKey, false);
                                }
                                catch (Exception e)
                                {
                                    _logger.LogWarning(
                                        $"Could not fetch the certificate: {c.Name} associated with CN {c?.CommonName} error {LogHandler.FlattenException(e)}.");
                                    sb.Append(
                                        $"Could not fetch the certificate: {c.Name} associated with CN {c?.CommonName}.{Environment.NewLine}");
                                    warningFlag = true;
                                    return new CurrentInventoryItem();
                                }
                            }).Where(cer => cer.Certificates!=null).ToList());

                    }
                }


                _logger.LogTrace("Submitting Inventory To Keyfactor via submitInventory.Invoke");
                submitInventory.Invoke(inventoryItems);
                _logger.LogTrace("Submitted Inventory To Keyfactor via submitInventory.Invoke");

                _logger.MethodExit(LogLevel.Debug);
                return ReturnJobResult(config, warningFlag, sb);
            }
            catch (Exception e)
            {
                _logger.LogError($"PerformInventory Error: {e.Message}");
                return new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Failure,
                    JobHistoryId = config.JobHistoryId,
                    FailureMessage = e.Message
                };
            }
        }

        private JobResult ReturnJobResult(InventoryJobConfiguration config, bool warningFlag, StringBuilder sb)
        {
            if (warningFlag)
            {
                _logger.LogTrace("Found Warning");
                return new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Warning,
                    JobHistoryId = config.JobHistoryId,
                    FailureMessage = sb.ToString()
                };
            }

            _logger.LogTrace("Return Success");
            return new JobResult
            {
                Result = OrchestratorJobStatusJobResult.Success,
                JobHistoryId = config.JobHistoryId,
                FailureMessage = sb.ToString()
            };
        }


        protected virtual CurrentInventoryItem BuildInventoryItem(string alias, string certPem, bool privateKey, bool trustedRoot)
        {
            try
            {
                _logger.MethodEntry();

                _logger.LogTrace($"Alias: {alias} Pem: {certPem} PrivateKey: {privateKey}");
                var acsi = new CurrentInventoryItem
                {
                    Alias = alias,
                    Certificates = new[] { certPem },
                    ItemStatus = OrchestratorInventoryItemStatus.Unknown,
                    PrivateKeyEntry = privateKey,
                    UseChainLevel = false
                };

                return acsi;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occurred in Inventory.BuildInventoryItem: {e.Message}");
                throw;
            }
        }


    }
}