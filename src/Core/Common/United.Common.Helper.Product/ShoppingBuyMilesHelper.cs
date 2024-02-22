using System;
using System.Globalization;
using System.Linq;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Helper;
using MOBSHOPPrice = United.Mobile.Model.TravelCredit.MOBSHOPPrice;
using MOBSHOPReservation = United.Mobile.Model.TravelCredit.MOBSHOPReservation;
using Reservation = United.Mobile.Model.TravelCredit.Reservation;

namespace United.Common.Helper.Product
{
    public class ShoppingBuyMilesHelper : IShoppingBuyMilesHelper
    {
        public void ApplyPriceChangesForBuyMiles(FlightReservationResponse flightReservationResponse, MOBSHOPReservation reservation = null,
            Reservation bookingPathReservation = null)
        {

            if (reservation != null)
            {
                if (reservation.Prices != null && reservation.Prices.Count > 0)
                {
                    UpdateGrandTotal(reservation);
                    UpdateMPFPriceTypeDescriptionForBuyMiles(reservation, flightReservationResponse);
                    UpdateTaxPriceTypeDescriptionForBuyMiles(reservation);
                    AddFarewayDescriptionForMultipaxForBuyMiles(reservation);
                    UpdateComplianceTaxesForBuyMiles(reservation);
                }
            }
            else if (bookingPathReservation != null)
            {
                UpdateGrandTotalForBookingReservation(bookingPathReservation);
                UpdateMPFPriceTypeDescriptionForBuyMilesForBookingReservation(bookingPathReservation, flightReservationResponse);
                UpdateTaxPriceTypeDescriptionForBuyMilesForBookingReservation(bookingPathReservation);
                AddFarewayDescriptionForMultipaxForBuyMilesForBookingReservation(bookingPathReservation);
                UpdateComplianceTaxesForBuyMilesForBookingReservation(bookingPathReservation);
            }
        }

        public void ApplyPriceChangesForBuyMiles(MOBSHOPReservation reservation = null, Reservation bookingPathReservation = null)
        {
            if (bookingPathReservation != null)
            {
                UpdateGrandTotalForBookingReservation(bookingPathReservation, reservation);
                UpdateMPFPriceTypeDescriptionForBuyMilesForBookingReservation(bookingPathReservation, reservation);
                UpdateTaxPriceTypeDescriptionForBuyMilesForBookingReservation(bookingPathReservation, reservation);
                AddFarewayDescriptionForMultipaxForBuyMilesForBookingReservation(bookingPathReservation, reservation);
                UpdateComplianceTaxesForBuyMilesForBookingReservation(bookingPathReservation, reservation);
            }
            else if (reservation != null)
            {
                UpdateGrandTotalForBookingReservation(bookingPathReservation, reservation);
                UpdateMPFPriceTypeDescriptionForBuyMiles(reservation);
                UpdateTaxPriceTypeDescriptionForBuyMilesForBookingReservation(reservation);
                AddFarewayDescriptionForMultipaxForBuyMilesForBookingReservation(reservation);
                UpdateComplianceTaxesForBuyMilesForBookingReservation(bookingPathReservation, reservation);
            }
        }
        private void UpdateTaxPriceTypeDescriptionForBuyMilesForBookingReservation(MOBSHOPReservation reservation = null)
        {
            if (reservation?.Prices != null)
            {
                var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "TAX");
                if (mpfIndex >= 0)
                    reservation.Prices[mpfIndex].PriceTypeDescription = reservation?.Prices?.FirstOrDefault(a => a.DisplayType == "TAX")?.PriceTypeDescription;
            }
        }

        private void AddFarewayDescriptionForMultipaxForBuyMilesForBookingReservation(Reservation bookingPathReservation, MOBSHOPReservation reservation = null)
        {
            var miles = bookingPathReservation.Prices.FirstOrDefault(a => a.DisplayType == "MILES");
            if (miles != null && reservation?.Prices?.Count > 0 && bookingPathReservation?.Prices?.Count > 0)
            {
                MOBSHOPPrice travelrPriceMPF = new MOBSHOPPrice();
                travelrPriceMPF.DisplayType = "TRAVELERPRICE_MPF";
                travelrPriceMPF.CurrencyCode = miles.CurrencyCode;
                travelrPriceMPF.DisplayValue = miles.DisplayValue;
                travelrPriceMPF.Value = miles.Value;
                travelrPriceMPF.PaxTypeCode = miles.PaxTypeCode;
                travelrPriceMPF.PriceTypeDescription = "Fare";
                travelrPriceMPF.FormattedDisplayValue = miles.FormattedDisplayValue;
                travelrPriceMPF.PaxTypeDescription = miles.PaxTypeDescription;
                reservation.Prices.Add(travelrPriceMPF);
            }
        }

