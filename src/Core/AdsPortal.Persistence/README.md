# Persistence Layer

This layer contains all configuration to databases.

dotnet tool install -g dotnet-ef
dotnet ef migrations add <name> --project ./Core/AdsPortal.Persistence
dotnet ef migrations add <name> --no-build -v -o "Migrations" --json --prefix-output

e.g. dotnet ef migrations add Initial --project ./Core/AdsPortal.Persistence

Add-Migration <name> -project ./Core/AdsPortal.Persistence

e.g. Add-Migration Initial -project ./Core/AdsPortal.Persistence
