using System;
using System.Collections.Generic;
using System.Text;

namespace United.Utility.Enum
{
    public enum FlowType
    {
        ALL,
        BOOKING,
        POSTBOOKING,
        MANAGERES,
        BAGGAGECALCULATOR,
        INITIAL,
        EXCHANGE,
        VIEWRES,
        CHECKIN,
        ERES,
        RESHOP,
        VIEWRES_SEATMAP,
        VIEWRES_BUNDLES_SEATMAP,
        FARELOCK,
        UPGRADEMALL,
        FLIGHTSTATUS_SEATMAP,
        BOOKING_PREVIEW_SEATMAP,
        CHECKINSDC,
        SCHEDULECHANGE,
        SHOPBYMAP,
        MOBILECHECKOUT
    }

    public enum IOSCatalogEnum
    {
        EnableBuyMilesFeatureCatalogID = 11388,
        AwardStrikeThroughPricing = 11428,
        EnableOmnicartRC4CChanges = 11463,
        EnableTaskTrainedServiceAnimalFeature = 11587,
        EnableEditSearchOnFSRHeader = 11502,
        EnableU4BCorporateBooking = 11648,
        EnableNewPartnerAirlines = 11643,
        EnableWheelchairLinkUpdate = 11647,
        EnableBundlesInManageRes = 11741,
        DisablePKDispenserKeyFromCSL = 11757,
        EnablePOMDeepLinkInActivePNR = 12000,
        EnableSaveTriptoMyAccount = 12047,
        EnableMilesFOPForPaidSeats = 12032,
        EnableIBEBuyOutInViewRes = 12090,
        EnablePOMGenericMessage= 12096,
        EnableVerticalSeatMap = 11937,
        EnableSAFInViewres = 12249,
        EnableAddPetMR = 12224
    }

    public enum AndroidCatalogEnum
    {
        EnableBuyMilesFeatureCatalogID = 21388,
        AwardStrikeThroughPricing = 21428,
        EnableOmnicartRC4CChanges = 21463,
        EnableTaskTrainedServiceAnimalFeature = 21587,
        EnableEditSearchOnFSRHeader = 21502,
        EnableU4BCorporateBooking = 21648,
        EnableNewPartnerAirlines = 21643,
        EnableWheelchairLinkUpdate = 21647,
        EnableBundlesInManageRes = 21741,
        DisablePKDispenserKeyFromCSL = 21757,
        EnablePOMDeepLinkInActivePNR = 22000,
        EnableSaveTriptoMyAccount = 22047,
        EnableMilesFOPForPaidSeats = 22032,
        EnableIBEBuyOutInViewRes = 22090,
        EnablePOMGenericMessage = 22096,
        EnableVerticalSeatMap = 21937,
        EnableSAFInViewres = 22249,
        EnableAddPetMR = 22224
    }
    public enum SeatMapRequestType
    {
        E01,
        ChangeSeats
    }
}