        private void AddFarewayDescriptionForMultipaxForBuyMilesForBookingReservation(MOBSHOPReservation reservation = null)
        {
            if (reservation?.Prices != null)
            {
                var miles = reservation.Prices.FirstOrDefault(a => a.DisplayType == "MILES");
                if (miles != null && reservation?.Prices?.Count > 0 && reservation?.Prices?.Count > 0)
                {
                    MOBSHOPPrice travelrPriceMPF = new MOBSHOPPrice();
                    travelrPriceMPF.DisplayType = "TRAVELERPRICE_MPF";
                    travelrPriceMPF.CurrencyCode = miles.CurrencyCode;
                    travelrPriceMPF.DisplayValue = miles.DisplayValue;
                    travelrPriceMPF.Value = miles.Value;
                    travelrPriceMPF.PaxTypeCode = miles.PaxTypeCode;
                    travelrPriceMPF.PriceTypeDescription = "Fare";
                    travelrPriceMPF.FormattedDisplayValue = miles.FormattedDisplayValue;
                    travelrPriceMPF.PaxTypeDescription = miles.PaxTypeDescription;
                    reservation.Prices.Add(travelrPriceMPF);
                }
            }
        }


        private void UpdateTaxPriceTypeDescriptionForBuyMilesForBookingReservation(Reservation bookingPathReservation, MOBSHOPReservation reservation = null)
        {
            if (reservation?.Prices != null)
            {
                var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "TAX");
                if (mpfIndex >= 0)
                    reservation.Prices[mpfIndex].PriceTypeDescription = bookingPathReservation?.Prices?.FirstOrDefault(a => a.DisplayType == "TAX")?.PriceTypeDescription;
            }
        }

        private void UpdateMPFPriceTypeDescriptionForBuyMiles(MOBSHOPReservation reservation)
        {
            string additionalMiles = "Additional miles";
            if (reservation?.Prices != null)
            {
                var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "MPF");
                if (mpfIndex >= 0)
                {
                    if (string.IsNullOrEmpty(reservation.Prices[mpfIndex].PriceTypeDescription))
                        reservation.Prices[mpfIndex].PriceTypeDescription = additionalMiles;
                }
            }
        }

        private void UpdateComplianceTaxesForBuyMilesForBookingReservation(Reservation bookingPathReservation, MOBSHOPReservation reservation = null)
        {
            try
            {
                if (reservation?.Prices != null)
                {
                    var complainceTaxes = reservation?.ShopReservationInfo2?.InfoNationalityAndResidence?.ComplianceTaxes;
                    if (complainceTaxes != null)
                    {
                        foreach (var taxes in complainceTaxes)
                        {
                            foreach (var tax in taxes)
                            {
                                if (tax.TaxCode == "MPF")
                                {
                                    taxes.Remove(tax);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }


        private void UpdateMPFPriceTypeDescriptionForBuyMilesForBookingReservation(Reservation bookingPathReservation, MOBSHOPReservation reservation = null)
        {
            var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "MPF");
            if (mpfIndex >= 0)
            {
                reservation.Prices[mpfIndex].PriceTypeDescription = bookingPathReservation?.Prices?.FirstOrDefault(a => a.DisplayType == "MPF")?.PriceTypeDescription;
            }
        }

        private void UpdateGrandTotalForBookingReservation(Reservation bookingPathReservation = null, MOBSHOPReservation reservation = null)
        {
            if (reservation?.Prices != null)
            {
                var grandTotalIndex = reservation.Prices.FindIndex(a => a.PriceType == "GRAND TOTAL");
                if (grandTotalIndex >= 0)
                {
                    string priceTypeDescription = reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.PriceTypeDescription;
                    double extraMilePurchaseAmount = (reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value != null) ?
                                             Convert.ToDouble(reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value) : 0;

                    if (extraMilePurchaseAmount > 0 && bookingPathReservation.Prices[grandTotalIndex].Value < extraMilePurchaseAmount)
                    {
                        reservation.Prices[grandTotalIndex].Value += extraMilePurchaseAmount;
                        CultureInfo ci = null;
                        ci = TopHelper.GetCultureInfo(reservation?.Prices[grandTotalIndex].CurrencyCode);
                        reservation.Prices[grandTotalIndex].DisplayValue = reservation.Prices[grandTotalIndex].Value.ToString();
                        reservation.Prices[grandTotalIndex].FormattedDisplayValue = TopHelper.FormatAmountForDisplay(reservation?.Prices[grandTotalIndex].Value.ToString(), ci, false);
                    }
                }
            }
        }

        private void UpdateComplianceTaxesForBuyMiles(MOBSHOPReservation reservation)
        {
            try
            {
                var complainceTaxes = reservation?.ShopReservationInfo2?.InfoNationalityAndResidence?.ComplianceTaxes;
                if (complainceTaxes != null)
                {
                    foreach (var taxes in complainceTaxes)
                    {
                        foreach (var tax in taxes)
                        {
                            if (tax.TaxCode == "MPF")
                            {
                                taxes.Remove(tax);
                                return;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void UpdateComplianceTaxesForBuyMilesForBookingReservation(Reservation reservation)
        {
            try
            {
                var complainceTaxes = reservation?.ShopReservationInfo2?.InfoNationalityAndResidence?.ComplianceTaxes;
                if (complainceTaxes != null)
                {
                    foreach (var taxes in complainceTaxes)
                    {
                        foreach (var tax in taxes)
                        {
                            if (tax.TaxCode == "MPF")
                            {
                                taxes.Remove(tax);
                                return;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void UpdateTaxPriceTypeDescriptionForBuyMiles(MOBSHOPReservation reservation)
        {
            var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "TAX");
            if (mpfIndex >= 0)
                reservation.Prices[mpfIndex].PriceTypeDescription = "Taxes and fees";
        }

        private void AddFarewayDescriptionForMultipaxForBuyMiles(MOBSHOPReservation reservation)
        {
            var miles = reservation.Prices.FirstOrDefault(a => a.DisplayType == "MILES");
            if (miles != null && reservation?.Prices?.Count > 0)
            {
                MOBSHOPPrice travelrPriceMPF = new MOBSHOPPrice();
                travelrPriceMPF.DisplayType = "TRAVELERPRICE_MPF";
                travelrPriceMPF.CurrencyCode = miles.CurrencyCode;
                travelrPriceMPF.DisplayValue = miles.DisplayValue;
                travelrPriceMPF.Value = miles.Value;
                travelrPriceMPF.PaxTypeCode = miles.PaxTypeCode;
                travelrPriceMPF.PriceTypeDescription = "Fare";
                travelrPriceMPF.FormattedDisplayValue = miles.FormattedDisplayValue;
                travelrPriceMPF.PaxTypeDescription = miles.PaxTypeDescription;
                reservation.Prices.Add(travelrPriceMPF);

            }
        }

        private void AddFarewayDescriptionForMultipaxForBuyMilesForBookingReservation(Reservation reservation)
        {
            var miles = reservation.Prices.FirstOrDefault(a => a.DisplayType == "MILES");
            if (miles != null && reservation?.Prices?.Count > 0)
            {
                MOBSHOPPrice travelrPriceMPF = new MOBSHOPPrice();
                travelrPriceMPF.DisplayType = "TRAVELERPRICE_MPF";
                travelrPriceMPF.CurrencyCode = miles.CurrencyCode;
                travelrPriceMPF.DisplayValue = miles.DisplayValue;
                travelrPriceMPF.Value = miles.Value;
                travelrPriceMPF.PaxTypeCode = miles.PaxTypeCode;
                travelrPriceMPF.PriceTypeDescription = "Fare";
                travelrPriceMPF.FormattedDisplayValue = miles.FormattedDisplayValue;
                travelrPriceMPF.PaxTypeDescription = miles.PaxTypeDescription;
                reservation.Prices.Add(travelrPriceMPF);

            }
        }

        private void UpdateMPFPriceTypeDescriptionForBuyMiles(MOBSHOPReservation reservation, FlightReservationResponse flightReservationResponse)
        {
            string additionalMiles = "Additional {0} miles";
            var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "MPF");
            if (mpfIndex >= 0)
            {
                string formattedMiles = String.Format("{0:n0}", flightReservationResponse.DisplayCart?.DisplayFees?.FirstOrDefault()?.SubItems?.Where(a => a.Key == "PurchaseMiles")?.FirstOrDefault()?.Value);
                reservation.Prices[mpfIndex].PriceTypeDescription = string.Format(additionalMiles, formattedMiles);
            }
        }

        public void UpdateGrandTotal(MOBSHOPReservation reservation)
        {
            var grandTotalIndex = reservation.Prices.FindIndex(a => a.PriceType == "GRAND TOTAL");
            if (grandTotalIndex >= 0)
            {
                double extraMilePurchaseAmount = (reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value != null) ?
                                         Convert.ToDouble(reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value) : 0;
                if (extraMilePurchaseAmount > 0)
                {
                    reservation.Prices[grandTotalIndex].Value += extraMilePurchaseAmount;
                    CultureInfo ci = null;
                    ci = TopHelper.GetCultureInfo(reservation?.Prices[grandTotalIndex].CurrencyCode);
                    reservation.Prices[grandTotalIndex].DisplayValue = reservation.Prices[grandTotalIndex].Value.ToString();
                    reservation.Prices[grandTotalIndex].FormattedDisplayValue = TopHelper.FormatAmountForDisplay(reservation?.Prices[grandTotalIndex].Value.ToString(), ci, false);
                }
            }
        }

        private void UpdateTaxPriceTypeDescriptionForBuyMilesForBookingReservation(Reservation reservation)
        {
            var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "TAX");
            if (mpfIndex >= 0)
                reservation.Prices[mpfIndex].PriceTypeDescription = "Taxes and fees";
        }

        private void UpdateMPFPriceTypeDescriptionForBuyMilesForBookingReservation(Reservation reservation, FlightReservationResponse flightReservationResponse)
        {
            string additionalMiles = "Additional {0} miles";
            var mpfIndex = reservation.Prices.FindIndex(a => a.DisplayType == "MPF");
            if (mpfIndex >= 0)
            {
                string formattedMiles = String.Format("{0:n0}", flightReservationResponse.DisplayCart?.DisplayFees?.FirstOrDefault()?.SubItems?.Where(a => a.Key == "PurchaseMiles")?.FirstOrDefault()?.Value);
                reservation.Prices[mpfIndex].PriceTypeDescription = string.Format(additionalMiles, formattedMiles);
            }
        }

        private void UpdateGrandTotalForBookingReservation(Reservation bookingPathReservation)
        {
            var grandTotalIndex = bookingPathReservation.Prices.FindIndex(a => a.PriceType == "GRAND TOTAL");
            if (grandTotalIndex >= 0)
            {
                string priceTypeDescription = bookingPathReservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.PriceTypeDescription;
                double extraMilePurchaseAmount = (bookingPathReservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value != null) ?
                                         Convert.ToDouble(bookingPathReservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value) : 0;

                if (extraMilePurchaseAmount > 0 && priceTypeDescription?.Contains("Additional") == false)
                {
                    bookingPathReservation.Prices[grandTotalIndex].Value += extraMilePurchaseAmount;
                    CultureInfo ci = null;
                    ci = TopHelper.GetCultureInfo(bookingPathReservation?.Prices[grandTotalIndex].CurrencyCode);
                    bookingPathReservation.Prices[grandTotalIndex].DisplayValue = bookingPathReservation.Prices[grandTotalIndex].Value.ToString();
                    bookingPathReservation.Prices[grandTotalIndex].FormattedDisplayValue = TopHelper.FormatAmountForDisplay(bookingPathReservation?.Prices[grandTotalIndex].Value.ToString(), ci, false);
                }
            }
        }
    }
}
