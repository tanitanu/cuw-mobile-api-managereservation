using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface ICCEAncillaryMyTripsFeedbackService
    {
        Task<string> SendCCEAncillaryMyTripsFeedback(string token, string request);
    }
}
