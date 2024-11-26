@echo off


set clientmachine=%FortiWebMachine%
set password=%FortiWebPassword%
set user=%FortiWebUser%
set storepath=/

echo ***********************************
echo Starting Management Test Cases
echo ***********************************
set casename=Management

GOTO:TC3

set cert=%random%
set mgt=add
set overwrite=false

echo ************************************************************************************************************************
echo TC1 %mgt%.  Add No Existing Bindings no overwrite will not do the %mgt% since there is no existing binding for this cert
echo ************************************************************************************************************************
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%


set overwrite=true

echo:
echo ************************************************************************************************************************
echo TC2 %mgt%.  Add No Existing Bindings w/ overwrite will not do the %mgt% since there is no existing binding for this cert
echo ************************************************************************************************************************
echo overwrite: %overwrite%
echo trusted: %trusted%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%

:TC3
set mgt=add
set overwrite=true

echo:
echo **************************************************************************************************************
echo TC3 Case Try to replace a bound cert bound to multiple policies, this should work and replace everywhere
echo **************************************************************************************************************
echo overwrite: %overwrite%
set /p cert=Please enter bound cert name:
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%

set mgt=add
set overwrite=false

echo:
echo *************************************************************************************************************
echo TC4 Case No Overwrite with biding information.  Should warn the user that the need the overwrite flag checked
echo *************************************************************************************************************
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%


echo:
echo ***************************************************
echo TC5 Invalid Store Path - Job should fail with error
echo ****************************************************
set storepath=/config
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%

:fortiwebinventory
echo:
echo:
echo ***********************************
echo Starting Inventory Test Cases
echo ***********************************
set storepath=/
set casename=Inventory

echo:
echo *************************************************************************************************
echo TC6 Firewall Inventory against firewall should return job status of "2" with no errors no Trusted
echo *************************************************************************************************
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -inventorytrusted=%inventorytrusted% -templatestackname=%templatestackname%

@pause
