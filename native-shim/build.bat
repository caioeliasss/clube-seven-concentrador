@echo off
REM Build script: companytec_shim.dll (Win32 / x86)
REM Requer Free Pascal Compiler instalado (fpc no PATH) ou Lazarus.
REM Download FPC: https://www.freepascal.org/download.html

setlocal
cd /d "%~dp0"

where fpc >nul 2>nul
if errorlevel 1 (
    echo [ERRO] fpc nao encontrado no PATH.
    echo Instale Free Pascal: https://www.freepascal.org/download.html
    exit /b 1
)

fpc -Mdelphi -Twin32 -Pi386 -O2 companytec_shim.dpr
if errorlevel 1 (
    echo [ERRO] Falha na compilacao.
    exit /b 1
)

if exist companytec_shim.dll (
    echo [OK] companytec_shim.dll gerado em %CD%
    REM Copia pra raiz do projeto (junto com companytec.dll)
    copy /Y companytec_shim.dll "..\companytec_shim.dll" >nul
    echo [OK] Copiado para ..\companytec_shim.dll
) else (
    echo [ERRO] companytec_shim.dll nao foi gerado.
    exit /b 1
)

endlocal
