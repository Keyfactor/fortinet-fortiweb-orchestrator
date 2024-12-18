## Overview

The FortiWeb Orchestrator Extension is an integration that can replace and inventory certificates on the device that are bound to a Vitrual Server via Policy.  The certificate store types that can be managed in the current version are: 

* FortiWeb - See Test Cases For Specific Use Cases that are supported.

## Requirements


## Client Machine Instructions

The Client Machine field should contain the IP or Domain name and Port Needed for REST API Access.  For SSH Access, Port 22 will be used.

## Developer Notes

During the inventory process, we encountered a limitation with the REST API: it does not return PEM files as part of the certificate data. To address this, we utilized SSH and the CLI to retrieve the PEM files directly.

### Key Points:
1. **CLI Dependency**: 
   - The retrieval process relies heavily on the current structure of the CLI.
   - If Fortinet updates or changes the CLI commands, this method could break, necessitating updates to the inventory logic.

2. **Scalability Concerns**: 
   - Testing was conducted with 750 certificates, and no issues were observed.
   - Inventories larger than 750 certificates have not been tested and may present limitations.

3. **Unreliable Transport**: 
   - SSH sessions are less stable compared to REST API calls.
   - Network disruptions or configuration changes could impact the retrieval process.

### Summary:
While this approach works, it is not ideal and introduces potential risks. Until FortiWeb provides PEM file support via the REST API or a more stable alternative, this method should be considered a temporary solution.

## API User And Profile Setup

<details>
<summary>API User and Profile Setup</summary>

This document outlines the security configuration for the FortiWeb API integration with the Keyfactor Orchestrator. The API profile, `ApiProfile`, has been configured to grant minimal access while ensuring the orchestrator has the necessary permissions to perform its functions.

### API Profile: `ApiProfile`

The `ApiProfile` is configured with the following permissions:

#### Access Control Permissions
The table below specifies the permissions granted to the API profile for each area of the FortiWeb system:

| **Access Control**                     | **Permissions**  |
|----------------------------------------|------------------|
| Maintenance                            | None             |
| System Configuration                   | Read-Write       |
| Network Configuration                  | None             |
| Log & Report                           | None             |
| Auth Users                             | None             |
| Server Policy Configuration            | Read-Write       |
| Web Protection Configuration           | None             |
| Machine Learning Configuration         | None             |
| Web Anti-Defacement Management         | None             |
| Web Vulnerability Scan Configuration   | None             |

#### Description of Permissions
- **None**: No access to the specified area.
- **Read-Only**: The user can view configurations but cannot make changes.
- **Read-Write**: The user can view and modify configurations.

#### Key Permissions for Integration
1. **System Configuration**: Grants the orchestrator the ability to manage system settings required for certificate deployment and system integration.
2. **Server Policy Configuration**: Allows the orchestrator to manage server policies, ensuring secure and efficient traffic handling.

### Security Best Practices
- Limit the use of the `ApiProfile` to only the Keyfactor Orchestrator account.
- Regularly audit API profile usage and permissions to ensure alignment with the principle of least privilege.
- Enable logging for API activity to monitor orchestrator interactions with the FortiWeb system.

### Integration Checklist
1. Create the `ApiProfile` in the FortiWeb system with the permissions listed above.
2. Assign the profile to the user account that the Keyfactor Orchestrator will use for authentication.
3. Verify that the orchestrator can access and modify only the required areas (System Configuration and Server Policy Configuration).
4. Perform a functionality test to ensure the orchestrator can complete all necessary operations without encountering permission errors.

By following this configuration, the Keyfactor Orchestrator will have secure and functional access to integrate with the FortiWeb system effectively.

For additional guidance, consult the FortiWeb and Keyfactor documentation or reach out to your administrator.

# API User Field Descriptions for FortiWeb Integration

This document explains the key fields required for API user authentication when integrating with FortiWeb.

---

## **API User Field Descriptions**

### 1. **`username`**
- **Definition**: The username of the FortiWeb API account.
- **Purpose**: Identifies the specific user accessing the FortiWeb API.
- **Details**: This username should belong to a user account configured in FortiWeb with an associated API profile that has the necessary permissions for the integration.
- **Example**:
  ```admin```

---

