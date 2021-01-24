#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebPortal/Presentation/AdsPortal.WebPortal/AdsPortal.WebPortal.csproj", "WebPortal/Presentation/AdsPortal.WebPortal/"]
COPY ["Shared/Infrastructure/AdsPortal.Shared.Infrastructure.Logging/AdsPortal.Shared.Infrastructure.Logging.csproj", "Shared/Infrastructure/AdsPortal.Shared.Infrastructure.Logging/"]
COPY ["Shared/Library/MagicOperations/MagicOperations.csproj", "Shared/Library/MagicOperations/"]
COPY ["Shared/Library/MagicModels/MagicModels.csproj", "Shared/Library/MagicModels/"]
COPY ["Shared/Library/StringUnformatter/StringUnformatter.csproj", "Shared/Library/StringUnformatter/"]
COPY ["Shared/Common/AdsPortal.Shared.Extensions/AdsPortal.Shared.Extensions.csproj", "Shared/Common/AdsPortal.Shared.Extensions/"]
RUN dotnet restore "WebPortal/Presentation/AdsPortal.WebPortal/AdsPortal.WebPortal.csproj"
COPY . .
WORKDIR "/src/WebPortal/Presentation/AdsPortal.WebPortal"
RUN dotnet build "AdsPortal.WebPortal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdsPortal.WebPortal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdsPortal.WebPortal.dll"]