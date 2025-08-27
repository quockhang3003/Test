using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MajorService
    {
        private readonly IMajorRepository _repo;
        public MajorService(IMajorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Major>> GetAllAsync() => await _repo.GetAllAsync();
    }
}
