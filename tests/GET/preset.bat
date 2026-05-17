@echo off
title Teste - Preset
echo === POST /api/concentrador/preset ===
echo Bico: 01  Valor: 50.00
curl -i -X POST "http://localhost:5100/api/concentrador/preset" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json" ^
  -d "{\"bico\": \"01\", \"valor\": \"50.00\"}"
pause
