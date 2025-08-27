using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CityService
    {
        private readonly ICityRepository _repo;

        public CityService (ICityRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<City>> GetAllAsync() => await _repo.GetAllAsync();
    }
}
