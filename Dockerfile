# Используем официальный образ ASP.NET Core для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Используем .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EducationPlatformAPI.csproj", "./"]
RUN dotnet restore "./EducationPlatformAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "EducationPlatformAPI.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "EducationPlatformAPI.csproj" -c Release -o /app/publish

# Запускаем приложение из финального образа
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EducationPlatformAPI.dll"]
