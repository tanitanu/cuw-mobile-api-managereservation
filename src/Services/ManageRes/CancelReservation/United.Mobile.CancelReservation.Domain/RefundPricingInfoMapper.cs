using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using United.Common.Helper;
using United.Mobile.Model.ManageRes;

namespace United.Mobile.CancelReservation.Domain
{
    public class RefundPricingInfoMapper
    {
        const string CurrencyCode = "USD";
        string _languageCode = string.Empty;

        // Response from Refund Service
        private readonly MOBQuoteRefundResponse _refundResponse;
        private readonly IConfiguration _configuration;

        public RefundPricingInfoMapper(MOBQuoteRefundResponse refundResponse, IConfiguration configuration, string languageCode="en-US")
        {
            _refundResponse = refundResponse;
            _languageCode = languageCode;
            _configuration = configuration;
        }

        public MOBModifyFlowPricingInfo Map()
        {
            var pricingInfo = new MOBModifyFlowPricingInfo { QuoteType = _refundResponse.QuoteType };

            var totalTravelerCount = 0;

            decimal taxesAndFeesTotal = new decimal(0.0);

            if ((_refundResponse != null) && (_refundResponse.PriceBreakDown != null))
            {
                totalTravelerCount = _refundResponse.PriceBreakDown.Sum(mobPriceBreakdown => mobPriceBreakdown.TravelerCount);

                taxesAndFeesTotal = _refundResponse.PriceBreakDown.Sum(mobPriceBreakDown =>
                {
                    return mobPriceBreakDown.Taxes == null ? new decimal(0d) : mobPriceBreakDown.Taxes.Select(tax => tax == null ? new decimal(0d) : decimal.Parse(tax.Amount)).Sum();
                });
            }

            decimal totalPaid = (decimal.Parse(_refundResponse.RefundAmount.Amount));
            double.TryParse(totalPaid.ToString(), out double amountInDouble);
            pricingInfo.TotalPaid = ConfigUtility.GetCurrencyAmount(amountInDouble, _refundResponse.RefundAmount.CurrencyCode, 2, _languageCode);

            if (_configuration.GetValue<bool>("CancelRefund_FixFor_IncorrectBasePriceMultiPax"))
            {
                double totalTaxes = 0;
                foreach (var price in _refundResponse.PriceBreakDown)
                {
                    if (price.Taxes != null)
                    {
                        double total = price.Taxes.Select(tax => tax.Amount == null ? 0 : double.Parse(tax.Amount)).Sum();
                        totalTaxes = totalTaxes + (total * price.TravelerCount);
                    }
                }
                double.TryParse(totalTaxes.ToString(), out double taxesAndFeesTotalInDouble);
                pricingInfo.TaxesAndFeesTotal = ConfigUtility.GetCurrencyAmount(taxesAndFeesTotalInDouble, _refundResponse.RefundAmount.CurrencyCode, 2, _languageCode);
            }
            else
            {
                double.TryParse((taxesAndFeesTotal * totalTravelerCount).ToString(), out double taxesAndFeesTotalInDouble);
                pricingInfo.TaxesAndFeesTotal = ConfigUtility.GetCurrencyAmount(taxesAndFeesTotalInDouble, _refundResponse.RefundAmount.CurrencyCode, 2, _languageCode); ;
            }

            pricingInfo.PricesPerTypes = MapPricesPerType(totalTravelerCount);
            pricingInfo.CurrencyCode = _refundResponse.RefundAmount.CurrencyCode;

            return pricingInfo;
        }

        private List<MOBModifyFlowPricePerType> MapPricesPerType(int totalTravelerCount)
        {
            var pricesPerType = new List<MOBModifyFlowPricePerType>();

            if (_refundResponse.PriceBreakDown != null)
            {
                _refundResponse.PriceBreakDown.ForEach(price =>
                {

                    var groupedTravelerTypes = price.PassengerTypeCode.GroupBy(g => g).Select(s => new
                    {
                        Pax = s.Key,
                        Count = s.Count()
                    }).ToList();

                    if (_configuration.GetValue<bool>("EnableNonUSDCurrencyChangesInCancelRefund"))
                    {
                        foreach (var paxCode in groupedTravelerTypes)
                        {

                            var pCodeType = paxCode.Pax;
                            var TotalBaseFare = price.BasePrice == null ?
                                            "$0.00" :
                                            ConfigUtility.GetCurrencyAmount(double.Parse(price.BasePrice.Amount), price.BasePrice.CurrencyCode,2, _languageCode);

                            var pricePerType = new MOBModifyFlowPricePerType
                            {
                                Type = pCodeType,
                                TotalBaseFare = TotalBaseFare,
                                NumberOfTravelers = paxCode.Count,
                                TaxAndFeeBreakdown = price.Taxes == null ? new List<MOBModifyFlowPrice>() : price.Taxes.Select(tax => new MOBModifyFlowPrice
                                {
                                    Description = tax.Description,
                                    FormattedValue = ConfigUtility.GetCurrencyAmount(double.Parse(tax.Amount), price.BasePrice.CurrencyCode, 2, _languageCode)
                                }).ToList(),
                                TotalTaxesAndFeesPerPassenger = price.Taxes == null ? "$0.00" :
                                     ConfigUtility.GetCurrencyAmount(price.Taxes.Sum(tax => double.Parse(tax.Amount)), price.Taxes.First().CurrencyCode)
                                        
                            };
                            pricesPerType.Add(pricePerType);
                        }

                    }
                    else
                    {
                        foreach (var paxCode in groupedTravelerTypes)
                        {
                            var pricePerType = new MOBModifyFlowPricePerType
                            {
                                Type = paxCode.Pax,
                                TotalBaseFare = price.BasePrice == null ? "$0.00" :
                                    (decimal.Parse(price.BasePrice.Amount) * paxCode.Count).ToString("c",
                                        GetCurrencyFormatProviderSymbolDecimals(CurrencyCode)),
                                NumberOfTravelers = paxCode.Count,
                                TaxAndFeeBreakdown = price.Taxes == null ? new List<MOBModifyFlowPrice>() : price.Taxes.Select(tax => new MOBModifyFlowPrice
                                {
                                    Description = tax.Description,
                                    FormattedValue =
                                        decimal.Parse(tax.Amount)
                                            .ToString("c", GetCurrencyFormatProviderSymbolDecimals(CurrencyCode))
                                }).ToList(),
                                TotalTaxesAndFeesPerPassenger = price.Taxes == null ? "$0.00" :
                                    price.Taxes.Sum(tax => decimal.Parse(tax.Amount))
                                        .ToString("c", GetCurrencyFormatProviderSymbolDecimals(CurrencyCode))
                            };
                            pricesPerType.Add(pricePerType);
                        }
                    }
                });
            }

            return pricesPerType;
        }

        private static NumberFormatInfo GetCurrencyFormatProviderSymbolDecimals(string currencyCode)
        {
            var currencyNumberFormat = (from culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                        let region = new RegionInfo(culture.LCID)
                                        where String.Equals(region.ISOCurrencySymbol, currencyCode,
                                                            StringComparison.InvariantCultureIgnoreCase)
                                        select culture.NumberFormat).First();

            return currencyNumberFormat;
        }
    }
}
