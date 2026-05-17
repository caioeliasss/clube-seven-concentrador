@echo off
title Teste - Abastecimento
echo === GET /api/concentrador/abastecimento ===
curl -i -X GET "http://localhost:5100/api/concentrador/abastecimento" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
