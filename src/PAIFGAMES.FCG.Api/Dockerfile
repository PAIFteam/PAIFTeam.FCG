FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PAIFGAMES.FCG.Infra/PAIFGAMES.FCG.Infra.csproj", "PAIFGAMES.FCG.Infra/"]
COPY ["PAIFGAMES.FCG.Domain/PAIFGAMES.FCG.Domain.csproj", "PAIFGAMES.FCG.Domain/"]
COPY ["PAIFGAMES.FCG.Api/PAIFGAMES.FCG.Api.csproj", "PAIFGAMES.FCG.Api/"]

RUN dotnet restore "PAIFGAMES.FCG.Api/PAIFGAMES.FCG.Api.csproj"

COPY . .
WORKDIR "/src/PAIFGAMES.FCG.Api"

FROM build AS publish
RUN dotnet publish "PAIFGAMES.FCG.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PAIFGAMES.FCG.Api.dll"]