using System;
using United.Mobile.Model.Common;

namespace United.Common.Helper
{
    public interface ICslContext : IDisposable
    {
        Session Session { get; }
        string TransactionId { get; }
        string Token { get; }
    }
}