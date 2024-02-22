namespace United.Mobile.Model.SeatMap
{
    public class UIVersion
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public string DisplayText { get; set; }
        public override string ToString() => $"{Major}.{Minor}.{Build}";
    }
}
