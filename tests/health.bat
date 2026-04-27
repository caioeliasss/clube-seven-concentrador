@echo off
title Teste - Health
echo === GET /api/concentrador/health ===
curl -s -X GET "http://localhost:5100/api/concentrador/health"
pause
