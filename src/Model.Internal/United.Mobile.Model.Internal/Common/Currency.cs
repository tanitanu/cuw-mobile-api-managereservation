namespace United.Mobile.Model.Internal.Common
{
    public class Currency
    {
        public string Code { get; set; }        
        public string Name { get; set; }       
        public Genre Type { get; set; }        
        public virtual Status Status { get; set; }       
        public virtual int DecimalPlace { get; set; }       
        public virtual string ID { get; set; }       
        public virtual int ISONumericCode { get; set; }
    }
}
