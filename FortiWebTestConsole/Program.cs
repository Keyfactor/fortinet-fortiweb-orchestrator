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
using System.Threading;
using System.Threading.Tasks;
using Keyfactor.Extensions.Orchestrator.FortiWeb.Jobs;
using Keyfactor.Orchestrators.Extensions;
using Keyfactor.Orchestrators.Extensions.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace FortiWebTestConsole
{
    internal class Program
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string CaseName { get; set; }
        public static string CertAlias { get; set; }
        public static string ClientMachine { get; set; }
        public static string ApiKey { get; set; }
        public static string StorePath { get; set; }
        public static string Overwrite { get; set; }
        public static string ManagementType { get; set; }
        public static string CertificateContent { get; set; }


        private static async Task Main(string[] args)
        {

           
            var arguments = new Dictionary<string, string>();
            Thread.Sleep(20000);
            foreach (var argument in args)
            {
                var splitted = argument.Split('=',2);

                if (splitted.Length == 2) arguments[splitted[0]] = splitted[1];
            }
            if (args.Length > 0)
            {
                CaseName = arguments["-casename"];
                UserName = arguments["-user"];
                Password = arguments["-password"];
                StorePath = arguments["-storepath"];
                ApiKey = arguments["-apikey"];
                ClientMachine = arguments["-clientmachine"];
            }
            else
            {
                Console.WriteLine("Enter The Case Name Inventory or Management");
                CaseName = Console.ReadLine();
                Console.WriteLine("Enter User Name");
                UserName = Console.ReadLine();
                Console.WriteLine("Enter The Password");
                Password = Console.ReadLine();
                Console.WriteLine("Enter Store Path");
                StorePath = Console.ReadLine();
                Console.WriteLine("Enter ApiKey");
                ApiKey = Console.ReadLine();
                Console.WriteLine("Enter ClientMachine");
                ClientMachine = Console.ReadLine();
            }

            //Determine if this is a firewall or Panorama Instance
            var isPanorama = !Convert.ToBoolean(StorePath == "/");

            // Display message to user to provide parameters.
            Console.WriteLine("Running");

            switch (CaseName)
            {
                case "Inventory":
                    Console.WriteLine("Running Inventory");
                    InventoryJobConfiguration invJobConfig;
                    invJobConfig = GetInventoryJobConfiguration();
                    Console.WriteLine("Got Inventory Config");
                    SubmitInventoryUpdate sui = GetItems;
                    var secretResolver = new Mock<IPAMSecretResolver>();
                    secretResolver.Setup(m => m.Resolve(It.Is<string>(s => s == invJobConfig.ServerUsername)))
                        .Returns(() => invJobConfig.ServerUsername);
                    secretResolver.Setup(m => m.Resolve(It.Is<string>(s => s == invJobConfig.ServerPassword)))
                        .Returns(() => invJobConfig.ServerPassword);
                    var inv = new Inventory(secretResolver.Object);
                    Console.WriteLine("Created Inventory Object With Constructor");
                    var invResponse = inv.ProcessJob(invJobConfig, sui);
                    Console.WriteLine("Back From Inventory");
                    Console.Write(JsonConvert.SerializeObject(invResponse));
                    Console.ReadLine();
                    break;
                case "Management":
                    Console.WriteLine("Select Management Type Add or Remove");
                    string mgmtType;
                    mgmtType = args.Length == 0 ? Console.ReadLine() : arguments["-managementtype"];

                    if (mgmtType?.ToUpper() == "ADD")
                    {
                        if (args.Length > 0)
                        {
                            CertAlias = arguments["-certalias"];
                            Overwrite = arguments["-overwrite"];
                        }
                        else
                        {
                            Console.WriteLine("Enter Cert Alias");
                            CertAlias = Console.ReadLine();
                            Console.WriteLine("Overwrite (True or False)?");
                            Overwrite = Console.ReadLine();
                        }

                        Console.WriteLine("Start Generated Cert in KF API");
                        var client = new KeyfactorClient();
                        var kfResult = client.EnrollCertificate($"www.{CertAlias}.com").Result;
                        CertificateContent = kfResult.CertificateInformation.Pkcs12Blob;
                        Console.WriteLine("End Generated Cert in KF API");

                        var jobConfiguration = GetManagementJobConfiguration();
                        var mgmtSecretResolver = new Mock<IPAMSecretResolver>();
                        mgmtSecretResolver
                            .Setup(m => m.Resolve(It.Is<string>(s => s == jobConfiguration.ServerUsername)))
                            .Returns(() => jobConfiguration.ServerUsername);
                        mgmtSecretResolver
                            .Setup(m => m.Resolve(It.Is<string>(s => s == jobConfiguration.ServerPassword)))
                            .Returns(() => jobConfiguration.ServerPassword);
                        var mgmt = new Management(mgmtSecretResolver.Object);

                        var result = mgmt.ProcessJob(jobConfiguration);
                        Console.Write(JsonConvert.SerializeObject(result));
                        Console.ReadLine();
                    }

                    break;
            }
        }


        public static bool GetItems(IEnumerable<CurrentInventoryItem> items)
        {
            return true;
        }

        public static InventoryJobConfiguration GetInventoryJobConfiguration()
        {
            var fileContent = File.ReadAllText("FortiWebInventory.json").Replace("UserNameGoesHere", UserName)
                .Replace("PasswordGoesHere", Password).Replace("ClientMachineGoesHere", ClientMachine);
            var result =
                JsonConvert.DeserializeObject<InventoryJobConfiguration>(fileContent);
            return result;
        }


        public static ManagementJobConfiguration GetManagementJobConfiguration()
        {

            var overWriteReplaceString = "\"Overwrite\": false";
            if (Overwrite.ToUpper() == "TRUE")
            {
                overWriteReplaceString = "\"Overwrite\": true";
            }
            
            var fileContent = File.ReadAllText("FortiWebMgmt.json").Replace("UserNameGoesHere", UserName)
                .Replace("PasswordGoesHere", Password).Replace("TemplateNameGoesHere", StorePath)
                .Replace("ApiKeyGoesHere", ApiKey).Replace("AliasGoesHere", CertAlias)
                .Replace("ClientMachineGoesHere", ClientMachine)
                .Replace("\"Overwrite\": false",overWriteReplaceString)
                .Replace("CertificateContentGoesHere", CertificateContent);
            var result =
                JsonConvert.DeserializeObject<ManagementJobConfiguration>(fileContent);
            return result;
        }

    }
}