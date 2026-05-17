@echo off
title Teste - Autorizar
echo === POST /api/concentrador/autorizar ===
echo Bico: 01
curl -i -X POST "http://localhost:5100/api/concentrador/autorizar" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json" ^
  -d "{\"bico\": \"01\"}"
pause
