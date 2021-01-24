#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebApi/Presentation/AdsPortal.WebApi/AdsPortal.WebApi.csproj", "WebApi/Presentation/AdsPortal.WebApi/"]
COPY ["Shared/Infrastructure/AdsPortal.Shared.Infrastructure.Logging/AdsPortal.Shared.Infrastructure.Logging.csproj", "Shared/Infrastructure/AdsPortal.Shared.Infrastructure.Logging/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Infrastructure/AdsPortal.WebApi.Infrastructure.csproj", "WebApi/Core/AdsPortal.WebApi.Infrastructure/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Application/AdsPortal.WebApi.Application.csproj", "WebApi/Core/AdsPortal.WebApi.Application/"]
COPY ["Shared/Common/AdsPortal.Shared.Extensions/AdsPortal.Shared.Extensions.csproj", "Shared/Common/AdsPortal.Shared.Extensions/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Domain/AdsPortal.WebApi.Domain.csproj", "WebApi/Core/AdsPortal.WebApi.Domain/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Domain.Abstractions/AdsPortal.WebApi.Domain.Abstractions.csproj", "WebApi/Core/AdsPortal.WebApi.Domain.Abstractions/"]
COPY ["Shared/Library/AutoMapper.Extensions/AutoMapper.Extensions.csproj", "Shared/Library/AutoMapper.Extensions/"]
COPY ["Shared/Library/MediatR.GenericOperations/MediatR.GenericOperations.csproj", "Shared/Library/MediatR.GenericOperations/"]
COPY ["WebApi/Presentation/AdsPortal.WebApi.Grpc/AdsPortal.WebApi.Grpc.csproj", "WebApi/Presentation/AdsPortal.WebApi.Grpc/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Infrastructure.Identity/AdsPortal.WebApi.Infrastructure.Identity.csproj", "WebApi/Core/AdsPortal.WebApi.Infrastructure.Identity/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Infrastructure.JobScheduler/AdsPortal.WebApi.Infrastructure.JobScheduler.csproj", "WebApi/Core/AdsPortal.WebApi.Infrastructure.JobScheduler/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Infrastructure.Mailing/AdsPortal.WebApi.Infrastructure.Mailing.csproj", "WebApi/Core/AdsPortal.WebApi.Infrastructure.Mailing/"]
COPY ["WebApi/Presentation/AdsPortal.WebApi.Rest/AdsPortal.WebApi.Rest.csproj", "WebApi/Presentation/AdsPortal.WebApi.Rest/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Infrastructure.Media/AdsPortal.WebApi.Infrastructure.Media.csproj", "WebApi/Core/AdsPortal.WebApi.Infrastructure.Media/"]
COPY ["WebApi/Core/AdsPortal.WebApi.Persistence/AdsPortal.WebApi.Persistence.csproj", "WebApi/Core/AdsPortal.WebApi.Persistence/"]

RUN dotnet restore "WebApi/Presentation/AdsPortal.WebApi/AdsPortal.WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi/Presentation/AdsPortal.WebApi"
RUN dotnet build "AdsPortal.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdsPortal.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdsPortal.WebApi.dll"]