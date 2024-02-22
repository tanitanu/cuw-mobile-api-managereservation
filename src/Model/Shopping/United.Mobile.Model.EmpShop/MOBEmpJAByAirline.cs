using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpJAByAirline
    {
        
        private string airlineCode;        
        private string airlineDescription;        
        private string scheduleEngineCode;        
        private string boardDate;        
        private string suspendStartDate;        
        private string suspendEndDate;        
        private string businessPassClass;        
        private string personalPassClass;        
        private string familyPassClass;        
        private string vacationPassClass;        
        private string familyVacationPassClass;        
        private string buddyPassClass;        
        private string jumpSeatPassClass;
        private string deviationPassClass;        
        private string trainingPassClass;
        private string extendedFamilyPassClass;               
        //private CPAPassClasses cpc;        
        private string eTicketIndicator;
        private string paymentIndicator;
        private bool canBookFirstOnBusiness = false;
        private bool canBookSpouseOnBusiness = false;        
        //private PaymentDetails pd; 
        private bool feeWaivedFirst = false;
        private bool feeWaivedCoach = false;
        private string seniority;
        private string seniorityDate;
        private bool display;        
        
        public string AirlineCode
        {
            get { return this.airlineCode; }
            set { this.airlineCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string AirlineDescription
        {
            get { return this.airlineDescription; }
            set { this.airlineDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ScheduleEngineCode
        {
            get { return this.scheduleEngineCode; }
            set { this.scheduleEngineCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string BoardDate
        {
            get { return this.boardDate; }
            set { this.boardDate = value; }
        }

        public string SuspendStartDate
        {
            get { return this.suspendStartDate; }
            set { this.suspendStartDate = value; }
        }

        public string SuspendEndDate
        {
            get { return this.suspendEndDate; }
            set { this.suspendEndDate = value; }
        }

        public string BusinessPassClass
        {
            get { return this.businessPassClass; }
            set { this.businessPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string PersonalPassClass
        {
            get { return this.personalPassClass; }
            set { this.personalPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FamilyPassClass
        {
            get { return this.familyPassClass; }
            set { this.familyPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string VacationPassClass
        {
            get { return this.vacationPassClass; }
            set { this.vacationPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FamilyVacationPassClass
        {
            get { return this.familyVacationPassClass; }
            set { this.familyVacationPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string BuddyPassClass
        {
            get { return this.buddyPassClass; }
            set { this.buddyPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string JumpSeatPassClass
        {
            get { return this.jumpSeatPassClass; }
            set { this.jumpSeatPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DeviationPassClass
        {
            get { return this.deviationPassClass; }
            set { this.deviationPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string TrainingPassClass
        {
            get { return this.trainingPassClass; }
            set { this.trainingPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ExtendedFamilyPassClass
        {
            get { return this.extendedFamilyPassClass; }
            set { this.extendedFamilyPassClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        //public CPAPassClasses CPAPassClasses
        //{
        //    get { return this.cpc; }
        //    set { this.cpc = value; }
        //}

        public string ETicketIndicator
        {
            get { return this.eTicketIndicator; }
            set { this.eTicketIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string PaymentIndicator
        {
            get { return this.paymentIndicator; }
            set { this.paymentIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool CanBookFirstOnBusiness
        {
            get { return this.canBookFirstOnBusiness; }
            set { this.canBookFirstOnBusiness = value; }
        }

        public bool CanBookSpouseOnBusiness
        {
            get { return this.canBookSpouseOnBusiness; }
            set { this.canBookSpouseOnBusiness = value; }
        }

        //public PaymentDetails PaymentDetails
        //{
        //    get { return this.pd; }
        //    set { this.pd = value; }
        //}

        public bool FeeWaivedFirst
        {
            get { return this.feeWaivedFirst; }
            set { this.feeWaivedFirst = value; }
        }

        public bool FeeWaivedCoach
        {
            get { return this.feeWaivedCoach; }
            set { this.feeWaivedCoach = value; }
        }

        public string Seniority
        {
            get { return this.seniority; }
            set { this.seniority = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SeniorityDate
        {
            get { return this.seniorityDate; }
            set { this.seniorityDate = value; }
        }

        public bool Display
        {
            get { return this.display; }
            set { this.display = value; }
        }
    }
}
