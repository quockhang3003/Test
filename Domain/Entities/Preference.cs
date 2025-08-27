using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Preference
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Preference")]
        public string PreferenceName {  get; set; } = string.Empty;
    }
}
