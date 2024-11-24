using System;
using System.Collections.Generic;
using System.Text;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Requests
{


    public class Data
    {
        public string certificate { get; set; }
    }

    public class SetBindingsRequest
    {
        public Data data { get; set; }
    }

}