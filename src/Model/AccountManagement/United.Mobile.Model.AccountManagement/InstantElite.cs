// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstantElite.cs" company="United Continental Holdings, Inc.">
//   Copyright (c) 2013 United Continental Holdings, Inc. All rights reserved.
// </copyright>
// <summary>
//   $Id: InstantElite.cs 129 2012-10-18 20:15:50Z bob.wang $
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace United.Mobile.Model.Common
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The instant elite.
    /// </summary>
    [Serializable()]
    public partial class InstantElite
    {
        /// <summary>
        /// Gets or sets the instant elite year.
        /// </summary>
        [DataMember()]
        public virtual int EliteYear { get; set; }

        /// <summary>
        /// Gets or sets the instant elite level.
        /// </summary>
        [DataMember()]
        public virtual int EliteLevel { get; set; }

        /// <summary>
        /// Gets or sets the promotion code.
        /// </summary>
        [DataMember()]
        public virtual string PromotionCode { get; set; }

        /// <summary>
        /// Gets or sets the consolidated code.
        /// </summary>
        public virtual string ConsolidatedCode { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        [DataMember()]
        public virtual DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        [DataMember()]
        public virtual DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Implements IEquatable interface Equals
        /// </summary>
        /// <param name="obj">
        /// The object to compare to.
        /// </param>
        /// <returns>
        /// Returns if the objects are equal.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as InstantElite;

            if (null == other)
            {
                return false;
            }

            return this.EliteYear.Equals(other.EliteYear) &&
                   this.EliteLevel.Equals(other.EliteLevel) &&
                   (this.PromotionCode ?? string.Empty).Equals((other.PromotionCode ?? string.Empty)) &&
                   this.EffectiveDate.Equals(other.EffectiveDate) &&
                   this.ExpirationDate.Equals(other.ExpirationDate);
        }

        /// <summary>
        /// Get hash code, must be defined for composite keys with NHibernate
        /// </summary>
        /// <returns>
        /// Hash code for the class
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = this.EliteYear.GetHashCode() ^
                    this.EliteYear.GetHashCode() ^
                    (this.PromotionCode ?? string.Empty).GetHashCode() ^
                    this.EffectiveDate.GetHashCode() ^
                    this.ExpirationDate.GetHashCode();

                return hash;
            }
        }
    }
}
