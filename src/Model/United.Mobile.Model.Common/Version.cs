using System;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model
{
    [Serializable]
    public class Version
    {
        [Required, Range(1, int.MaxValue)]
        public int Major { get; set; }
        [Required]
        public int? Minor { get; set; }
        [Required]
        public int Build { get; set; }
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}";
        }
    }
}