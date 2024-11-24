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
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Requests;
using Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Responses;
using Keyfactor.Logging;
using Microsoft.Extensions.Logging;
using SSHNet::Renci.SshNet;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Client
{
    public class FortiWebClient
    {
        private readonly ILogger _logger;

        public FortiWebClient(string url, string userName, string password, string apiKey)
        {
            _logger = LogHandler.GetClassLogger<FortiWebClient>();
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };
            HttpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri("https://" + url) };
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiKey);
        }

        private string ApiKey { get; }

        private HttpClient HttpClient { get; }

        public async Task<Policy> GetPolicyList()
        {
            try
            {
                var uri = $"/api/v2.0/cmdb/server-policy/policy";
                var httpResponse = await HttpClient.GetAsync(uri);

                if (httpResponse.IsSuccessStatusCode)
                {
                    // Successfully received a response, deserialize the content
                    return await GetJsonResponseAsync<Policy>(httpResponse);
                }
                else
                {
                    // Handle error response
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    _logger.LogError($"Error Occurred: {errorContent}");

                    throw new ApplicationException($"Error {errorContent}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occurred in FortiWebClient.GetPolicyList: {e.Message}");
                throw;
            }
        }

        public async Task<CertificateMetaData> GetCertificateMetaList()
        {
            try
            {
                var uri = $"/api/v2.0/system/certificate.local";
                var httpResponse = await HttpClient.GetAsync(uri);

                if (httpResponse.IsSuccessStatusCode)
                {
                    // Successfully received a response, deserialize the content
                    return await GetJsonResponseAsync<CertificateMetaData>(httpResponse);
                }
                else
                {
                    // Handle error response
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    _logger.LogError($"Error Occurred: {errorContent}");

                    throw new ApplicationException($"Error {errorContent}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occurred in FortiWebClient.GetCertificateMetaList: {e.Message}");
                throw;
            }
        }

        public List<CertificateDetails> GetCertificateInventory(string clientMachine,int port,string serverUserName,string serverPassword)
        {
            List<CertificateDetails> cliCertResults = null;

            using (var sshClient = new SshClient(StripPort(clientMachine), 22, serverUserName, serverPassword))
            {
                try
                {
                    // Connect to the SSH server
                    sshClient.Connect();

                    // Execute the command
                    var cmd = sshClient.CreateCommand("config system certificate local");
                    string sshResult = cmd.Execute();

                    cmd = sshClient.CreateCommand("show");
                    sshResult = cmd.Execute();

                    cliCertResults = ParseCertificates(sshResult);

                    // Disconnect
                    sshClient.Disconnect();

                    return cliCertResults;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                return cliCertResults;
            }
        }

        public static List<CertificateDetails> ParseCertificates(string input)
        {
            var certificates = new List<CertificateDetails>();
            int currentIndex = 0;

            while (currentIndex < input.Length)
            {
                // Locate the start of a certificate block
                int editIndex = input.IndexOf("edit \"", currentIndex);
                if (editIndex == -1) break;

                // Extract the name
                int nameStart = editIndex + 6;
                int nameEnd = input.IndexOf("\"", nameStart);
                string name = input.Substring(nameStart, nameEnd - nameStart);

                // Check for certificate presence
                int certStart = input.IndexOf("-----BEGIN CERTIFICATE-----", nameEnd);
                if (certStart == -1 || certStart < editIndex)
                {
                    // Skip non-certificate entries
                    currentIndex = nameEnd;
                    continue;
                }

                // Extract Common Name
                int subjectIndex = input.IndexOf("subject=", nameEnd);
                string commonName = null;
                if (subjectIndex != -1 && subjectIndex < certStart)
                {
                    int cnIndex = input.IndexOf("CN=", subjectIndex);
                    if (cnIndex != -1)
                    {
                        int cnEnd = input.IndexOf(",", cnIndex);
                        if (cnEnd == -1 || cnEnd > input.Length)
                            cnEnd = input.IndexOf(Environment.NewLine, cnIndex);
                        if (cnEnd == -1 || cnEnd > input.Length)
                            cnEnd = input.Length;

                        commonName = input.Substring(cnIndex + 3, cnEnd - cnIndex - 3).Trim();
                    }
                }

                // Extract the certificate PEM
                int certEnd = input.IndexOf("-----END CERTIFICATE-----", certStart);
                string certificatePem = null;
                if (certEnd != -1)
                {
                    certEnd += "-----END CERTIFICATE-----".Length;
                    certificatePem = input.Substring(certStart, certEnd - certStart).Trim();
                }

                // Determine if a private key is present
                bool hasPrivateKey = input.IndexOf("set private-key", nameEnd) != -1;

                // Add the certificate details
                certificates.Add(new CertificateDetails
                {
                    Name = name,
                    CommonName = commonName,
                    CertificatePEM = certificatePem,
                    HasPrivateKey = hasPrivateKey
                });

                // Move to the next block
                currentIndex = certEnd != -1 ? certEnd : nameEnd;
            }

            return certificates;
        }

        static string StripPort(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("IP address cannot be null or empty.");

            // Find the colon separating the IP and port
            int colonIndex = ipAddress.LastIndexOf(':');

            // If colon is found and it's not part of an IPv6 address
            if (colonIndex != -1 && !ipAddress.Contains("["))
            {
                return ipAddress.Substring(0, colonIndex);
            }

            return ipAddress; // Return the original string if no port is found
        }

        public async Task<SuccessResponse> ImportCertificate(string name, string passPhrase, byte[] certBytes, byte[] keyBytes,
            string includeKey, string category, string storePath)
        {

            try
            {
                var uri = $@"/api/v2.0/system/certificate.local.import_certificate";

                    // Successfully received a response, deserialize the content
                    var boundary = $"--------------------------{Guid.NewGuid():N}";
                    var requestContent = new MultipartFormDataContent();
                    requestContent.Headers.Remove("Content-Type");
                    requestContent.Headers.TryAddWithoutValidation("Content-Type",
                        $"multipart/form-data; boundary={boundary}");
                    //Workaround Palo Alto API does not like double quotes around boundary so can't use built in .net client
                    requestContent.GetType().BaseType?.GetField("_boundary", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(requestContent, boundary);
                    var cert = new ByteArrayContent(certBytes);
                    var key = new ByteArrayContent(keyBytes);
                    var pass = new StringContent(passPhrase);
                    var type = new StringContent("certificate");
                    //pfxContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-x509-ca-cert");
                    requestContent.Add(cert, "\"certificateFile\"", $"\"{name}.crt\"");
                    requestContent.Add(key, "\"keyFile\"", $"\"{name}.key\"");
                    requestContent.Add(pass, "password");
                    requestContent.Add(type, "type");
                    var httpResponse = await GetJsonResponseAsync<SuccessResponse>(
                        await HttpClient.PostAsync(uri, requestContent));

                return httpResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occurred in FortiWebClient.GetCertificateList: {e.Message}");
                throw;
            }
        }


        public async Task<SetBindingsResponse> SetCertificateBinding(string serviceName,string certificateName)
        {

            try
            {
                var uri = $@"/api/v2.0/cmdb/server-policy/policy?mkey={serviceName}";

                // Create the request object and populate it
                var sbRequest = new SetBindingsRequest
                {
                    data = new Data
                    {
                        certificate = certificateName
                    }
                };

                // Serialize the request object to JSON
                var json = JsonSerializer.Serialize(sbRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the PUT request
                var httpResponse = await GetJsonResponseAsync<SetBindingsResponse>(
                    await HttpClient.PutAsync(uri, httpContent));

                return httpResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occurred in FortiWebClient.GetCertificateList: {e.Message}");
                throw;
            }
        }

        public async Task<T> GetJsonResponseAsync<T>(HttpResponseMessage response)
        {
            try
            {
                EnsureSuccessfulResponse(response);
                var stringResponse =
                    await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
                return JsonSerializer.Deserialize<T>(stringResponse);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occured in FortiWebClient.GetXmlResponseAsync: {e.Message}");
                throw;
            }
        }

        public async Task<string> GetResponseAsync(HttpResponseMessage response)
        {
            try
            {
                EnsureSuccessfulResponse(response);
                var stringResponse =
                    await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
                return stringResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occured in FortiWebClient.GetResponseAsync: {e.Message}");
                throw;
            }
        }



        private void EnsureSuccessfulResponse(HttpResponseMessage response)
        {
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    var error = new StreamReader(response.Content.ReadAsStreamAsync().Result).ReadToEnd();
                    throw new Exception($"Request to FortiWeb was not successful - {response.StatusCode} - {error}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Occured in FortiWebClient.EnsureSuccessfulResponse: {e.Message}");
                throw;
            }
        }
    }
}