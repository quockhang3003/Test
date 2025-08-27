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
    public class PreferenceRepository : IPreferenceRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public PreferenceRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<IEnumerable<Preference>> GetAllAsync()
        {
            using var conn = _dbFactory.CreateConnection();
            return await conn.QueryAsync<Preference>("SELECT * FROM Preference");
        }
    }
}
