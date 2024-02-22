using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace United.Ebs.Logging.Providers
{
    public abstract class LogEventPropertyValueVisitor<TState, TResult>
    {
        protected virtual TResult Visit(TState state, LogEventPropertyValue value, string key)
        {
            switch (value)
            {
                case null:
                    throw new ArgumentNullException(nameof(value));
                case ScalarValue scalar:
                    return this.VisitScalarValue(state, scalar, key);
                case SequenceValue sequence:
                    return this.VisitSequenceValue(state, sequence, key);
                case StructureValue structure:
                    return this.VisitStructureValue(state, structure, key);
                case DictionaryValue dictionary:
                    return this.VisitDictionaryValue(state, dictionary, key);
                default:
                    return this.VisitUnsupportedValue(state, value, key);
            }
        }

        protected abstract TResult VisitScalarValue(TState state, ScalarValue scalar, string key);

        protected abstract TResult VisitSequenceValue(TState state, SequenceValue sequence, string key);

        protected abstract TResult VisitStructureValue(
          TState state,
          StructureValue structure,
          string key);

        protected abstract TResult VisitDictionaryValue(
          TState state,
          DictionaryValue dictionary,
          string key);

        protected virtual TResult VisitUnsupportedValue(
          TState state,
          LogEventPropertyValue value,
          string key)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            throw new NotSupportedException(string.Format("The value {0} is not of a type supported by this visitor.", (object)value));
        }
    }

}
