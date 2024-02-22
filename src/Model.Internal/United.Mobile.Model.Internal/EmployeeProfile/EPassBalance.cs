namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class EPassBalance
    {
        public string ExpiringYear { get; set; }
        public string PassType { get; set; } = string.Empty;
        public string PassTypeDescription { get; set; } = string.Empty;
        public int PassCount { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
    }
}
