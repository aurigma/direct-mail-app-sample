rem: Folder 'TheMailworks/src/Aurigma.DirectMail.Sample.TypeScriptApiClient'
set currentDir=%CD%

@echo off
echo ...
echo --- UPDATE DIRECT MAIL BACKEND API CLIENT ---

rem: Going to project root directory
cd ..
cd ..

:CHECK_TOOL_MANIFEST

if exist ./.config/dotnet-tools.json goto CHECK_SWASHBUCLE_CLI

dotnet new tool-manifest

:CHECK_SWASHBUCLE_CLI

dotnet tool install --version 6.5.0 Swashbuckle.AspNetCore.Cli

:CHECKNPM

if exist ./node_modules/nswag/bin/nswag.js goto REFRESH

call npm install nswag --save-dev

:REFRESH

cd %currentDir%/../Aurigma.DirectMail.Sample.WebHost/bin/net8.0/

rem: Replacing the configuration file with an empty one
@echo off
ren appSettings.json appSettingsSaved.json
echo {}> appSettings.json


rem: Generating a swagger.json file
dotnet swagger tofile --output ../../../Aurigma.DirectMail.Sample.TypeScriptApiClient/swagger.json ./Aurigma.DirectMail.Sample.WebHost.dll v1

rem: Executing an .swag configuration document
cd %currentDir%/Axios/
call "%currentDir%/../../node_modules/.bin/nswag" run

rem: Restoring the configuration file
cd %currentDir%/../Aurigma.DirectMail.Sample.WebHost/bin/net8.0/
del appSettings.json
ren appSettingsSaved.json appSettings.json
