# Persistence Layer

This layer contains all configuration to databases.

dotnet tool install -g dotnet-ef
dotnet ef migrations add <name> --project ./Core/AdsPortal.Persistence
dotnet ef migrations add <name> --no-build -v -o "Migrations" --json --prefix-output

e.g. dotnet ef migrations add --project ./Core/AdsPortal.Persistence Initial

Add-Migration <name> -project ./Core/AdsPortal.Persistence

e.g. Add-Migration -project ./Core/AdsPortal.Persistence Initial
