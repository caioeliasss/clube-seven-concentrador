@echo off
title Teste - Visualizacao SSE (bico filtrado)
set BICO=%1
if "%BICO%"=="" set BICO=04
echo === GET /api/concentrador/visualizacao/stream?bico=%BICO% ===
echo Ctrl+C para encerrar
echo.
curl -N --no-buffer -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  "http://localhost:5100/api/concentrador/visualizacao/stream?intervaloMs=50&bico=04"
pause
