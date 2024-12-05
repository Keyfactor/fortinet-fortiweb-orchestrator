## Keyfactor Orchestrator Integration: FortiWeb API Profile Setup

### Overview
This document outlines the security configuration for the FortiWeb API integration with the Keyfactor Orchestrator. The API profile, `ApiProfile`, has been configured to grant minimal access while ensuring the orchestrator has the necessary permissions to perform its functions.

<details>
<summary>API User and Profile Setup</summary>

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

### Test Cases/Screenshots/Results

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

