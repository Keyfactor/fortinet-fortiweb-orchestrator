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
using Keyfactor.Orchestrators.Common.Enums;
using Keyfactor.Orchestrators.Extensions;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb
{
    public class Validators
    {
       

        public static (bool valid, JobResult result) ValidateStoreProperties(JobProperties storeProperties,
            string storePath,string clientMachine,long jobHistoryId, string serverUserName, string serverPassword)
        {
            var errors = string.Empty;

            if (string.IsNullOrEmpty(storeProperties?.ApiKey))
            {
                errors += "You need to specify an ApiKey for FortiWeb.";
            }

            var hasErrors = (errors.Length > 0);

            if (hasErrors)
            {
                var result = new JobResult
                {
                    Result = OrchestratorJobStatusJobResult.Failure,
                    JobHistoryId = jobHistoryId,
                    FailureMessage = $"The store setup is not valid. {errors}"
                };

                return (false, result);
            }

            return (true, new JobResult());
        }

    }
}
