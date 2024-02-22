// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceResponse.cs" company="United Continental Holdings, Inc.">
//   Copyright (c) 2013 United Continental Holdings, Inc. All rights reserved.
// </copyright>
// <summary>
//   $Id: ServiceResponse.cs 129 2012-10-18 20:15:50Z bob.wang $
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace United.Mobile.Model.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The response type.
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Gets or sets the request GUID.
        /// </summary>
        public virtual string RequestGuid { get; set; }

        /// <summary>
        /// Gets or sets the run type.
        /// </summary>
        public virtual string RunType { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer Id.
        /// </summary>
        public virtual long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the victim indicator.
        /// </summary>
        public virtual string VictimIndicator { get; set; }

        /// <summary>
        /// Gets or sets the account reference.
        /// </summary>
        public virtual string AccountReference { get; set; }

        /// <summary>
        /// Gets or sets the enrollment date.
        /// </summary>
        public virtual DateTime EnrollmentDate { get; set; }

        /// <summary>
        /// Gets or sets the under age indicator.
        /// </summary>
        public virtual string UnderAgeIndicator { get; set; }

        /// <summary>
        /// Gets or sets the run date.
        /// </summary>
        public virtual string RunDate { get; set; }

        /// <summary>
        /// Gets or sets the refresh date.
        /// </summary>
        public virtual DateTime RefreshDate { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Gets or sets the elite year.
        /// </summary>
        public virtual int EliteYear { get; set; }

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        public virtual string CityName { get; set; }

        /// <summary>
        /// Gets or sets the state code.
        /// </summary>
        public virtual string StateCode { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        public virtual string PostCode { get; set; }

        /// <summary>
        /// Gets or sets the current premier level.
        /// </summary>
        public virtual int CurrentPremierLevel { get; set; }

        /// <summary>
        /// Gets or sets the million miler level.
        /// </summary>
        public virtual int MillionMilerLevel { get; set; }

        /// <summary>
        /// Gets or sets the million miler level code.
        /// </summary>
        public virtual string MillionMilerLevelCode { get; set; }

        /// <summary>
        /// Gets or sets the infinite level.
        /// </summary>
        public virtual int InfiniteLevel { get; set; }

        /// <summary>
        /// Gets or sets the infinite level.
        /// </summary>
        public virtual int InfiniteEliteYear { get; set; }

        /// <summary>
        /// Gets or sets the current year instant elite.
        /// </summary>
        public virtual InstantElite CurrentYearInstantElite { get; set; }

        /// <summary>
        /// Gets or sets the future year instant elite.
        /// </summary>
        public virtual InstantElite FutureYearInstantElite { get; set; }

        /// <summary>
        /// Gets or sets the active indicator.
        /// </summary>
        public virtual string ActiveIndicator { get; set; }

        /// <summary>
        /// Gets or sets the CUB indicator.
        /// </summary>
        public virtual string CubIndicator { get; set; }

        /// <summary>
        /// Gets or sets the SUB indicator.
        /// </summary>
        public virtual string SubIndicator { get; set; }

        /// <summary>
        /// Gets or sets the closed fraud indicator.
        /// </summary>
        public virtual string ClosedFraudIndicator { get; set; }

        /// <summary>
        /// Gets or sets the close pending indicator.
        /// </summary>
        public virtual string ClosePendingIndicator { get; set; }

        /// <summary>
        /// Gets or sets the deceased indicator.
        /// </summary>
        public virtual string DeceasedIndicator { get; set; }

        /// <summary>
        /// Gets or sets the mail status indicator.
        /// </summary>
        public virtual string MailStatusIndicator { get; set; }

        /// <summary>
        /// Gets or sets the status indicator.
        /// </summary>
        public virtual ServiceStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the process duration.
        /// </summary>
        public virtual string ProcessIndicator { get; set; }

        /// <summary>
        /// Gets or sets the test account indicator.
        /// </summary>
        public virtual string TestAccountIndicator { get; set; }

        /// <summary>
        /// Gets or sets the trial elite indicator.
        /// </summary>
        public virtual string TrialEliteIndicator { get; set; }

        /// <summary>
        /// Gets or sets the charity account indicator.
        /// </summary>
        public virtual string CharityAccountIndicator { get; set; }

        /// <summary>
        /// Gets or sets the lifetime miles.
        /// </summary>
        public virtual long LifetimeMiles { get; set; }

        /// <summary>
        /// Gets or sets the current balance.
        /// </summary>
        public virtual long CurrentBalance { get; set; }

        /// <summary>
        /// Gets or sets the PQD met.
        /// </summary>
        public virtual int PqdMet { get; set; }

        /// <summary>
        /// Gets or sets the PQD other met.
        /// </summary>
        public virtual int PqdOtherMet { get; set; }

        /// <summary>
        /// Gets or sets the current year activity.
        /// </summary>
        public virtual Activity CurrentYearActivity { get; set; }

        /// <summary>
        /// Gets or sets the previous year activity.
        /// </summary>
        public virtual Activity PreviousYearActivity { get; set; }

        /// <summary>
        /// Gets or sets the two-year prior activity.
        /// </summary>
        public virtual Activity TwoYearPriorActivity { get; set; }

        /// <summary>
        /// Gets or sets the CEO indicator.
        /// </summary>
        public virtual string CeoIndicator { get; set; }

        /// <summary>
        /// Gets or sets the RPC RpcHighest indicator.
        /// </summary>
        public virtual string RpcHighest { get; set; }

        /// <summary>
        /// Gets or sets the mileage plus card indicator.
        /// </summary>
        public virtual string MileagePlusCardIndicator { get; set; }

        /// <summary>
        /// Gets or sets the United club card indicator.
        /// </summary>
        public virtual string UnitedClubIndicator { get; set; }

        /// <summary>
        /// Gets or sets the million miler sponsor account.
        /// </summary>
        public virtual string MillionMilerSponsorAccount { get; set; }

        /// <summary>
        /// Gets or sets the million miler sponsor level.
        /// </summary>
        public virtual int MillionMilerSponsorLevel { get; set; }

        /// <summary>
        /// Gets or sets current-year companion account.
        /// </summary>
        public virtual string CurrentYearCompanionAccount { get; set; }

        /// <summary>
        /// Gets or sets future-year companion account.
        /// </summary>
        public virtual string FutureYearCompanionAccount { get; set; }


        /// <summary>
        /// Gets or sets the companion account effective date.
        /// </summary>
        public virtual DateTime CompanionAccountEffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the companion account expiration date.
        /// </summary>
        public virtual DateTime CompanionAccountExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the latest kit history.
        /// </summary>
       
        public virtual DateTime CurrentPremierLevelExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets Marriott indicator.
        /// </summary>
        public virtual string MarriottIndicator { get; set; }

        /// <summary>
        /// Gets or sets Hertz indicator.
        /// </summary>
        public virtual string HertzIndicator { get; set; }

        /// <summary>
        /// Gets or sets MarriottCode For United.
        /// </summary>
        public virtual string MarriottCodeForUnited { get; set; }

        /// <summary>
        /// Gets or sets UnitedCode For Marriott.
        /// </summary>
        public virtual string UnitedCodeForMarriott { get; set; }

        /// <summary>
        /// Gets or sets TSA pre-check indicator.
        /// </summary>
        public virtual string TsaPreCheckIndicator { get; set; }

        /// <summary>
        /// Gets or sets disqualified indicator.
        /// </summary>
        public virtual string DisqualifiedIndicator { get; set; }

        /// <summary>
        /// Gets or sets GS recon status.
        /// </summary>
        public virtual string YearEndIndicator { get; set; }
        /// <summary>
        /// Gets or sets the tracking level.
        /// </summary>
        public virtual int TrackingLevel { get; set; }


    }
}
