@echo off
REM Publica self-contained (x86, single-file) para empacotar no instalador.
REM Saida: dist\  -> nao exige .NET runtime no PC de destino.
cd /d "%~dp0"

if exist dist rmdir /s /q dist

dotnet publish SevenConcentradorBridge.csproj -c Release -r win-x86 --self-contained true -o dist
if errorlevel 1 (
  echo.
  echo ERRO no publish.
  pause
  exit /b 1
)

echo.
echo Verificando arquivos esperados em dist\ ...
if not exist "dist\SevenConcentradorBridge.exe" echo [FALTA] SevenConcentradorBridge.exe
if not exist "dist\companytec.dll"               echo [FALTA] companytec.dll
if not exist "dist\appsettings.json"             echo [FALTA] appsettings.json
if not exist "dist\wwwroot\index.html"           echo [FALTA] wwwroot\index.html

echo.
echo Publicado em dist\
