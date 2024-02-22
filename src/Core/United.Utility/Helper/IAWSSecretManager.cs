using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Utility.Helper
{
    public interface IAWSSecretManager
    {
        Task<string> GetSecretValue(string secretKey);
    }
}
