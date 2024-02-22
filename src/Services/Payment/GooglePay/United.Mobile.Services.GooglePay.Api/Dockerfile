#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Services/Payment/GooglePay/United.Mobile.Services.GooglePay.Api/United.Mobile.Services.GooglePay.Api.csproj", "src/Services/Payment/GooglePay/United.Mobile.Services.GooglePay.Api/"]
COPY ["src/Services/Payment/GooglePay/United.Mobile.Services.GooglePay.Domain/United.Mobile.Services.GooglePay.Domain.csproj", "src/Services/Payment/GooglePay/United.Mobile.Services.GooglePay.Domain/"]
COPY ["src/DataAccess/United.Mobile.DataAccess.DynamoDB/United.Mobile.DataAccess.DynamoDB.csproj", "src/DataAccess/United.Mobile.DataAccess.DynamoDB/"]
COPY ["src/DataAccess/United.Mobile.DataAccess.Common/United.Mobile.DataAccess.Common.csproj", "src/DataAccess/United.Mobile.DataAccess.Common/"]
COPY ["src/Core/United.Utility/United.Utility.csproj", "src/Core/United.Utility/"]
COPY ["src/Core/United.Ebs.Logging/United.Ebs.Logging.csproj", "src/Core/United.Ebs.Logging/"]
COPY ["src/Model/United.Mobile.Model.Common/United.Mobile.Model.Common.csproj", "src/Model/United.Mobile.Model.Common/"]
COPY ["src/Model/SeatEngine/United.Mobile.Model.SeatEngine/United.Mobile.Model.SeatEngine.csproj", "src/Model/SeatEngine/United.Mobile.Model.SeatEngine/"]
COPY ["src/Model.Internal/United.Mobile.Model.Internal.Exception/United.Mobile.Model.Internal.Exception.csproj", "src/Model.Internal/United.Mobile.Model.Internal.Exception/"]
COPY ["src/Model.Internal/United.Definition/United.Mobile.Model.Internal.Common.csproj", "src/Model.Internal/United.Definition/"]
COPY ["src/Model.Internal/United.Mobile.Model.Internal/United.Mobile.Model.Internal.csproj", "src/Model.Internal/United.Mobile.Model.Internal/"]
COPY ["src/Model/eRes/United.Mobile.Model.HomePageContent/United.Mobile.Model.HomePageContent.csproj", "src/Model/eRes/United.Mobile.Model.HomePageContent/"]
COPY ["src/Model/Payment/United.Mobile.Model.GooglePay/United.Mobile.Model.GooglePay.csproj", "src/Model/Payment/United.Mobile.Model.GooglePay/"]
COPY ["src/DataAccess/Payment/United.Mobile.DataAccess.Payment/United.Mobile.DataAccess.Payment.csproj", "src/DataAccess/Payment/United.Mobile.DataAccess.Payment/"]
COPY ["src/Core/United.Utility.Middleware/United.Utility.Middleware.csproj", "src/Core/United.Utility.Middleware/"]
RUN dotnet restore "src/Services/Payment/GooglePay/United.Mobile.Services.GooglePay.Api/United.Mobile.Services.GooglePay.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Payment/GooglePay/United.Mobile.Services.GooglePay.Api"
RUN dotnet build "United.Mobile.Services.GooglePay.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "United.Mobile.Services.GooglePay.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "United.Mobile.Services.GooglePay.Api.dll"]