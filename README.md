# AdsPortal
AdsPortal application built using ASP.NET Core and Entity Framework Core.

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [Visual Studio Code or Visual Studio 2019](https://visualstudio.microsoft.com/vs/) (version 16.8 or later)
* [.NET Core SDK 5.0](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [SwitchStartupProject](https://marketplace.visualstudio.com/items?itemName=vs-publisher-141975.SwitchStartupProjectForVS2019) extension for Visual Studio 2019

## License

This project is licensed under the MIT License

## JWT Key Generation

```
node -e "console.log(require('crypto').randomBytes(256).toString('base64'));"
```
