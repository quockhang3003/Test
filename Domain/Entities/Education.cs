using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Education
    {
        public int Id { get; set; }
        
        // Thêm UserID để liên kết với User
        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }
        
        [Required(ErrorMessage = "Please select a university")]
        public int UniversityID { get; set; }
        
        [Required(ErrorMessage = "Please select a major")]
        public int MajorID { get; set; }

        [Required(ErrorMessage = "Please select a degree")]
        public int DegreeID { get; set; }

        [Required(ErrorMessage = "Please enter location")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter GPA")]
        [Range(0.01, 4.0, ErrorMessage = "GPA must be between 0.01 and 4.0")]
        public decimal GPA { get; set; }

        [Required(ErrorMessage = "Please enter maximum GPA")]
        [Range(1, 10, ErrorMessage = "Out of must be between 1 and 10")]
        public decimal OutOf { get; set; }

        [Required(ErrorMessage = "Please select graduation month")]
        public string GraduationMonth { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select graduation year")]
        [Range(1900, 2030, ErrorMessage = "Please select a valid graduation year")]
        public int GraduationYear { get; set; }

        public string FormattedGraduationDate
        {
            get
            {
                var monthNumber = GraduationMonth switch
                {
                    "January" => 1,
                    "February" => 2,
                    "March" => 3,
                    "April" => 4,
                    "May" => 5,
                    "June" => 6,
                    "July" => 7,
                    "August" => 8,
                    "September" => 9,
                    "October" => 10,
                    "November" => 11,
                    "December" => 12,
                    _ => 1
                };
                return $"{monthNumber:D2}/{GraduationYear}";
            }
        }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
