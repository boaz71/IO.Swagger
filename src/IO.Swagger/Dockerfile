# שלב בסיס
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# שלב build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/IO.Swagger/IO.Swagger.csproj", "src/IO.Swagger/"]
COPY ["NuGet.Config", "."]
RUN dotnet restore "src/IO.Swagger/IO.Swagger.csproj"

COPY . .
WORKDIR "/src/src/IO.Swagger"
RUN dotnet publish "IO.Swagger.csproj" -c Release -o /app/publish

# שלב סופי
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# הגדרת פורט להרצה
ENTRYPOINT ["dotnet", "IO.Swagger.dll", "--urls", "http://+:8080"]
