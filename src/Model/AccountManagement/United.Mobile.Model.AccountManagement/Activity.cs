// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Activity.cs" company="United Continental Holdings, Inc.">
//   Copyright (c) 2011 United Continental Holdings, Inc. All rights reserved.
// </copyright>
// <summary>
//   $Id: Activity.cs 2262 2019-07-22 22:03:24Z bob.wang $
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace United.Mobile.Model.Common
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Class defining the repeat columns in QualDetail db table
    /// </summary>
    [DataContract(Namespace = "")]
    public class Activity
    {
        [DataMember()]
        public virtual int ActivityYear { get; set; }

        [DataMember()]
        public virtual long AnnualLifetimeMiles { get; set; }

        [DataMember()]
        public virtual long EliteQualifyingMiles { get; set; }

        [DataMember()]
        public virtual long EliteQualifyingMilesOnUnited { get; set; }

        [DataMember()]
        public virtual double EliteQualifyingPoints { get; set; }

        [DataMember()]
        public virtual double EliteQualifyingPointsOnUnited { get; set; }

        [DataMember()]
        public virtual long FlexEliteQualifyingMiles { get; set; }

        [DataMember()]
        public virtual double HighYieldSegments { get; set; }

        [DataMember()]
        public virtual double MinimumSegmentsRequired { get; set; }

        [DataMember()]
        public virtual string MinRequiredSegmentsIndicator { get; set; }

        [DataMember()]
        public virtual double TotalRevenue { get; set; }

        [DataMember()]
        public virtual double UnpublishedTotalRevenue { get; set; }

        [DataMember()]
        public virtual double AdjustmentRevenue { get; set; }

        [DataMember()]
        public virtual double TotalFlightRevenue { get; set; }

        [DataMember()]
        public virtual double TotalCopaRevenue { get; set; }

        [DataMember()]
        public virtual double TotalJVRevenue { get; set; }

        [DataMember()]
        public virtual double TotalAncillaryRevenue { get; set; }

        [DataMember()]
        public virtual double ChaseCardSpendRevenue { get; set; }

        [DataMember()]
        public virtual string ChaseCardSpendIndicator { get; set; }

        [DataMember()]
        public virtual string PresidentPlusIndicator { get; set; }

        [DataMember()]
        public virtual string DomesticIndicator { get; set; }

        [DataMember()]
        public virtual string RpcWaivedIndicator { get; set; }

        [DataMember()]
        public virtual string AddressIndicator { get; set; }

        [DataMember()]
        public virtual string RpcCode { get; set; }

        [DataMember()]
        public virtual long PremierQualifyingPoints { get; set; }

        [DataMember()]
        public virtual double PremierQualifyingFlightSegments { get; set; }

        [DataMember()]
        public virtual long FlexPremierQualifyingPoints { get; set; }
    }
}