using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UnfinishedBooking
{
    public interface IOmniChannelCartService
    {
        Task<string> PurgeUnfinshedBookings(string token, string action, string sessionId);

    }
}
