namespace United.Ebs.Logging.Common
{
    public enum  EventType
    {

        Default=1,
        Error=5,
        ApplicationStart = 6,
        NotificationSend = 7,
        AuthenticationSuccess = 8,
        AuthorizationSuccess = 9,
        AuthorizationFailure = 10,
        AuthenticationFailure = 11,
        CacheHit = 12,
        Unknown = 13,
        ConfigurationSetting = 14,
        ApplicationEnd = 15,
        Logout = 16,
        RestResponse = 17,
        RestRequest = 18,
        ServiceMetaData = 19,
        RestRequestResponse = 20,
        UnHandledException = 21,
        
    }
}
