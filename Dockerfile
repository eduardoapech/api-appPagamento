# Etapa base com o runtime do ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build com o SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia todos os arquivos e restaura as dependências
COPY . .
RUN dotnet restore "./PagamentosApp.csproj"
RUN dotnet publish "./PagamentosApp.csproj" -c Release -o /app/publish

# Etapa final: runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PagamentosApp.dll"]
