## Overview

This orchestrator automates SSL/TLS certificate management on FortiWeb appliances by streamlining the inventory and renewal processes. Using a hybrid approach that combines REST API and SSH/CLI commands, it ensures accurate certificate tracking and efficient policy updates.

### FortiWeb Orchestrator: Certificate Management and Renewal

#### Key Features

- **Certificate Inventory**:
  - Inventories only certificates bound to active policies on FortiWeb.
  - Uses **SSH/CLI commands** for inventory due to lack of API support for certificate retrieval.

- **Certificate Renewal**:
  - Replaces certificates needing renewal for policies.
  - Leverages **REST API** for certificate upload and policy updates.

- **Seamless Integration**:
  - Combines SSH/CLI for inventory with REST API for renewal.
  - Maintains compatibility with FortiWeb's configurations.

#### Workflow

#### 1. Certificate Inventory

- The orchestrator connects to FortiWeb via **SSH**.
- Executes CLI commands to retrieve certificates and associated policies.
- Outputs a report with:
  - Certificate names
  - Associated policies
  - Expiry dates

#### 2. Certificate Renewal

- Uses the **REST API** for replacing certificates tied to policies.
- Renewal process:
  1. Identify expiring/expired certificates.
  2. Upload the renewed certificate to FortiWeb.
  3. Update policies to use the new certificate.
  4. Validate successful updates.

#### 3. Error Handling and Validation

- Retries SSH/CLI commands if inventory retrieval fails.
- Validates API responses to ensure successful certificate updates.
- Logs all operations for auditing and troubleshooting.

### Prerequisites

1. **FortiWeb Configuration**:
   - SSH access with permissions for inventory commands.
   - REST API enabled with credentials for certificate management.

2. **Network Requirements**:
   - Ensure SSH and API connectivity to FortiWeb.

3. **Certificates**:
   - Renewed certificates must be pre-provisioned or accessible.

### Security Considerations

- **Credential Management**: Use secure storage for SSH and API credentials.
- **Logging**: Avoid logging sensitive information (e.g., private keys).
- **Access Control**: Restrict access to authorized users and systems.

### Example Commands and API Endpoints

#### SSH/CLI for Inventory
```bash
show full-configuration | grep -A 10 policy

