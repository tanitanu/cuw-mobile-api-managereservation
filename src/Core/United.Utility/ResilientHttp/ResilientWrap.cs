using Polly;
using Polly.Timeout;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using United.Utility.Config;

namespace United.Utility.Common
{
    public class ResilientWrap
    {
        private readonly IAsyncPolicy _policyWrap;
        private static IDictionary<string, ResilientWrap> instances;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id to identify unique instance</param>
        /// <param name="retryExceptionTypes">The list of exception types which will trigger retry</param>
        /// <param name="timeoutPolicyConfig"></param>
        /// <param name="retryPolicyConfig"></param>
        /// <param name="cbPolicyConfig"></param>
        /// <returns></returns>
        public static ResilientWrap GetInstance(string id, ICollection<Type> retryExceptionTypes = null, TimeoutPolicyConfig timeoutPolicyConfig = null, RetryPolicyConfig retryPolicyConfig = null, CircuitBreakerPolicyConfig cbPolicyConfig = null)
        {
            if (instances == null)
            {
                instances = new ConcurrentDictionary<string, ResilientWrap>();
            }
            if (!instances.TryGetValue(id, out ResilientWrap instance))
            {
                instance = new ResilientWrap(retryExceptionTypes, timeoutPolicyConfig, retryPolicyConfig, cbPolicyConfig);
            }
            return instance;
        }

        private ResilientWrap(ICollection<Type> retryExceptionTypes = null, TimeoutPolicyConfig timeoutPolicyConfig = null, RetryPolicyConfig retryPolicyConfig = null, CircuitBreakerPolicyConfig cbPolicyConfig = null)
        {
            if (retryExceptionTypes == null || retryExceptionTypes.Count == 0)
            {
                retryExceptionTypes = new List<Type> { };
            }
            foreach (Type t in retryExceptionTypes)
            {
                if (!t.IsSubclassOf(typeof(Exception)) && !t.Equals(typeof(Exception)))
                {
                    throw new ArgumentException("The type must be an Exception type", "retryExceptionTypes");
                }
            }
            timeoutPolicyConfig = timeoutPolicyConfig ?? new TimeoutPolicyConfig();
            retryPolicyConfig = retryPolicyConfig ?? new RetryPolicyConfig();
            cbPolicyConfig = cbPolicyConfig ?? new CircuitBreakerPolicyConfig();
            _policyWrap = Policy.WrapAsync(
                Policy.Handle<Exception>()
                    .CircuitBreakerAsync(cbPolicyConfig.AllowExceptions, TimeSpan.FromSeconds(cbPolicyConfig.BreakDuration)),
                Policy
                    .Handle<IOException>()
                    .Or<TimeoutRejectedException>()
                    .Or<Exception>((ex) => retryExceptionTypes.Any((t) => t.IsInstanceOfType(ex)))
                    .WaitAndRetryAsync(retryCount: retryPolicyConfig.RetryCount,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))),
                Policy.TimeoutAsync(timeoutPolicyConfig.Seconds));
        }

        public async Task DoAsync(Func<CancellationToken, Task> action)
        {
            await _policyWrap.ExecuteAsync(action, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<T> DoAsync<T>(Func<CancellationToken, Task<T>> action)
        {
            return await _policyWrap.ExecuteAsync(action, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
