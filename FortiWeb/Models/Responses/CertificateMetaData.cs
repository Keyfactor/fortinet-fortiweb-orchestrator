using System;
using System.Collections.Generic;
using System.Text;

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
