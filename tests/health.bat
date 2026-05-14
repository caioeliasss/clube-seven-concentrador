@echo off
title Teste - Health
echo === GET /api/concentrador/health ===
curl -i -X GET "http://localhost:5100/api/concentrador/health"
echo.
pause
