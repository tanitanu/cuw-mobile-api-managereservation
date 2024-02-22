using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBStyledText
    {

        public string Text { get; set; } = string.Empty;
        public string TextColor { get; set; } = MOBStyledColor.Black.GetDescription();

        public bool IsItalic { get; set; }
        public string BackgroundColor { get; set; } = MOBStyledColor.Clear.GetDescription();

        public string SortPriority { get; set; } = string.Empty;

    }


    public enum MOBStyledColor
    {
        [Description("#FF 000000")]
        Black,
        [Description("#FF FFC558")]
        Yellow,
        [Description("#FF 1D7642")]
        Green,
        [Description("#00 000000")]
        Clear,
        [Description("#FF FFFFFF")]
        White,
    }

    public enum MOBFlightBadgeSortOrder
    {
        CovidTestRequired
    }

    public enum MOBFlightProductBadgeSortOrder
    {
        Specialoffer,
        MixedCabin,
        YADiscounted,
        CorporateDiscounted,
        MyUADiscounted,
        BreakFromBusiness,
        SaverAward
    }

    public enum MOBFlightProductAwardType
    {
        Saver,
        Standard
    }
    public static class LinqHelper
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
    }
}