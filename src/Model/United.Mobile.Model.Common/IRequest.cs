namespace United.Mobile.Model
{
    public interface IRequest<T>
    {
        T Data { get; set; }
    }
}