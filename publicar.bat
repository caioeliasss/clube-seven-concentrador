@echo off
cd /d "%~dp0"
dotnet publish SevenConcentradorBridge.csproj -c Release -r win-x86 --self-contained false -o clube-seven-concentrador-v0.0
copy /y iniciar.bat clube-seven-concentrador-v0.0\iniciar.bat
echo.
xcopy /y /i /e tests clube-seven-concentrador-v0.0\tests
echo Publicado!
pause
