using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LocationOffice
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Location")]
        public string LocationName {  get; set; } = string.Empty;
    }
}
