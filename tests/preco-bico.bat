@echo off
title Teste - Preco por Bico
echo === GET /api/concentrador/precos/{bico} ===
set BICO=01
echo Bico: %BICO%
curl -s -X GET "http://localhost:5100/api/concentrador/precos/%BICO%" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
