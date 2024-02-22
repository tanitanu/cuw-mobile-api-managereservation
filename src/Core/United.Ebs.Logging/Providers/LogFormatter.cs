using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using System;
using System.IO;

namespace United.Ebs.Logging.Providers
{
    public class LogFormatter : ITextFormatter
    {
        readonly JsonValueFormatter valueFormatter = null;
        public LogFormatter(JsonValueFormatter valueFormatter = null)
        {
            this.valueFormatter = new JsonValueFormatter("United");
        }
        public void Format(LogEvent logEvent, TextWriter output)
        {
            try
            {
                if (logEvent.Level == LogEventLevel.Information && logEvent.Properties["RequestPath"].ToString().ToLower().Contains("/api/healthcheck"))
                {
                    return;
                }
            }
            catch { }
            output.Write('{');

            output.Write("\"LogLevel\":\"");
            output.Write(logEvent.Level);
            output.Write('\"');

            output.Write(",\"Message\":");

            try
            {
                if (logEvent.MessageTemplate.Text.Contains(Common.Constants.AttribsTemplate))
                {
                    var message = logEvent.MessageTemplate.Text.Replace(Common.Constants.AttribsTemplate, string.Empty).Trim();
                    if (logEvent.Exception != null)
                    {
                        message += Common.Constants.ExceptionTitle + logEvent.Exception.Message;
                    }

                    SpanJsonValueFormatter.WriteQuotedJsonString(message, output);                   
                }
                else
                {
                    // Changed by Ashrith
                    // var message = logEvent.MessageTemplate.Render(logEvent.Properties);
                    var message = logEvent.MessageTemplate.Text;

                    if (logEvent.Exception != null)
                    {
                        message += Common.Constants.ExceptionTitle + logEvent.Exception.Message;
                    }

                    SpanJsonValueFormatter.WriteQuotedJsonString(message, output);
                }

                foreach (var property in logEvent.Properties)
                {
                    var name = property.Key;

                    if (IsTemplateToken(logEvent, name))
                        continue;

                    if (name.Length > 0 && name[0] == '@')
                    {
                        // Escape first '@' by doubling
                        name = '@' + name;
                    }

                    output.Write(',');
                    SpanJsonValueFormatter.WriteQuotedJsonString(name, output);
                    //just use span of T
                    output.Write(':');
                    valueFormatter.Format(property.Value, output);
                }
            }
            catch (Exception ex)
            {
                var message = Common.Constants.ExceptionTitle + ex.ToString();
                SpanJsonValueFormatter.WriteQuotedJsonString(message, output);
            }

            output.Write("}");
            output.Write(System.Environment.NewLine);
            // Ashrith - Added New Line for clear readability
            //output.Write(System.Environment.NewLine);
        }

        private bool IsTemplateToken(LogEvent logEvent, string name)
        {
            try
            {
                foreach (var token in logEvent.MessageTemplate.Tokens)
                {
                    string value = token.ToString();
                    value = value.TrimStart('{').TrimEnd('}');
                    if (value == name)
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
