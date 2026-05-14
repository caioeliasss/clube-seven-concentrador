@echo off
title Teste - Preco Bico 04 protocolo RS-232 raw (&T04U..)
set API=http://localhost:5100/api/concentrador
set KEY=TROCAR_POR_CHAVE_SEGURA

echo === GET /preco-raw/04 (TX: "(^&T04U33)", esperado RX: "(TG04XXXXXXXXKK)") ===
curl -i -X GET "%API%/preco-raw/04" -H "X-Api-Key: %KEY%"
echo.

pause
