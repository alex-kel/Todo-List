1. Поднять SQL-Server (одним из этих вариантов):
   1. Локальная установка и применение ./init/init.sql
   2. Запуск в контейнере - в docker-compose проекте запустить сначала sql-server, а затем sql-server-configurator
2. Запустить:
   ```bash
   dotnet ef database update 
   ```
3. Запустить приложение (можно это также сделать в docker контейнере)