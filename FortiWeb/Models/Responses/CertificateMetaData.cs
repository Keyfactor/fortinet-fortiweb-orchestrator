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

using System.Collections.Generic;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Responses
{
    public class CertResult
    {
        public string _id { get; set; }
        public string name { get; set; }
        public int q_ref { get; set; }
        public string cert_type { get; set; }
        public int pkey_type { get; set; }
        public bool can_delete { get; set; }
        public bool can_view { get; set; }
        public bool can_download { get; set; }
        public bool can_config { get; set; }
        public bool is_default { get; set; }
        public string hsm { get; set; }
        public string comments { get; set; }
        public string status { get; set; }
        public string issuer { get; set; }
        public string validFrom { get; set; }
        public string validTo { get; set; }
        public int version { get; set; }
        public string subject { get; set; }
        public string serialNumber { get; set; }
        public string extension { get; set; }
    }

    public class CertificateMetaData
    {
        public List<CertResult> results { get; set; }
    }
}
