namespace United.Ebs.Logging.Providers
{
    public class LoggingConfiguration : ILoggingConfiguration
    {
        public string DateFormat { get; set; }
        public int InnerExceptionLength { get; set; }
        public int StackTraceLength { get; set; }
        public bool RequestResponseEnabled { get; set; }
    }
}
