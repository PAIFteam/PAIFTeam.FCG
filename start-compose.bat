@echo off
cd /d "%~dp0"
echo Inicializando ambiente Docker...

docker compose -f docker-compose.yml down
docker compose -f docker-compose.yml up --build -d

echo Ambiente iniciado com sucesso!
pause
