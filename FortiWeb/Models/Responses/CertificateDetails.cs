using System;
using System.Collections.Generic;
using System.Text;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Responses
{
    public class CertificateDetails
    {
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string CertificatePEM { get; set; }
        public bool HasPrivateKey { get; set; }
    }
}
