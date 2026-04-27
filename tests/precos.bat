@echo off
title Teste - Precos
echo === GET /api/concentrador/precos ===
curl -s -X GET "http://localhost:5100/api/concentrador/precos" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json"
pause