### 2. **`password`**
- **Definition**: The password associated with the username for authentication.
- **Purpose**: Used to securely authenticate the API user and ensure access control.
- **Details**: Ensure the password is strong and stored securely (e.g., encrypted storage or environment variables).
- **Example**:
  ```P@ssw0rd123!```

---

### 3. **`vdom` (ADOM Name)**
- **Definition**: The **Administrative Domain (ADOM)** or **Virtual Domain (VDOM)** name in the FortiWeb system.
- **Purpose**: Specifies the administrative or virtual domain within the FortiWeb system that the API user is targeting.
  - **ADOMs (Administrative Domains)**: Used in FortiManager environments to manage multiple instances of FortiWeb. ADOMs isolate administrative control between teams or environments.
  - **VDOMs (Virtual Domains)**: Virtualization feature in FortiWeb to segment and isolate configurations or policies within a single appliance.
- **When Required**: If the FortiWeb appliance is configured with multiple ADOMs or VDOMs, this field directs the API user to the correct domain. If no ADOMs/VDOMs are configured, use `"root"`.
- **Example**:
  ```Production_ADOM```

---

### **Best Practices**

#### 1. **Username & Password Security**
- Use a dedicated API user account with minimal permissions.
- Store credentials securely using encrypted storage or environment variables.
- Regularly rotate passwords and follow your organization's security policies.

#### 2. **VDOM/ADOM Selection**
- Ensure the `vdom` value corresponds to the correct administrative or virtual domain in your FortiWeb system.
- For single-domain systems, use the default value: `"root"`.

#### 3. **Audit Access**
- Regularly review and audit API user activity to ensure security and compliance.
</details>

## Test Cases

<details>
<summary>Test Case List</summary>

| Test Case | Description                                                                                     | Parameters                                                                                                                                                  | Expected Result                                                                                      | Actual Result                                                                    | Pass/Fail| Screenshot  |
|-----------|-------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------|----------|-------------|
| TC1       | Add certificate with no existing bindings and no overwrite.                                     | `managementtype=add`, `overwrite=false`, `certalias=www.tc1.com`                                                                                                 | Operation should not proceed since there are no existing bindings for the certificate.             | Operation did not proceed since there are no existing bindings for the certificate | Pass     | ![](docsource/Images/TC1.gif) |
| TC2       | Add certificate with no existing bindings and overwrite enabled.                                | `managementtype=add`, `overwrite=true`, `certalias=www.tc2.com`                                                                                                  | Operation should not proceed even with overwrite, as there are no existing bindings.               | Operation did not proceed since there are no existing bindings for the certificate | Pass     | ![](docsource/Images/TC2.gif) |
| TC3       | Replace a certificate bound to multiple policies.                                               | `managementtype=add`, `overwrite=true`, `certalias=www.testerdomain82.com`                                                                                              | Certificate should be replaced across all policies it is bound to.                                  | Certificate was replaced across all policies it is bound to.        | Pass | ![](docsource/Images/TC3.gif) |
| TC4       | Replace a certificate bound to a single policy.                                                 | `managementtype=add`, `overwrite=true`, `certalias=www.testerdomain1.com`                                                                                              | Certificate should be replaced in the single policy it is bound to.                                 | Certificate was replaced in the single policy it is bound to        | Pass | ![](docsource/Images/TC4.gif) |
| TC5       | Attempt to replace a certificate bound to a single policy without overwrite enabled.            | `managementtype=add`, `overwrite=false`, `certalias=www.testerdomain2.com`                                                                                             | Operation should fail with a message indicating overwrite is needed.                                | Operation failed with a message indicating overwrite is needed.        | Pass| ![](docsource/Images/TC5.gif) |
| TC6       | Inventory test to list only bound certificates.                                                 | `casename=Inventory`, `storepath=/`, `clientmachine=20.10.138.208:8443` 															                         | Should return a list of two bound certificates.                                                     | Returned a list of the two bound certificates       | Pass | ![](docsource/Images/TC6.gif) |
| TC7       | Test error handling with an invalid client machine.                                             | `casename=Inventory`, `storepath=/`, `clientmachine=20.10.138.211:8443`                      | Should return a reasonable error indicating the client machine is invalid.                          | Did return a reasonable error indicating the client machine is invalid.        | Pass| ![](docsource/Images/TC7.gif) |

</details>
