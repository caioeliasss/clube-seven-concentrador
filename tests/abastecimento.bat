@echo off
title Teste - Abastecimento
echo === GET /api/concentrador/abastecimento ===
curl -s -X GET "http://localhost:5100/api/concentrador/abastecimento" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
