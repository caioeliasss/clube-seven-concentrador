@echo off
title Teste - Status
echo === GET /api/concentrador/status ===
curl -s -X GET "http://localhost:5100/api/concentrador/status" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
