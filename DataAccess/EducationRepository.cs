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
    public class EducationRepository : IEducationRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public EducationRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task AddAsync(Education education)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = @"INSERT INTO Users (
                                             UniversityID,
                                             MajorID, 
                                             DegreeID,
                                             Location,
                                             GPA,
                                             GraduationYear,
                                             CreateAd) 
                       VALUES (
                                @UniversityID,
                                @MajorID,
                                @DegreeID,
                                @Location,
                                @GPA,
                                @GraduationYear,
                                @CreateAd)";
            await conn.ExecuteAsync(sql, education);
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = @"UPDATE Users SET DeletedAt = @DeletedAt WHERE ID = @Id";
            await conn.ExecuteAsync(sql, new { Id = id, DeletedAt = DateTime.UtcNow });
        }

        public async Task<IEnumerable<Education>> GetAllAsync()
        {
            using var conn = _dbFactory.CreateConnection();
            return await conn.QueryAsync<Education>("SELECT * FROM Education");
        }

        public Task UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
