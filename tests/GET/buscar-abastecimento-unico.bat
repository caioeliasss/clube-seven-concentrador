@echo off
title Teste - Buscar abastecimento
REM Uso: buscar-abastecimento.bat [bico] [litros] [data ISO] [tolMin]
REM Ex:  buscar-abastecimento.bat 04 12.34 2026-05-17T15:30 5

set BICO=%1
if "%BICO%"=="" set BICO=04
set LITROS=%2
set DATA=%3
set TOLMIN=%4
if "%TOLMIN%"=="" set TOLMIN=5

echo === GET /api/concentrador/abastecimento/buscar ===
echo bico=%BICO%  litros=%LITROS%  data=%DATA%  toleranciaMin=%TOLMIN%
echo.

curl -i -G "http://localhost:5100/api/concentrador/abastecimento/buscar" ^
      -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
      --data-urlencode "bico=04" ^
      --data-urlencode "litros=10" ^
      --data-urlencode "toleranciaLitros=2" ^
      --data-urlencode "data=2026-05-17T15:30" ^
      --data-urlencode "toleranciaMin=5000" ^
      --data-urlencode "maxScan=200"

pause
