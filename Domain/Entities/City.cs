using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "City")]
        public string CityName { get; set; } = string.Empty;
    }
}
