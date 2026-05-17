@echo off
title Teste - Alterar Preco
echo === POST /api/concentrador/preco ===
echo Bico: 04  Preco: 5990 (R$5,990 — 3 decimais)
curl -i -X POST "http://localhost:5100/api/concentrador/preco" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json" ^
  -d "{\"bico\": \"04\", \"preco\": \"5990\"}"
pause
