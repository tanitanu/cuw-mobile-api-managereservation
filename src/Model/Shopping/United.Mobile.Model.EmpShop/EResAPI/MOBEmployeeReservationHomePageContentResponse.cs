using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class MOBEmployeeReservationHomePageContentResponse
    {
        public bool IsAllowedToSelectFlight { get; set; }
        public Content Content { get; set; }
        public List< NewsDetail> NewsDetails { get; set; }
        public List<EmpTravelType> TravelType { get; set; }
        public string EmployeeMsgType { get; set; }
        public List<BaseAlert> BaseAlert { get; set; }
        public List<Menu> Menus { get; set; }
        public object Error { get; set; }
        public string LastCallDateTime { get; set; }
        public string ServerName { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public string TransferMessage { get; set; }
    }

    public class Content
    {
        public string Message { get; set; }
    }

    public class NewsDetail
    {
        public string GetsMSG { get; set; }
        public string NewsID { get; set; }
        public string HeadingRed { get; set; }
        public string HeadingBlack { get; set; }
        public object SpaceCount { get; set; }
        public DateTime PostDate { get; set; }
        public string PostBody { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public string EmployeeId { get; set; }
        public string[] EmployeeStatus { get; set; }
        public string[] WorkGroup { get; set; }
    }

    public class EmpTravelType
    {
        public string TravelCode { get; set; }
        public string TravelDescription { get; set; }
        public EmergencyInvolf[] EmergencyInvolves { get; set; }
        public NatureOfEmergency[] NatureOfEmergency { get; set; }
    }

    public class EmergencyInvolf
    {
        public string PassengerCode { get; set; }
        public string PassengerDescription { get; set; }
    }

    public class NatureOfEmergency
    {
        public string EmergencyCode { get; set; }
        public string EmergencyDescription { get; set; }
    }

    
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public MenuCategory[] MenuCategories { get; set; }
    }

    public class MenuCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentMenuId { get; set; }
        public Submenu[] SubMenu { get; set; }
    }

    public class Submenu
    {
        public int SubMenuId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public bool IsExternalLink { get; set; }
        public int MenuCategoryId { get; set; }
        public bool IsExternalSite { get; set; }
    }





}
