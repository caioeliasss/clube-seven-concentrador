@echo off
title Teste - Precos DLL
echo === GET /api/concentrador/precos-dll?niveis=0 ===
curl -s -X GET "http://localhost:5100/api/concentrador/precos-dll?niveis=0" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json"
pause
