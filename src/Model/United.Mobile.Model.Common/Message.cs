namespace United.Mobile.Model.Common
{
    public class Message
    {
        public string MessageTitle { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public MessageType MessageType { get; set; } = MessageType.None;
      
    }
}