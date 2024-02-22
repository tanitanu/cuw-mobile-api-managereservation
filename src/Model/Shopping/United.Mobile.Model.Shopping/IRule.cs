

using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    public interface IRule<T>
    {
        /// <summary>
        /// Check to see if this rule is satisfied.
        /// This serves as the condition to execute this rule.
        /// </summary>
        /// <returns>True if rule is satisfied, otherwise false</returns>
        bool ShouldExecuteRule();

        /// <summary>
        /// Execute this rule.
        /// </summary>
        Task<T> Execute ();
    }
}
