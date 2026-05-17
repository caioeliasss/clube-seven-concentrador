@echo off
title Teste - Ler Registro
set /p POS=Posicao (0-9999):
echo === GET /api/concentrador/registro/%POS% ===
curl -i -X GET "http://localhost:5100/api/concentrador/registro/%POS%" ^
  -H "X-Api-Key: TROCAR_POR_CHAVE_SEGURA"
pause
