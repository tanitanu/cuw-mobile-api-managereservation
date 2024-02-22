using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable]
    public class MOBStyledText
    {
        private string text;
        private string textColor = MOBStyledColor.Black.GetDescription();
        private bool isItalic;
        private string backgroundColor = MOBStyledColor.Clear.GetDescription();
        private string sortPriority;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public string TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }
        public bool IsItalic
        {
            get { return isItalic; }
            set { isItalic = value; }
        }

        public string BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public string SortPriority
        {
            get { return sortPriority; }
            set { sortPriority = value; }
        }
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
        Clear
    }

    public enum MOBFlightBadgeSortOrder
    {
        CovidTestRequired
    }

    public enum MOBFlightProductBadgeSortOrder
    {
        MixedCabin,
        YADiscounted,
        CorporateDiscounted,
        MyUADiscounted,
        BreakFromBusiness
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
