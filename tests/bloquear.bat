@echo off
title Teste - Bloquear
echo === POST /api/concentrador/bloquear ===
echo Bico: 01
curl -s -X POST "http://localhost:5100/api/concentrador/bloquear" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json" ^
  -d "{\"bico\": \"01\"}"
pause
