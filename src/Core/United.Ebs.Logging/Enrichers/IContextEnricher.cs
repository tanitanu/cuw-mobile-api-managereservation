namespace United.Ebs.Logging.Enrichers
{
    // Added by Ashrith
    public interface IContextEnricher
    {
        void Add(string key, dynamic value);
    }
}
