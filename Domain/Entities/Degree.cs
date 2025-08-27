using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Degree
    {
        public int Id { get; set; } 
        [Required][Display(Name = "Degree")] public string DegreeName { get; set; }  
    }
}
