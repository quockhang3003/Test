using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class LocationOfficeRepository : ILocationOfficeRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public LocationOfficeRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<LocationOffice>> GetAllAsync()
        {
            using var conn = _dbFactory.CreateConnection();
            return await conn.QueryAsync<LocationOffice>("SELECT * FROM LocationOffice");
        }
    }
}
