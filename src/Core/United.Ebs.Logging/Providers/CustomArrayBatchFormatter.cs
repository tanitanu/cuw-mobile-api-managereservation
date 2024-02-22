using Serilog.Sinks.Http;
using Serilog.Sinks.Http.BatchFormatters;

namespace United.Ebs.Logging.Providers
{
    /// <summary>
    /// Formatter serializing batches of log events into a JSON array.
    /// <para/>
    /// Example:
    /// [
    ///   { event n },
    ///   { event n+1 }
    /// ]
    /// </summary>
    public class CustomArrayBatchFormatter : ArrayBatchFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBatchFormatter"/> class.
        /// </summary>
        /// <param name="eventBodyLimitBytes">
        /// The maximum size, in bytes, that the JSON representation of an event may take before it
        /// is dropped rather than being sent to the server. Specify null for no limit. Default
        /// value is 16 MB.
        /// </param>
        public CustomArrayBatchFormatter(long? eventBodyLimitBytes = 16 * ByteSize.MB)
            : base(eventBodyLimitBytes)
        {
        }
    }
}