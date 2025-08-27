using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LocationOfficeService
    {
        private readonly ILocationOfficeRepository _repo;
        public LocationOfficeService(ILocationOfficeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LocationOffice>> GetAll() => await _repo.GetAllAsync();
    }
}
