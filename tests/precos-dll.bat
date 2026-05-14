@echo off
title Teste - Precos DLL
set API=http://localhost:5100/api/concentrador
set KEY=TROCAR_POR_CHAVE_SEGURA

echo === GET /precos-dll (todos, niveis=2) ===
curl -i -X GET "%API%/precos-dll?niveis=2" -H "X-Api-Key: %KEY%"
echo.

echo === GET /precos-dll/04 (niveis=2) ===
curl -i -X GET "%API%/precos-dll/04?niveis=2" -H "X-Api-Key: %KEY%"
echo.

echo === GET /precos-dll/05 (niveis=2) ===
curl -i -X GET "%API%/precos-dll/05?niveis=2" -H "X-Api-Key: %KEY%"
echo.

echo === GET /precos-dll/04 (niveis=0) ===
curl -i -X GET "%API%/precos-dll/04?niveis=0" -H "X-Api-Key: %KEY%"
echo.

pause
