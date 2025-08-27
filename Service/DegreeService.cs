using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DegreeService
    {
        private readonly IDegreeRepository _repo;
        public DegreeService(IDegreeRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Degree>> GetAllAsync() => await _repo.GetAllAsync();
    }
}
