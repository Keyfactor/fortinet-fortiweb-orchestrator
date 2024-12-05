## Keyfactor Orchestrator Integration: FortiWeb API Profile Setup

### Overview
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
