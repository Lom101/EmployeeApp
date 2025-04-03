# Используем .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY ../EmployeeApp.sln ./
COPY EmployeeApp.Backend/*.csproj EmployeeApp.Backend/
RUN dotnet restore EmployeeApp.Backend/EmployeeApp.Backend.csproj

# Копируем остальной код и собираем приложение
COPY EmployeeApp.Backend/. EmployeeApp.Backend/
WORKDIR /app/EmployeeApp.Backend
RUN dotnet publish -c Release -o /out

# Используем более легкий runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Запускаем приложение
ENTRYPOINT ["dotnet", "EmployeeApp.Backend.dll"]