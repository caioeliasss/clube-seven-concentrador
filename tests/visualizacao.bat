@echo off
title Teste - Visualizacao
echo === GET /api/concentrador/visualizacao ===
curl -s -X GET "http://localhost:5100/api/concentrador/visualizacao" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
