@echo off
title Teste - Preco Bico 04 via C_SendReceiveText
set API=http://localhost:5100/api/concentrador
set KEY=TROCAR_POR_CHAVE_SEGURA

echo === GET /precos/04 (usa C_SendReceiveText: 14BB / 05BB03 / 05BB09) ===
curl -i -X GET "%API%/precos/04" -H "X-Api-Key: %KEY%"
echo.

pause
