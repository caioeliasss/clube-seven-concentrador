@echo off
title Teste - Preco por Bico
echo === GET /api/concentrador/precos/{bico} ===
set BICO=04
echo Bico: %BICO%
curl -i -X GET "http://localhost:5100/api/concentrador/precos-dll/%BICO%" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
