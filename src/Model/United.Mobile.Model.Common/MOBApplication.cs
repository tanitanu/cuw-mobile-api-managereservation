using System;

namespace United.Mobile.Model
{
    [Serializable]
    public class MOBApplication
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public MOBVersion Version { get; set; }

        public bool IsProduction { get; set; }
    }
}