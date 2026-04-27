@echo off
cd /d "%~dp0"
dotnet publish SevenConcentradorBridge.csproj -c Release -r win-x86 --self-contained false -o seven-concentrador-v0.0
copy /y iniciar.bat publish\iniciar.bat
echo.
echo Publicado! Rode publish\iniciar.bat para iniciar.
pause
