using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBEliteStatus
    {
        private string code = string.Empty;
        private string description = string.Empty;
        private string starEliteDescription = string.Empty;
        private string esda = string.Empty;
        private int level = 0;

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;

                switch (this.code)
                {
                    case "0":
                        if (IsPSS())
                        {
                            this.description = "Member";
                            this.starEliteDescription = "No Status";
                            this.esda = "Member";
                            this.level = 0;
                        }
                        else
                        {
                            this.description = "NonElite";
                            this.starEliteDescription = "No Status";
                            this.esda = "Member";
                            this.level = 0;
                        }
                        break;
                    case "1":
                        if (IsPSS())
                        {
                            this.description = "Premier Silver";
                            this.starEliteDescription = "Silver";
                            this.esda = "Silver";
                            this.level = 1;
                        }
                        else
                        {
                            this.description = "Silver";
                            this.starEliteDescription = "Silver";
                            this.esda = "Silver";
                            this.level = 1;
                        }
                        break;
                    case "2":
                        if (IsPSS())
                        {
                            this.description = "Premier Gold";
                            this.starEliteDescription = "Gold";
                            this.esda = "Gold";
                            this.level = 2;
                        }
                        else
                        {
                            this.description = "Gold";
                            this.starEliteDescription = "Gold";
                            this.esda = "Gold";
                            this.level = 2;
                        }
                        break;
                    case "3":
                        if (IsPSS())
                        {
                            this.description = "Premier Platinum";
                            this.starEliteDescription = "Gold";
                            this.esda = "Platinum";
                            this.level = 3;
                        }
                        else
                        {
                            this.description = "Platinum";
                            this.starEliteDescription = "Gold";
                            this.esda = "Platinum";
                            this.level = 3;
                        }
                        break;
                    case "4":
                        if (IsPSS())
                        {
                            this.description = "Premier 1K";
                            this.starEliteDescription = "Gold";
                            this.esda = "1K";
                            this.level = 4;
                        }
                        else
                        {
                            this.description = "Presidential Platinum";
                            this.starEliteDescription = "Gold";
                            this.esda = "Presidential Platinum";
                            this.level = 4;
                        }
                        break;
                    case "5":
                        this.description = "Global Services";
                        this.starEliteDescription = "Gold";
                        this.esda = "GS";
                        this.level = 5;
                        break;
                    case "GN":
                        this.description = "Member";
                        this.starEliteDescription = "No Status";
                        this.esda = "Member";
                        this.level = 0;
                        break;
                    case "2P":
                        this.description = "Premier";
                        this.starEliteDescription = "Silver";
                        this.esda = "Silver";
                        this.level = 1;
                        break;
                    case "1P":
                        this.description = "Premier Executive";
                        this.starEliteDescription = "Gold";
                        this.esda = "Premier Executive";
                        this.level = 2;
                        break;
                    case "1K":
                        this.description = "1K";
                        this.starEliteDescription = "Gold";
                        this.esda = "1K";
                        this.level = 3;
                        break;
                    case "GP":
                        this.description = "Global Services";
                        this.starEliteDescription = "Gold";
                        this.esda = "Global Services";
                        this.level = 4;
                        break;
                    case "GK":
                        this.description = "Global Services";
                        this.starEliteDescription = "Gold";
                        this.esda = "Global Services";
                        this.level = 4;
                        break;
                    default:
                        this.description = "Unknown";
                        this.starEliteDescription = "No Status";
                        this.esda = "Unknown";
                        this.level = 0;
                        break;
                }
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string STAREliteDescription
        {
            get
            {
                return this.starEliteDescription;
            }
            set
            {
                this.starEliteDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public string ESDA
        {
            get
            {
                return this.esda;
            }
            set
            {
                this.esda = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private bool IsPSS()
        {
            bool isPSS = false;

            try
            {
                //TODO isPSS = Convert.ToBoolean(ConfigurationManager.AppSettings["PSS"]);
            }
            catch (System.Exception) { }

            return isPSS;
        }
    }
}
