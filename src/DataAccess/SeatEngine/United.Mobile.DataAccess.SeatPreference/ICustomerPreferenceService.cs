namespace United.Mobile.DataAccess.SeatPreference
{
    public interface ICustomerPreferenceService
    {
        Task<string> GetAsync(string token, string recordLocator, string transactionId);
        Task<string> SaveAsync(string token, string recordLocator, string transactionId);
    }
}
