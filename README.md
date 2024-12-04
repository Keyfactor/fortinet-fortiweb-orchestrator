<h1 align="center" style="border-bottom: none">
    FortiWeb Universal Orchestrator Extension
</h1>

<p align="center">
  <!-- Badges -->
<img src="https://img.shields.io/badge/integration_status-production-3D1973?style=flat-square" alt="Integration Status: production" />
<a href="https://github.com/Keyfactor/fortinet-fortiweb-orchestrator/releases"><img src="https://img.shields.io/github/v/release/Keyfactor/fortinet-fortiweb-orchestrator?style=flat-square" alt="Release" /></a>
<img src="https://img.shields.io/github/issues/Keyfactor/fortinet-fortiweb-orchestrator?style=flat-square" alt="Issues" />
<img src="https://img.shields.io/github/downloads/Keyfactor/fortinet-fortiweb-orchestrator/total?style=flat-square&label=downloads&color=28B905" alt="GitHub Downloads (all assets, all releases)" />
</p>

<p align="center">
  <!-- TOC -->
  <a href="#support">
    <b>Support</b>
  </a>
  ·
  <a href="#installation">
    <b>Installation</b>
  </a>
  ·
  <a href="#license">
    <b>License</b>
  </a>
  ·
  <a href="https://github.com/orgs/Keyfactor/repositories?q=orchestrator">
    <b>Related Integrations</b>
  </a>
</p>

## Overview

The FortiWeb Orchestrator Extension is an integration that can replace and inventory certificates on the device that are bound to a Vitrual Server via Policy.  The certificate store types that can be managed in the current version are: 

* FortiWeb - Store Type for the FortiWeb instance



### FortiWeb

The RFDER store type can be used to manage DER encoded files.

Use cases supported:
1. Single certificate stores with private key in an external file.
2. Single certificate stores with no private key.

## Compatibility

This integration is compatible with Keyfactor Universal Orchestrator version 10.4 and later.

## Support
The FortiWeb Universal Orchestrator extension is supported by Keyfactor for Keyfactor customers. If you have a support issue, please open a support ticket with your Keyfactor representative. If you have a support issue, please open a support ticket via the Keyfactor Support Portal at https://support.keyfactor.com. 
 
> To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

## Requirements & Prerequisites

