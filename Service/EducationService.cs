using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EducationService
    {
        private readonly IEducationRepository _repo;
        public EducationService(IEducationRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Education>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task AddEducation(EducationDTO _dto)
        { 
            var _edu = new Education
            {
                UniversityID = _dto.UniversityID.Value,
                MajorID = _dto.MajorID.Value,
                DegreeID = _dto.DegreeID.Value,
                Location = _dto.Location,
                GPA = _dto.GPA,
                GraduationMonth = _dto.GraduationMonth,
                GraduationYear = _dto.GraduationYear,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(_edu);
        }
    }
}
