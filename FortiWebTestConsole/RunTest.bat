@echo off



set clientmachine=%FortiWebMachine%
set password=%FortiWebPassword%
set user=%FortiWebUser%
set storepath=/

#GOTO:fortiwebinventory

echo ***********************************
echo Starting Management Test Cases
echo ***********************************
set casename=Management
set cert=20062
set mgt=add
set overwrite=true

echo ************************************************************************************************************************
echo TC1 %mgt%.  Should do the %mgt% and add anything in the chain
echo ************************************************************************************************************************
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%


set overwrite=true

echo:
echo *******************************************************************************************************
echo TC2 %mgt%.  Should do the %mgt% and replace the certificate with a new name
echo *******************************************************************************************************
echo overwrite: %overwrite%
echo trusted: %trusted%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%


set mgt=remove
set overwrite=true

echo:
echo **************************************************************************************************************
echo TC3 Case Try to remove a bound cert, should not be allowed unless you want to delete the binding too not good
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