Before installing the FortiWeb Universal Orchestrator extension, we recommend that you install [kfutil](https://github.com/Keyfactor/kfutil). Kfutil is a command-line tool that simplifies the process of creating store types, installing extensions, and instantiating certificate stores in Keyfactor Command.


<details>
<summary><b>API and SSH Access:</b></summary>

1. The Fortiweb Orchestrator will manage certificates on the device using either SSH commands for inventory, since REST API calls were not available with the certificate information or using the FortiWeb REST API when replacing certificates since the REST API commands were available for that:


## Create the FortiWeb Certificate Store Type

To use the FortiWeb Universal Orchestrator extension, you **must** create the FortiWeb Certificate Store Type. This only needs to happen _once_ per Keyfactor Command instance.



* **Create FortiWeb using kfutil**:

    ```shell
    # FortiWeb
    kfutil store-types create FortiWeb
    ```

* **Create FortiWeb manually in the Command UI**:
    <details><summary>Create FortiWeb manually in the Command UI</summary>

    Create a store type called `FortiWeb` with the attributes in the tables below:

    #### Basic Tab
    | Attribute | Value | Description |
    | --------- | ----- | ----- |
    | Name | FortiWeb | Display name for the store type (may be customized) |
    | Short Name | FortiWeb | Short display name for the store type |
    | Capability | FortiWeb | Store type name orchestrator will register with. Check the box to allow entry of value |
    | Supports Add | ✅ Checked | Check the box. Indicates that the Store Type supports Management Add |
    | Supports Remove | ✅ Checked | Check the box. Indicates that the Store Type supports Management Remove |
    | Supports Discovery | 🔲 Unchecked |  Indicates that the Store Type supports Discovery |
    | Supports Reenrollment | 🔲 Unchecked |  Indicates that the Store Type supports Reenrollment |
    | Supports Create | 🔲 Unchecked |  Indicates that the Store Type supports store creation |
    | Needs Server | ✅ Checked | Determines if a target server name is required when creating store |
    | Blueprint Allowed | 🔲 Unchecked | Determines if store type may be included in an Orchestrator blueprint |
    | Uses PowerShell | 🔲 Unchecked | Determines if underlying implementation is PowerShell |
    | Requires Store Password | 🔲 Unchecked | Enables users to optionally specify a store password when defining a Certificate Store. |
    | Supports Entry Password | 🔲 Unchecked | Determines if an individual entry within a store can have a password. |

    The Basic tab should look like this:

    ![FortiWeb Basic Tab](docsource/images/FortiWeb-basic-store-type-dialog.png)

    #### Advanced Tab
    | Attribute | Value | Description |
    | --------- | ----- | ----- |
    | Supports Custom Alias | Required | Determines if an individual entry within a store can have a custom Alias. |
    | Private Key Handling | Optional | This determines if Keyfactor can send the private key associated with a certificate to the store. Required because IIS certificates without private keys would be invalid. |
    | PFX Password Style | Default | 'Default' - PFX password is randomly generated, 'Custom' - PFX password may be specified when the enrollment job is created (Requires the Allow Custom Password application setting to be enabled.) |

    The Advanced tab should look like this:

    ![FortiWeb Advanced Tab](docsource/images/FortiWeb-advanced-store-type-dialog.png)

    #### Custom Fields Tab
    Custom fields operate at the certificate store level and are used to control how the orchestrator connects to the remote target server containing the certificate store to be managed. The following custom fields should be added to the store type:

    | Name | Display Name | Description | Type | Default Value/Options | Required |
    | ---- | ------------ | ---- | --------------------- | -------- | ----------- |
    | ServerUsername | Server Username | A username for CLI/SSH access.  Used for inventory. (or valid PAM key if the username is stored in a KF Command configured PAM integration). | Secret |  | 🔲 Unchecked |
    | ServerPassword | Server Password | A password for CLI/SSH access.  Used for inventory.(or valid PAM key if the password is stored in a KF Command configured PAM integration). | Secret |  | 🔲 Unchecked |
    | ServerUseSsl | Use SSL | Should be true, http is not supported. | Bool | true | ✅ Checked |
    | ApiKey | Api Key | API Key for access to FortiWeb REST API.  Generate Key on there platform and plug it in here. | Secret |  | 🔲 Unchecked |

    The Custom Fields tab should look like this:

    ![FortiWeb Custom Fields Tab](docsource/images/FortiWeb-custom-fields-store-type-dialog.png)



    </details>

## Installation

1. **Download the latest FortiWeb Universal Orchestrator extension from GitHub.** 

    Navigate to the [FortiWeb Universal Orchestrator extension GitHub version page](https://github.com/Keyfactor/fortinet-fortiweb-orchestrator/releases/latest). Refer to the compatibility matrix below to determine whether the `net6.0` or `net8.0` asset should be downloaded. Then, click the corresponding asset to download the zip archive.
    | Universal Orchestrator Version | Latest .NET version installed on the Universal Orchestrator server | `rollForward` condition in `Orchestrator.runtimeconfig.json` | `fortinet-fortiweb-orchestrator` .NET version to download |
    | --------- | ----------- | ----------- | ----------- |
    | Older than `11.0.0` | | | `net6.0` |
    | Between `11.0.0` and `11.5.1` (inclusive) | `net6.0` | | `net6.0` | 
    | Between `11.0.0` and `11.5.1` (inclusive) | `net8.0` | `Disable` | `net6.0` | 
    | Between `11.0.0` and `11.5.1` (inclusive) | `net8.0` | `LatestMajor` | `net8.0` | 
    | `11.6` _and_ newer | `net8.0` | | `net8.0` |

    Unzip the archive containing extension assemblies to a known location.

    > **Note** If you don't see an asset with a corresponding .NET version, you should always assume that it was compiled for `net6.0`.

2. **Locate the Universal Orchestrator extensions directory.**

    * **Default on Windows** - `C:\Program Files\Keyfactor\Keyfactor Orchestrator\extensions`
    * **Default on Linux** - `/opt/keyfactor/orchestrator/extensions`
    
3. **Create a new directory for the FortiWeb Universal Orchestrator extension inside the extensions directory.**
        
    Create a new directory called `fortinet-fortiweb-orchestrator`.
    > The directory name does not need to match any names used elsewhere; it just has to be unique within the extensions directory.

4. **Copy the contents of the downloaded and unzipped assemblies from __step 2__ to the `fortinet-fortiweb-orchestrator` directory.**

5. **Restart the Universal Orchestrator service.**

    Refer to [Starting/Restarting the Universal Orchestrator service](https://software.keyfactor.com/Core-OnPrem/Current/Content/InstallingAgents/NetCoreOrchestrator/StarttheService.htm).


6. **(optional) PAM Integration** 

    The FortiWeb Universal Orchestrator extension is compatible with all supported Keyfactor PAM extensions to resolve PAM-eligible secrets. PAM extensions running on Universal Orchestrators enable secure retrieval of secrets from a connected PAM provider.

    To configure a PAM provider, [reference the Keyfactor Integration Catalog](https://keyfactor.github.io/integrations-catalog/content/pam) to select an extension, and follow the associated instructions to install it on the Universal Orchestrator (remote).


> The above installation steps can be supplimented by the [official Command documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/InstallingAgents/NetCoreOrchestrator/CustomExtensions.htm?Highlight=extensions).



## Defining Certificate Stores



* **Manually with the Command UI**

    <details><summary>Create Certificate Stores manually in the UI</summary>

    1. **Navigate to the _Certificate Stores_ page in Keyfactor Command.**

        Log into Keyfactor Command, toggle the _Locations_ dropdown, and click _Certificate Stores_.

    2. **Add a Certificate Store.**

        Click the Add button to add a new Certificate Store. Use the table below to populate the **Attributes** in the **Add** form.
        | Attribute | Description |
        | --------- | ----------- |
        | Category | Select "FortiWeb" or the customized certificate store name from the previous step. |
        | Container | Optional container to associate certificate store with. |
        | Client Machine |  |
        | Store Path |  |
        | Orchestrator | Select an approved orchestrator capable of managing `FortiWeb` certificates. Specifically, one with the `FortiWeb` capability. |
        | ServerUsername | A username for CLI/SSH access.  Used for inventory. (or valid PAM key if the username is stored in a KF Command configured PAM integration). |
        | ServerPassword | A password for CLI/SSH access.  Used for inventory.(or valid PAM key if the password is stored in a KF Command configured PAM integration). |
        | ServerUseSsl | Should be true, http is not supported. |
        | ApiKey | API Key for access to FortiWeb REST API.  Generate Key on there platform and plug it in here. |


        

        <details><summary>Attributes eligible for retrieval by a PAM Provider on the Universal Orchestrator</summary>

        If a PAM provider was installed _on the Universal Orchestrator_ in the [Installation](#Installation) section, the following parameters can be configured for retrieval _on the Universal Orchestrator_.
        | Attribute | Description |
        | --------- | ----------- |
        | ServerUsername | A username for CLI/SSH access.  Used for inventory. (or valid PAM key if the username is stored in a KF Command configured PAM integration). |
        | ServerPassword | A password for CLI/SSH access.  Used for inventory.(or valid PAM key if the password is stored in a KF Command configured PAM integration). |


        Please refer to the **Universal Orchestrator (remote)** usage section ([PAM providers on the Keyfactor Integration Catalog](https://keyfactor.github.io/integrations-catalog/content/pam)) for your selected PAM provider for instructions on how to load attributes orchestrator-side.

        > Any secret can be rendered by a PAM provider _installed on the Keyfactor Command server_. The above parameters are specific to attributes that can be fetched by an installed PAM provider running on the Universal Orchestrator server itself. 
        </details>
        

    </details>

* **Using kfutil**
    
    <details><summary>Create Certificate Stores with kfutil</summary>
    
    1. **Generate a CSV template for the FortiWeb certificate store**

        ```shell
        kfutil stores import generate-template --store-type-name FortiWeb --outpath FortiWeb.csv
        ```
    2. **Populate the generated CSV file**

        Open the CSV file, and reference the table below to populate parameters for each **Attribute**.
        | Attribute | Description |
        | --------- | ----------- |
        | Category | Select "FortiWeb" or the customized certificate store name from the previous step. |
        | Container | Optional container to associate certificate store with. |
        | Client Machine |  |
        | Store Path |  |
        | Orchestrator | Select an approved orchestrator capable of managing `FortiWeb` certificates. Specifically, one with the `FortiWeb` capability. |
        | ServerUsername | A username for CLI/SSH access.  Used for inventory. (or valid PAM key if the username is stored in a KF Command configured PAM integration). |
        | ServerPassword | A password for CLI/SSH access.  Used for inventory.(or valid PAM key if the password is stored in a KF Command configured PAM integration). |
        | ServerUseSsl | Should be true, http is not supported. |
        | ApiKey | API Key for access to FortiWeb REST API.  Generate Key on there platform and plug it in here. |


        

        <details><summary>Attributes eligible for retrieval by a PAM Provider on the Universal Orchestrator</summary>

        If a PAM provider was installed _on the Universal Orchestrator_ in the [Installation](#Installation) section, the following parameters can be configured for retrieval _on the Universal Orchestrator_.
        | Attribute | Description |
        | --------- | ----------- |
        | ServerUsername | A username for CLI/SSH access.  Used for inventory. (or valid PAM key if the username is stored in a KF Command configured PAM integration). |
        | ServerPassword | A password for CLI/SSH access.  Used for inventory.(or valid PAM key if the password is stored in a KF Command configured PAM integration). |


        > Any secret can be rendered by a PAM provider _installed on the Keyfactor Command server_. The above parameters are specific to attributes that can be fetched by an installed PAM provider running on the Universal Orchestrator server itself. 
        </details>
        

    3. **Import the CSV file to create the certificate stores** 

        ```shell
        kfutil stores import csv --store-type-name FortiWeb --file FortiWeb.csv
        ```
    </details>

> The content in this section can be supplimented by the [official Command documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/ReferenceGuide/Certificate%20Stores.htm?Highlight=certificate%20store).




## Client Machine Instructions

ToDo

## Developer Notes

ToDo


## License

Apache License 2.0, see [LICENSE](LICENSE).

## Related Integrations

See all [Keyfactor Universal Orchestrator extensions](https://github.com/orgs/Keyfactor/repositories?q=orchestrator).