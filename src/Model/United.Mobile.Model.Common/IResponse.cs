namespace United.Mobile.Model
{
    public interface IResponse<T>
    {
        string DateTimeUtc { get; set; }
        T Data { get; set; }
        string MachineName { get; set; }
        long Duration { get; set; }
    }
}