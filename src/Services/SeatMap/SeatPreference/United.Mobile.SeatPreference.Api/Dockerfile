#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM artifactorycloud.ual.com/l-docker-releases/datadog/dotnet/aspnet:6.0.5-2.10.0 AS base

WORKDIR /app
EXPOSE 80

FROM artifactorycloud.ual.com/l-docker/ms/dotnet/core/sdk:6.0 AS build
WORKDIR /

COPY ["src/Services/SeatMap/SeatPreference/United.Mobile.SeatPreference.Api/United.Mobile.SeatPreference.Api.csproj", "src/Services/SeatMap/SeatPreference/United.Mobile.SeatPreference.Api/"]
COPY ["src/Core/United.Utility.Middleware/United.Utility.Middleware.csproj", "src/Core/United.Utility.Middleware/"]
COPY ["src/Model/United.Mobile.Model.Common/United.Mobile.Model.Common.csproj", "src/Model/United.Mobile.Model.Common/"]
COPY ["src/Model.Internal/United.Mobile.Model.Internal.Exception/United.Mobile.Model.Internal.Exception.csproj", "src/Model.Internal/United.Mobile.Model.Internal.Exception/"]
COPY ["src/Model/SeatEngine/United.Mobile.Model.SeatEngine/United.Mobile.Model.SeatEngine.csproj", "src/Model/SeatEngine/United.Mobile.Model.SeatEngine/"]
COPY ["src/Core/United.Ebs.Logging/United.Ebs.Logging.csproj", "src/Core/United.Ebs.Logging/"]
COPY ["src/Core/United.Utility/United.Utility.csproj", "src/Core/United.Utility/"]
COPY ["src/Services/SeatMap/SeatPreference/United.Mobile.SeatPreference.Domain/United.Mobile.SeatPreference.Domain.csproj", "src/Services/SeatMap/SeatPreference/United.Mobile.SeatPreference.Domain/"]
COPY ["src/Core/Common/United.Common.Helper.Shopping/United.Common.Helper.Shopping.csproj", "src/Core/Common/United.Common.Helper.Shopping/"]
COPY ["src/DataAccess/AccountManagement/United.Mobile.DataAccess.Customer/United.Mobile.DataAccess.Customer.csproj", "src/DataAccess/AccountManagement/United.Mobile.DataAccess.Customer/"]
COPY ["src/DataAccess/AccountManagement/United.Mobile.DataAccess.Profile/United.Mobile.DataAccess.Profile.csproj", "src/DataAccess/AccountManagement/United.Mobile.DataAccess.Profile/"]
COPY ["United.Mobile.Model.CSLModels/United.Mobile.Model.CSLModels.csproj", "United.Mobile.Model.CSLModels/"]
COPY ["src/DataAccess/BagCalculator/United.Mobile.DataAccess.CMSContent/United.Mobile.DataAccess.CMSContent.csproj", "src/DataAccess/BagCalculator/United.Mobile.DataAccess.CMSContent/"]
COPY ["src/DataAccess/ManageReservation/United.Mobile.DataAccess.ManageReservation/United.Mobile.DataAccess.ManageReservation.csproj", "src/DataAccess/ManageReservation/United.Mobile.DataAccess.ManageReservation/"]
COPY ["src/DataAccess/Shopping/United.Mobile.DataAccess.Shopping/United.Mobile.DataAccess.Shopping.csproj", "src/DataAccess/Shopping/United.Mobile.DataAccess.Shopping/"]
COPY ["src/DataAccess/Shopping/United.Mobile.DataAccess.ShopTrips/United.Mobile.DataAccess.ShopTrips.csproj", "src/DataAccess/Shopping/United.Mobile.DataAccess.ShopTrips/"]
COPY ["src/DataAccess/United.Mobile.DataAccess.Common/United.Mobile.DataAccess.Common.csproj", "src/DataAccess/United.Mobile.DataAccess.Common/"]
COPY ["src/Model.Internal/United.Definition/United.Mobile.Model.Internal.Common.csproj", "src/Model.Internal/United.Definition/"]
COPY ["src/Model.Internal/United.Mobile.Model.Internal/United.Mobile.Model.Internal.csproj", "src/Model.Internal/United.Mobile.Model.Internal/"]
COPY ["src/DataAccess/United.Mobile.DataAccess.FlightShopping/United.Mobile.DataAccess.FlightShopping.csproj", "src/DataAccess/United.Mobile.DataAccess.FlightShopping/"]
COPY ["src/DataAccess/United.Mobile.DataAccess.MerchandizeService/United.Mobile.DataAccess.MerchandizeService.csproj", "src/DataAccess/United.Mobile.DataAccess.MerchandizeService/"]
COPY ["src/Model.Internal/AccountManagement/United.Mobile.Model.Internal.AccountManagement/United.Mobile.Model.Internal.AccountManagement.csproj", "src/Model.Internal/AccountManagement/United.Mobile.Model.Internal.AccountManagement/"]
COPY ["src/Model/Shopping/United.Mobile.Model.EmpShop/United.Mobile.Model.EmpShop.csproj", "src/Model/Shopping/United.Mobile.Model.EmpShop/"]
COPY ["src/Model/ManageRes/United.Mobile.Model.ManageRes/United.Mobile.Model.ManageRes.csproj", "src/Model/ManageRes/United.Mobile.Model.ManageRes/"]
COPY ["src/DataAccess/Ancillary/United.Mobile.DataAccess.Fitbit/United.Mobile.DataAccess.Fitbit.csproj", "src/DataAccess/Ancillary/United.Mobile.DataAccess.Fitbit/"]
COPY ["src/Model/AccountManagement/United.Mobile.Model.MPRewards/United.Mobile.Model.MPRewards.csproj", "src/Model/AccountManagement/United.Mobile.Model.MPRewards/"]
COPY ["src/Model/Ancillary/United.Mobile.Model.Fitbit/United.Mobile.Model.Fitbit.csproj", "src/Model/Ancillary/United.Mobile.Model.Fitbit/"]
COPY ["src/Model/AccountManagement/United.Mobile.Model.AccountManagement/United.Mobile.Model.MPSignIn.csproj", "src/Model/AccountManagement/United.Mobile.Model.AccountManagement/"]
COPY ["src/Model/Shopping/United.Mobile.Model.Booking/United.Mobile.Model.Booking.csproj", "src/Model/Shopping/United.Mobile.Model.Booking/"]
COPY ["src/Model/Shopping/United.Mobile.Model.Shopping/United.Mobile.Model.Shopping.csproj", "src/Model/Shopping/United.Mobile.Model.Shopping/"]
COPY ["src/Model/PNRManagement/United.Mobile.Model.PNRManagement/United.Mobile.Model.PNRManagement.csproj", "src/Model/PNRManagement/United.Mobile.Model.PNRManagement/"]
COPY ["src/Model/Shopping/United.Mobile.Model.ShopAward/United.Mobile.Model.ShopAward.csproj", "src/Model/Shopping/United.Mobile.Model.ShopAward/"]
COPY ["src/Model/Shopping/United.Mobile.Model.Common/United.Mobile.Model.Shopping.Common.csproj", "src/Model/Shopping/United.Mobile.Model.Common/"]
COPY ["src/Model/Ancillary/United.Mobile.Model.Catalog/United.Mobile.Model.Catalog.csproj", "src/Model/Ancillary/United.Mobile.Model.Catalog/"]
COPY ["src/Model/AccountManagement/United.Mobile.Model.MemberProfile/United.Mobile.Model.MemberProfile.csproj", "src/Model/AccountManagement/United.Mobile.Model.MemberProfile/"]
COPY ["src/Model/AccountManagement/United.Mobile.Model.UpdateMemberProfile/United.Mobile.Model.UpdateMemberProfile.csproj", "src/Model/AccountManagement/United.Mobile.Model.UpdateMemberProfile/"]
COPY ["src/Model/Ancillary/United.Mobile.Model.FeedBack/United.Mobile.Model.FeedBack.csproj", "src/Model/Ancillary/United.Mobile.Model.FeedBack/"]
COPY ["src/Model/Payment/United.Mobile.Model.Payment/United.Mobile.Model.Payment.csproj", "src/Model/Payment/United.Mobile.Model.Payment/"]
COPY ["src/Model/ReShopping/United.Mobile.Model.ReShop/United.Mobile.Model.ReShop.csproj", "src/Model/ReShopping/United.Mobile.Model.ReShop/"]
COPY ["src/Model/Shopping/United.Mobile.Model.ShopTrips/United.Mobile.Model.ShopTrips.csproj", "src/Model/Shopping/United.Mobile.Model.ShopTrips/"]
COPY ["src/Model/Shopping/United.Mobile.Model.TripPlannerGetService/United.Mobile.Model.TripPlannerGetService.csproj", "src/Model/Shopping/United.Mobile.Model.TripPlannerGetService/"]
COPY ["src/Core/United.Common.Helper/United.Common.Helper.csproj", "src/Core/United.Common.Helper/"]
COPY ["src/DataAccess/OnPrem/United.Mobile.DataAccess.OnPremiseSQLSP/United.Mobile.DataAccess.OnPremiseSQLSP.csproj", "src/DataAccess/OnPrem/United.Mobile.DataAccess.OnPremiseSQLSP/"]
COPY ["src/Model/BagCalculator/United.Mobile.Model.BagCalculator/United.Mobile.Model.BagCalculator.csproj", "src/Model/BagCalculator/United.Mobile.Model.BagCalculator/"]
COPY ["src/DataAccess/United.Mobile.DataAccess.DynamoDB/United.Mobile.DataAccess.DynamoDB.csproj", "src/DataAccess/United.Mobile.DataAccess.DynamoDB/"]
COPY ["src/Model.Internal/Shopping/United.Mobile.Model.Shopping.Internal/United.Mobile.Model.Shopping.Internal.csproj", "src/Model.Internal/Shopping/United.Mobile.Model.Shopping.Internal/"]
COPY ["src/DataAccess/SeatEngine/United.Mobile.DataAccess.SeatPreference/United.Mobile.DataAccess.SeatPreference.csproj", "src/DataAccess/SeatEngine/United.Mobile.DataAccess.SeatPreference/"]
COPY ["src/Model/SeatMap/United.Mobile.Model.SeatMap/United.Mobile.Model.SeatMap.csproj", "src/Model/SeatMap/United.Mobile.Model.SeatMap/"]
COPY ["src/Model/Shopping/United.Mobile.Model.ShopSeats/United.Mobile.Model.ShopSeats.csproj", "src/Model/Shopping/United.Mobile.Model.ShopSeats/"]


ARG NUGET_PASSWORD
ARG NUGET_USERNAME


RUN dotnet nuget add source https://artifactorycloud.ual.com/artifactory/api/nuget/v3/v-nuget \
            -n artifactory \
            -u ${NUGET_USERNAME}  \
            -p ${NUGET_PASSWORD} \
            --store-password-in-clear-text


COPY . .


WORKDIR "/src/Services/SeatMap/SeatPreference/United.Mobile.SeatPreference.Api"

FROM build AS publish
RUN dotnet publish "United.Mobile.SeatPreference.Api.csproj" -c Release --self-contained -r linux-x64 -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "United.Mobile.SeatPreference.Api.dll"]