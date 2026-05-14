@echo off
title SevenConcentradorBridge - Dev Server
cd /d "%~dp0"

echo Starting dev server on http://localhost:5100 ...
echo Press Ctrl+C to stop.
echo.

dotnet run --project SevenConcentradorBridge.csproj --urls "http://localhost:5100"
