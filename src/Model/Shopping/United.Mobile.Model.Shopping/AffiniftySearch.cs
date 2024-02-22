using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  United.Mobile.Model.Shopping

{
    [Serializable]
    public class AffinitySearch
    {
        public SHOPResultsSolution[] Solution { get; set; }
        
        public int Count { get; set; }

        public string Id { get; set; } = string.Empty;
       
        public string Version { get; set; } = string.Empty;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type { get; set; } = string.Empty;
        
    }

    public class SHOPResultsSolution
    {
        public decimal Price { get; set; } 
        
       public string Currency { get; set; } = string.Empty;
       
        public string From { get; set; } = string.Empty;
       
        public string To { get; set; } = string.Empty;
        
        public System.DateTime Departs { get; set; }
        
        public System.DateTime Returns { get; set; } 
       
    }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class results
        {
            
            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("solution")]
            public resultsSolution[] solution { get; set; } 
            
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int count { get; set; } 
          
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string id { get; set; } = string.Empty;
           
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string version { get; set; } = string.Empty;
           

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type { get; set; } = string.Empty;
            
            //method added to force sample data due to dev unavailability
            public static results getSampleResults()
            {
                results ret = new results();
                resultsSolution[] resultsSoln = new resultsSolution[20];
                ret.count = resultsSoln.Count();
                ret.type = "prices";
                ret.id = new Guid().ToString();
                ret.version = "ITA.IS.API/vSample";

                for (int i = 0; i < ret.count; i++)
                {
                    resultsSoln[i] = new resultsSolution();
                    resultsSoln[i].currency = "USD";
                    resultsSoln[i].departs = DateTime.Now.AddDays(1);
                    resultsSoln[i].returns = DateTime.Now.AddDays(1+i);
                    resultsSoln[i].from = "IAH";
                    resultsSoln[i].to = "LAS";
                    resultsSoln[i].price = 999.99M;
                }
                ret.solution = resultsSoln;

                return ret;
            }

        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class resultsSolution
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal price { get; set; } 
           

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string currency { get; set; } = string.Empty;
          

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string from { get; set; } = string.Empty;
           
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string to { get; set; } = string.Empty;
            

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
            public System.DateTime departs { get; set; }
            

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
            public System.DateTime returns { get; set; }
            
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class error
        {
            
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type { get; set; } = string.Empty;
           
            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string reason { get; set; } 

    }
    
}
