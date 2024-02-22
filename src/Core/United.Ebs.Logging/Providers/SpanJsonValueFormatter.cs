using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Serilog.Events;

namespace United.Ebs.Logging.Providers
{
    public class SpanJsonValueFormatter : LogEventPropertyValueVisitor<TextWriter, bool>
    {        
        private readonly string _typeTagName;
        private const string DefaultTypeTagName = "_typeTag";

        public SpanJsonValueFormatter(string typeTagName = "_typeTag")
        {
            this._typeTagName = typeTagName;
        }

        public void Format(LogEventPropertyValue value, TextWriter output, string key)
        {
            this.Visit(output, value, key);
        }

        protected override bool VisitScalarValue(TextWriter state, ScalarValue scalar, string key)
        {
            if (scalar == null)
                throw new ArgumentNullException(nameof(scalar));
            this.FormatLiteralValue(scalar.Value, state, key);
            return false;
        }

        protected override bool VisitSequenceValue(TextWriter state, SequenceValue sequence, string key)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            state.Write('[');
            string str = "";
            for (int index = 0; index < sequence.Elements.Count; ++index)
            {
                state.Write(str);
                str = ",";
                this.Visit(state, sequence.Elements[index], key);
            }
            state.Write(']');
            return false;
        }

        protected override bool VisitStructureValue(
          TextWriter state,
          StructureValue structure,
          string key)
        {
            state.Write('{');
            string str = "";
            for (int index = 0; index < structure.Properties.Count; ++index)
            {
                state.Write(str);
                str = ",";
                LogEventProperty property = structure.Properties[index];
                SpanJsonValueFormatter.WriteQuotedJsonString(property.Name, state);
                state.Write(':');
                this.Visit(state, property.Value, property.Name);
            }
            if (this._typeTagName != null && structure.TypeTag != null)
            {
                state.Write(str);
                SpanJsonValueFormatter.WriteQuotedJsonString(this._typeTagName, state);
                state.Write(':');
                SpanJsonValueFormatter.WriteQuotedJsonString(structure.TypeTag, state);
            }
            state.Write('}');
            return false;
        }

