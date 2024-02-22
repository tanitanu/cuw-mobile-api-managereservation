using System;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Common.OnPremise
{
    [Serializable]
    public class Application
    {
        [Required]
        [Range(1, 100)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsProduction { get; set; }
        [Required]
        public Version Version { get; set; }
    }
}
