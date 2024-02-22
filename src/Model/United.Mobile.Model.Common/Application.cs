using System;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model
{
    [System.Serializable]
    public class Application
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public Version Version { get; set; }
    }
}