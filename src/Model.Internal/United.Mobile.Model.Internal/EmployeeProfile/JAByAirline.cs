namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class JAByAirline
    {
        public string AirlineCode { get; set; }
        public string AirlineDescription { get; set; }
        public string ScheduleEngineCode { get; set; }
        public string BoardDate { get; set; }
        public string SuspendStartDate { get; set; }
        public string SuspendEndDate { get; set; }
        public string BusinessPassClass { get; set; }
        public string PersonalPassClass { get; set; }
        public string FamilyPassClass { get; set; }
        public string VacationPassClass { get; set; }
        public string FamilyVacationPassClass { get; set; }
        public string BuddyPassClass { get; set; }
        public string JumpSeatPassClass { get; set; }
        public string DeviationPassClass { get; set; }
        public string TrainingPassClass { get; set; }
        public string ExtendedFamilyPassClass { get; set; }
        public string ServiceYears { get; set; }
        public string ServiceDays { get; set; }
        public string ServiceMonths { get; set; }
        public CPAPassclasses CPAPassClasses { get; set; }
        public string ETicketIndicator { get; set; }
        public string PaymentIndicator { get; set; }
        public bool CanBookFirstOnBusiness { get; set; }
        public bool CanBookSpouseOnBusiness { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public bool IsFeeWaivedFirst { get; set; }
        public bool IsFeeWaivedCoach { get; set; }
        public string Seniority { get; set; }
        public string SeniorityDate { get; set; }
        public int TaxIndicator { get; set; }
        public bool BusinessBoardingPriorityNA { get; set; }
    }
}
