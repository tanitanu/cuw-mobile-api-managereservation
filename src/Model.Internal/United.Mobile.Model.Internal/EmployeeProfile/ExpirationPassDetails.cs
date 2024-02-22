using System;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class ExpirationPassDetails
    {
        public int ExpirationCount { get; set; } = 0;
        public DateTime ExpirationYear { get; set; } = DateTime.MinValue;
        public string ExpirationText { get; set; } = string.Empty;
    }
}