        protected override bool VisitDictionaryValue(
          TextWriter state,
          DictionaryValue dictionary,
          string key)
        {
            state.Write('{');
            string str = "";
            foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> element in (IEnumerable<KeyValuePair<ScalarValue, LogEventPropertyValue>>)dictionary.Elements)
            {
                state.Write(str);
                str = ",";
                SpanJsonValueFormatter.WriteQuotedJsonString((element.Key.Value ?? (object)"null").ToString(), state);
                state.Write(':');
                this.Visit(state, element.Value, element.Key.Value.ToString());
            }
            state.Write('}');
            return false;
        }

        public virtual void FormatLiteralValue(object value, TextWriter output, string key)
        {
            switch (value)
            {
                case null:
                    SpanJsonValueFormatter.FormatNullValue(output);
                    return;
                case string str:
                    SpanJsonValueFormatter.FormatStringValue(str, output, key);
                    return;
                case ValueType _:
                    switch (value)
                    {
                        case int _:
                        case uint _:
                        case long _:
                        case ulong _:
                        case Decimal _:
                        case byte _:
                        case sbyte _:
                        case short _:
                        case ushort _:
                            SpanJsonValueFormatter.FormatExactNumericValue((IFormattable)value, output);
                            return;
                        case double num:
                            SpanJsonValueFormatter.FormatDoubleValue(num, output, key);
                            return;
                        case float num:
                            SpanJsonValueFormatter.FormatFloatValue(num, output, key);
                            return;
                        case bool flag:
                            SpanJsonValueFormatter.FormatBooleanValue(flag, output);
                            return;
                        case char _:
                            SpanJsonValueFormatter.FormatStringValue(value.ToString(), output, key);
                            return;
                        case DateTime _:
                        case DateTimeOffset _:
                            SpanJsonValueFormatter.FormatDateTimeValue((IFormattable)value, output);
                            return;
                        case TimeSpan timeSpan:
                            SpanJsonValueFormatter.FormatTimeSpanValue(timeSpan, output);
                            return;
                    }
                    break;
            }
            SpanJsonValueFormatter.FormatLiteralObjectValue(value, output, key);
        }

        public static void FormatBooleanValue(bool value, TextWriter output)
        {
            output.Write(value ? "true" : "false");
        }

        public static void FormatFloatValue(float value, TextWriter output, string key)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                SpanJsonValueFormatter.FormatStringValue(value.ToString((IFormatProvider)CultureInfo.InvariantCulture), output, key);
            else
                output.Write(value.ToString("R", (IFormatProvider)CultureInfo.InvariantCulture));
        }

        public static void FormatDoubleValue(double value, TextWriter output, string key)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                SpanJsonValueFormatter.FormatStringValue(value.ToString((IFormatProvider)CultureInfo.InvariantCulture), output, key);
            else
                output.Write(value.ToString("R", (IFormatProvider)CultureInfo.InvariantCulture));
        }

        public static void FormatExactNumericValue(IFormattable value, TextWriter output)
        {
            output.Write(value.ToString((string)null, (IFormatProvider)CultureInfo.InvariantCulture));
        }

        public static void FormatDateTimeValue(IFormattable value, TextWriter output)
        {
            output.Write('"');
            output.Write(value.ToString("O", (IFormatProvider)CultureInfo.InvariantCulture));
            output.Write('"');
        }

        public static void FormatTimeSpanValue(TimeSpan value, TextWriter output)
        {
            output.Write('"');
            output.Write(value.ToString());
            output.Write('"');
        }

        public static void FormatLiteralObjectValue(object value, TextWriter output, string key)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            SpanJsonValueFormatter.FormatStringValue(JsonConvert.SerializeObject(value), output, key);
        }

        public static void FormatStringValue(string str, TextWriter output, string key)
        {
            SpanJsonValueFormatter.WriteQuotedJsonString(str, output, key);
        }

        public static void FormatNullValue(TextWriter output)
        {
            output.Write("null");
        }

        public static void WriteQuotedJsonString(string str, TextWriter output, string key)
        {
            SpanJsonValueFormatter.WriteQuotedJsonStringWithSpan(str, output);
        }

        public static void WriteQuotedJsonString(string str, TextWriter output)
        {
            output.Write('"');
            int startIndex = 0;
            bool flag = false;
            for (int index = 0; index < str.Length; ++index)
            {
                char ch = str[index];
                if (ch < ' ' || ch == '\\' || ch == '"')
                {
                    flag = true;
                    output.Write(str.Substring(startIndex, index - startIndex));
                    startIndex = index + 1;
                    switch (ch)
                    {
                        case '\t':
                            output.Write("\\t");
                            continue;
                        case '\n':
                            output.Write("\\n");
                            continue;
                        case '\f':
                            output.Write("\\f");
                            continue;
                        case '\r':
                            output.Write("\\r");
                            continue;
                        case '"':
                            output.Write("\\\"");
                            continue;
                        case '\\':
                            output.Write("\\\\");
                            continue;
                        default:
                            output.Write("\\u");
                            output.Write(((int)ch).ToString("X4"));
                            continue;
                    }
                }
            }
            if (flag)
            {
                if (startIndex != str.Length)
                    output.Write(str.Substring(startIndex));
            }
            else
                output.Write(str);
            output.Write('"');
        }

        public static void WriteQuotedJsonStringWithSpan(ReadOnlySpan<char> chars, TextWriter output)
        {
            output.Write('"');
            int startIndex = 0;
            bool flag = false;
            for (int index = 0; index < chars.Length; ++index)
            {
                char ch = chars[index];
                if (ch < ' ' || ch == '\\' || ch == '"')
                {
                    flag = true;
                    output.Write(chars.Slice(startIndex, index - startIndex));
                    startIndex = index + 1;
                    switch (ch)
                    {
                        case '\t':
                            output.Write("\\t");
                            continue;
                        case '\n':
                            output.Write("\\n");
                            continue;
                        case '\f':
                            output.Write("\\f");
                            continue;
                        case '\r':
                            output.Write("\\r");
                            continue;
                        case '"':
                            output.Write("\\\"");
                            continue;
                        case '\\':
                            output.Write("\\\\");
                            continue;
                        default:
                            output.Write("\\u");
                            output.Write(((int)ch).ToString("X4"));
                            continue;
                    }
                }
            }

            if (flag)
            {
                if (startIndex != chars.Length)
                    output.Write(chars.Slice(startIndex));
            }
            else
                output.Write(chars);

            output.Write('"');
        }

    }
}
