@echo off
title Teste - Visualizacao SSE (stream)
echo === GET /api/concentrador/visualizacao/stream (SSE) ===
echo Ctrl+C para encerrar
echo.
curl -N --no-buffer -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  "http://localhost:5100/api/concentrador/visualizacao/stream?intervaloMs=200"
pause
