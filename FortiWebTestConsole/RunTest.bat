@echo off

cd C:\Users\bhill\source\repos\fortinet-fortiweb-orchestrator\FortiWebTestConsole\bin\Debug\netcoreapp3.1
set FortiWebMachine=11.22.38.208:8443
set FortiWebUser=dasklfa
set FortiWebPassword=asdfsa
set FortiWebApiKey=eyJ

set clientmachine=%FortiWebMachine%
set password=%FortiWebPassword%
set user=%FortiWebUser%
set storepath=/

echo ***********************************
echo Starting Management Test Cases
echo ***********************************
set casename=Management

#GOTO:TC3

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
set /p cert=Please enter multi policy bound cert name:
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%

:TC4
set mgt=add
set overwrite=true

echo:
echo **************************************************************************************************************
echo TC4 Case Try to replace a bound cert bound single policy, this should work and replace single bound cert
echo **************************************************************************************************************
echo overwrite: %overwrite%
set /p cert=Please enter single policy bound cert name:
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%


:TC5
set mgt=add
set overwrite=false

echo:
echo **************************************************************************************************************
echo TC5 Case Try to replace a bound cert bound single policy no overwrite, should fail saying overwrite is needed
echo **************************************************************************************************************
echo overwrite: %overwrite%
set /p cert=Please enter single policy bound cert name:
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -certalias=%cert% -overwrite=%overwrite%

:TC6
echo:
echo:
echo ***********************************
echo Starting Inventory Test Cases
echo ***********************************
set storepath=/
set casename=Inventory

echo:
echo *************************************************************************************************
echo TC6 Fortiweb Inventory will only inventory bound certificates should return 2 status
echo *************************************************************************************************
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -inventorytrusted=%inventorytrusted% -templatestackname=%templatestackname%

:TC7
echo:
echo:
echo ***********************************
echo Error Test Cases
echo ***********************************
set storepath=/
set casename=Inventory
set clientmachine=20.10.138.211:8443

echo:
echo *************************************************************************************************
echo TC7 Invalid Client Machine Should Return Reasonable Error
echo *************************************************************************************************
echo overwrite: %overwrite%
echo cert name: %cert%

FortiWebTestConsole.exe -clientmachine=%clientmachine% -casename=%casename% -user=%user% -password=%password% -storepath=%storepath% -apikey=%FortiWebApiKey% -managementtype=%mgt% -inventorytrusted=%inventorytrusted% -templatestackname=%templatestackname%

@pause
