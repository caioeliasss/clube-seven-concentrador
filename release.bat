@echo off
REM Publica um release no GitHub a partir da versao em <Version> do csproj.
REM   1) build-installer.bat -> installer\Output\ClubeSevenBridge-Setup-<ver>.exe
REM   2) gh release create v<ver> com o instalador anexado (o asset que o auto-update baixa).
REM Pre-requisitos: Inno Setup (iscc) e GitHub CLI (gh) com 'gh auth login' feito uma vez.
cd /d "%~dp0"

REM Versao: fonte unica e o <Version> do csproj.
for /f "usebackq delims=" %%V in (`powershell -NoProfile -Command "(Select-Xml -Path 'SevenConcentradorBridge.csproj' -XPath '//Version').Node.InnerText"`) do set "APPVER=%%V"
if "%APPVER%"=="" (
  echo ERRO: nao foi possivel ler ^<Version^> do csproj.
  pause
  exit /b 1
)

set "SETUP=installer\Output\ClubeSevenBridge-Setup-%APPVER%.exe"

echo === 1/2 Gerando instalador v%APPVER% ===
call build-installer.bat
if errorlevel 1 exit /b 1

if not exist "%SETUP%" (
  echo ERRO: instalador nao encontrado em %SETUP%
  pause
  exit /b 1
)

echo.
echo === 2/2 Publicando release v%APPVER% no GitHub ===
where gh >nul 2>&1
if errorlevel 1 (
  echo ERRO: GitHub CLI ^(gh^) nao encontrado no PATH. Instale e rode 'gh auth login'.
  pause
  exit /b 1
)

gh release create v%APPVER% "%SETUP%" --title "v%APPVER%" --notes "Release v%APPVER%"
if errorlevel 1 (
  echo.
  echo ERRO ao criar o release. A tag v%APPVER% ja existe? Use 'gh release upload v%APPVER% "%SETUP%" --clobber'.
  pause
  exit /b 1
)

echo.
echo Release v%APPVER% publicado. O auto-update dos clientes vai detectar em ate Update:IntervaloHoras.
pause
