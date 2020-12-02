# AdsPortal
AdsPortal application built using ASP.NET Core and Entity Framework Core.

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [Visual Studio Code or Visual Studio 2019](https://visualstudio.microsoft.com/vs/) (version 16.8 or later)
* [.NET Core SDK 5.0](https://dotnet.microsoft.com/download/dotnet-core/5.0)

### Setup
Follow these steps to get your development environment set up:

  1. Clone the repository
  2. At the root directory, restore required packages by running:
     ```
     dotnet restore
     ```
  3. Next, build the solution by running:
     ```
     dotnet build
     ```
  4. Next, launch the backend by running:
     ```
	 dotnet run
	 ```
  5. Launch [https://localhost:2137/api](http://localhost:2137/api) in your browser to view the API using Swagger

## License

This project is licensed under the MIT License

## JWT Key Generation

```
node -e "console.log(require('crypto').randomBytes(256).toString('base64'));"
```
