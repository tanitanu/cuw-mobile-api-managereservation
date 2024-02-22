using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class EliteStatus
    {
       
        public string Code
        {
            get
            {
                return this.Code;
            }
            set
            {
                this.Code = value;

                switch (this.Code)
                {
                    case "0":
                        if (IsPSS())
                        {
                            this.Description = "Member";
                            this.STAREliteDescription = "No Status";
                            this.ESDA = "Member";
                            this.Level = 0;
                        }
                        else
                        {
                            this.Description = "NonElite";
                            this.STAREliteDescription = "No Status";
                            this.ESDA = "Member";
                            this.Level = 0;
                        }
                        break;
                    case "1":
                        if (IsPSS())
                        {
                            this.Description = "Premier Silver";
                            this.STAREliteDescription = "Silver";
                            this.ESDA = "Silver";
                            this.Level = 1;
                        }
                        else
                        {
                            this.Description = "Silver";
                            this.STAREliteDescription = "Silver";
                            this.ESDA = "Silver";
                            this.Level = 1;
                        }
                        break;
                    case "2":
                        if (IsPSS())
                        {
                            this.Description = "Premier Gold";
                            this.STAREliteDescription = "Gold";
                            this.ESDA = "Gold";
                            this.Level = 2;
                        }
                        else
                        {
                            this.Description = "Gold";
                            this.STAREliteDescription = "Gold";
                            this.ESDA = "Gold";
                            this.Level = 2;
                        }
                        break;
                    case "3":
                        if (IsPSS())
                        {
                            this.Description = "Premier Platinum";
                            this.STAREliteDescription = "Gold";
                            this.ESDA = "Platinum";
                            this.Level = 3;
                        }
                        else
                        {
                            this.Description = "Platinum";
                            this.STAREliteDescription = "Gold";
                            this.ESDA = "Platinum";
                            this.Level = 3;
                        }
                        break;
                    case "4":
                        if (IsPSS())
                        {
                            this.Description = "Premier 1K";
                            this.STAREliteDescription = "Gold";
                            this.ESDA = "1K";
                            this.Level = 4;
                        }
                        else
                        {
                            this.Description = "Presidential Platinum";
                            this.STAREliteDescription = "Gold";
                            this.ESDA = "Presidential Platinum";
                            this.Level = 4;
                        }
                        break;
                    case "5":
                        this.Description = "Global Services";
                        this.STAREliteDescription = "Gold";
                        this.ESDA = "GS";
                        this.Level = 5;
                        break;
                    case "GN":
                        this.Description = "Member";
                        this.STAREliteDescription = "No Status";
                        this.ESDA = "Member";
                        this.Level = 0;
                        break;
                    case "2P":
                        this.Description = "Premier";
                        this.STAREliteDescription = "Silver";
                        this.ESDA = "Silver";
                        this.Level = 1;
                        break;
                    case "1P":
                        this.Description = "Premier Executive";
                        this.STAREliteDescription = "Gold";
                        this.ESDA = "Premier Executive";
                        this.Level = 2;
                        break;
                    case "1K":
                        this.Description = "1K";
                        this.STAREliteDescription = "Gold";
                        this.ESDA = "1K";
                        this.Level = 3;
                        break;
                    case "GP":
                        this.Description = "Global Services";
                        this.STAREliteDescription = "Gold";
                        this.ESDA = "Global Services";
                        this.Level = 4;
                        break;
                    case "GK":
                        this.Description = "Global Services";
                        this.STAREliteDescription = "Gold";
                        this.ESDA = "Global Services";
                        this.Level = 4;
                        break;
                    default:
                        this.Description = "Unknown";
                        this.STAREliteDescription = "No Status";
                        this.ESDA = "Unknown";
                        this.Level = 0;
                        break;
                }
            }
        }

        public string Description { get; set; } = string.Empty;
       

        public string STAREliteDescription { get; set; } = string.Empty;
       

        public int Level { get; set; } 

        public string ESDA { get; set; } = string.Empty;
       

        private bool IsPSS()
        {
            bool isPSS = false;

            //try
            //{
            //    isPSS = Convert.ToBoolean(ConfigurationManager.AppSettings["PSS"]);
            //}
            //catch (System.Exception) { }

            return isPSS;
        }
    }
}
