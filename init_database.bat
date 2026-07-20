@echo off
chcp 65001 >nul
setlocal

set "SERVER=%~1"
if "%SERVER%"=="" set "SERVER=."

set "SCRIPT=%~dp0database_query_and_data.sql"

if not exist "%SCRIPT%" (
    echo Database script not found: %SCRIPT%
    if /I not "%~2"=="--no-pause" pause
    exit /b 1
)

where sqlcmd >nul 2>nul
if errorlevel 1 (
    echo sqlcmd was not found. Please install SQL Server command line tools first.
    if /I not "%~2"=="--no-pause" pause
    exit /b 1
)

echo SQL Server: %SERVER%
echo Initializing BookSalesDB...

sqlcmd -S "%SERVER%" -E -b -f 65001 -i "%SCRIPT%"
if errorlevel 1 (
    echo.
    echo Database initialization failed. Check SQL Server service and database permissions.
    if /I not "%~2"=="--no-pause" pause
    exit /b 1
)

echo.
echo Database initialization completed. You can start WinFormsApp2 now.
if /I not "%~2"=="--no-pause" pause
