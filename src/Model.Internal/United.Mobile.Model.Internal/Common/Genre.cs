namespace United.Mobile.Model.Internal.Common
{
    public class Genre
    {        
        public string Key { get; set; }      
        public string Description { get; set; }      
        public string DefaultIndicator { get; set; }      
        public int DisplaySequence { get; set; }        
        public virtual string Value { get; set; }
    }
}
