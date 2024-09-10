set NSWAG_VER="13.18.2"

ECHO OFF
ECHO ...
ECHO --- UPDATE NSWAG TO %NSWAG_VER% ---

(
echo {
echo     "name": "tools",
echo     "scripts": {},
echo     "private": true,
echo     "dependencies": {},
echo     "devDependencies": {
echo         "nswag": %NSWAG_VER%
echo     }
echo }
) > package.json

npm list nswag --depth=0 | find /i %NSWAG_VER%

if not errorlevel 1 (goto GENERATE_API_CLIENTS)

call npm install nswag --save-dev

:GENERATE_API_CLIENTS

set BASE_DIR=%CD%

cd %BASE_DIR%/src/Aurigma.DirectMail.Sample.TypeScriptApiClient/

call refresh.bat

cd %BASE_DIR%
