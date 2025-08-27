using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Preferable office location")]
        public int PreferableOfficeLocationID { get; set; }

        [Required]
        [Display(Name = "1st preference")]
        public int FirstPreferenceID { get; set; }

        [Display(Name = "2nd preference")]
        public int? SecondPreferenceID { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Full name in Vietnamese")]
        public string VietnameseName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Contact")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "ID Card No / Passport No")]
        public string PasswordHash { get; set; }

        [Display(Name = "Date of Issue")]
        [DataType(DataType.Date)]
        public DateTime? DateOfIssue { get; set; }

        [Display(Name = "Place of Issue")]
        public string PlaceOfIssue { get; set; }

        [Required]
        [Display(Name = "Mobile Contact")]
        public string Mobile { get; set; }

       
        [Display(Name = "Number & Street")]
        public string Street { get; set; }

        public string Ward { get; set; }

        [Display(Name = "City/Province")]
        public int CityID { get; set; }


        [Display(Name = "Address")]
        public string CurrentAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


    }
}
