namespace United.Mobile.Model.Common
{
    public class MOBEmployeeProfileTravelTypeResponse
    {
        private MOBEmployeeProfileResponse employeeJAResponse;
        private MOBEmpTravelTypeResponse travelTypeResponse;

        public MOBEmployeeProfileResponse EmployeeJAResponse
        {
            get
            {
                return this.employeeJAResponse;
            }
            set
            {
                employeeJAResponse = value;
            }
        }
        public MOBEmpTravelTypeResponse TravelTypeResponse
        {
            get
            {
                return this.travelTypeResponse;
            }
            set
            {
                travelTypeResponse = value;
            }
        }
    }
}
