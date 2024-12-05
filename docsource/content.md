## Overview

The FortiWeb Orchestrator Extension is an integration that can replace and inventory certificates on the device that are bound to a Vitrual Server via Policy.  The certificate store types that can be managed in the current version are: 

* FortiWeb

## Requirements

* TestCases


## Client Machine Instructions

ToDo


## Developer Notes

ToDo

## Test Cases

<details>
<summary>Test Cases</summary>

| Test Case | Description                                                                                     | Parameters                                                                                                                                                  | Expected Result                                                                                      | Actual Result                       | Pass/Fail  | Screenshot  |
|-----------|-------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------|-------------------------------------|------------|-------------|
| TC1       | Add certificate with no existing bindings and no overwrite.                                     | `managementtype=add`, `overwrite=false`, `certalias=random`                                                                                                 | Operation should not proceed since there are no existing bindings for the certificate.             | *To be filled after testing*        | *To be filled* | *To be filled* |
| TC2       | Add certificate with no existing bindings and overwrite enabled.                                | `managementtype=add`, `overwrite=true`, `certalias=random`                                                                                                  | Operation should not proceed even with overwrite, as there are no existing bindings.               | *To be filled after testing*        | *To be filled* | *To be filled* |
| TC3       | Replace a certificate bound to multiple policies.                                               | `managementtype=add`, `overwrite=true`, `certalias=user-input`                                                                                              | Certificate should be replaced across all policies it is bound to.                                  | *To be filled after testing*        | *To be filled* | *To be filled* |
| TC4       | Replace a certificate bound to a single policy.                                                 | `managementtype=add`, `overwrite=true`, `certalias=user-input`                                                                                              | Certificate should be replaced in the single policy it is bound to.                                 | *To be filled after testing*        | *To be filled* | *To be filled* |
| TC5       | Attempt to replace a certificate bound to a single policy without overwrite enabled.            | `managementtype=add`, `overwrite=false`, `certalias=user-input`                                                                                             | Operation should fail with a message indicating overwrite is needed.                                | *To be filled after testing*        | *To be filled* | *To be filled* |
| TC6       | Inventory test to list only bound certificates.                                                 | `casename=Inventory`, `storepath=/`, `clientmachine=11.22.38.208:8443`, `managementtype=add`, `inventorytrusted`, `templatestackname`                        | Should return a list of two bound certificates.                                                     | *To be filled after testing*        | *To be filled* | *To be filled* |
| TC7       | Test error handling with an invalid client machine.                                             | `casename=Inventory`, `storepath=/`, `clientmachine=20.10.138.211:8443`, `managementtype=add`, `inventorytrusted`, `templatestackname`                       | Should return a reasonable error indicating the client machine is invalid.                          | *To be filled after testing*        | *To be filled* | *To be filled* |

</details>
