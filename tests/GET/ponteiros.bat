@echo off
title Teste - Ponteiros
echo === GET /api/concentrador/ponteiros ===
curl -i -X GET "http://localhost:5100/api/concentrador/ponteiros" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
