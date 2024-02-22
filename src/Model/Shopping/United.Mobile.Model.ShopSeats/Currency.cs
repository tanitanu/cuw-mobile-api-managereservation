namespace United.Mobile.Model.ShopSeats
{
    public class Currency
    {
        public virtual string Code { get; set; }
        public virtual CountryZipCode Country { get; set; }
        public virtual int DecimalPlace { get; set; }
        public virtual string Name { get; set; }
        public virtual Status Status { get; set; }
        public virtual Genre Type { get; set; }
    }
}
