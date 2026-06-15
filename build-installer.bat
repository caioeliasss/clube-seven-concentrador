@echo off
REM Gera o instalador setup.exe.
REM Pre-requisito: Inno Setup instalado e iscc.exe no PATH
REM   (https://jrsoftware.org/isdl.php). Caso nao esteja no PATH, ajuste ISCC abaixo.
cd /d "%~dp0"

set ISCC=iscc
where %ISCC% >nul 2>&1
if errorlevel 1 (
  if exist "%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" set "ISCC=%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe"
)

echo === 1/2 Publicando self-contained ===
call publicar-instalador.bat
if errorlevel 1 exit /b 1

echo.
echo === 2/2 Compilando instalador (Inno Setup) ===
"%ISCC%" installer\setup.iss
if errorlevel 1 (
  echo.
  echo ERRO ao compilar instalador. Verifique se o Inno Setup esta instalado.
  pause
  exit /b 1
)

echo.
echo Instalador gerado em installer\Output\
pause
