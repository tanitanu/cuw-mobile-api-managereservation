namespace United.Mobile.Model.Internal.Common
{
    public class Status
    {
        public string DefaultIndicator { get; set; }
        public string Description { get; set; }
        public int DisplaySequence { get; set; }
        public string Key { get; set; }
        public virtual string Code { get; set; }
        public virtual Genre Type { get; set; }
    }
}
