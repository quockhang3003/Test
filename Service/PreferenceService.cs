using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PreferenceService 
    {
        private readonly IPreferenceRepository _repo;

        public PreferenceService (IPreferenceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Preference>> GetAll() => await _repo.GetAllAsync();
    }
}
