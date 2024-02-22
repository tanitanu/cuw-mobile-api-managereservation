namespace United.Ebs.Logging.Providers
{
    public interface ILoggingConfiguration
    {
        string DateFormat { get; set; }
        int InnerExceptionLength { get; set; }
        int StackTraceLength { get; set; }
        bool RequestResponseEnabled { get; set; }
    }
}
