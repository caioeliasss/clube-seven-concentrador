@echo off
title Teste - Comando Nativo
echo === POST /api/concentrador/native ===
echo Comando: (^&T04U33)
curl -i -X POST "http://localhost:5100/api/concentrador/native" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA" ^
  -H "Content-Type: application/json" ^
  -d "{\"comando\": \"(^&T04U33)\"}"
pause
