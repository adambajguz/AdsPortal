# Persistence Layer

This layer contains all configuration to databases.

dotnet tool install -g dotnet-ef
or
dotnet tool update -g dotnet-ef

dotnet ef migrations add <name> --project ./Core/AdsPortal.Persistence
dotnet ef migrations add <name> --no-build -v -o "Migrations" --json --prefix-output

e.g. dotnet ef migrations add --project ./WebApi/Core/AdsPortal.WebApi.Persistence Initial

Add-Migration -project ./WebApi/Core/AdsPortal.WebApi.Persistence <name>

e.g. Add-Migration -project ./WebApi/Core/AdsPortal.WebApi.Persistence Initial
