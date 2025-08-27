using Dapper;
using System.Data;
using Domain.Interfaces;
using Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using BCrypt.Net;

namespace DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
     
        public UserRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var conn = _dbFactory.CreateConnection();
            return await conn.QueryAsync<User>("SELECT * FROM Users");
        }

        public async Task AddAsync(User user)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = @"INSERT INTO Users (
                                             PreferableOfficeLocationID,
                                             FirstPreferenceID,
                                             SecondPreferenceID,
                                             LastName,
                                             FirstName,
                                             VietnameseName,
                                             Gender,
                                             Nationality,
                                             DateOfBirth,
                                             PlaceOfBirth,
                                             Email,
                                             PasswordHash,
                                             DateOfIssue,
                                             PlaceOfIssue,
                                             Mobile,
                                             Street,
                                             Ward,
                                             CityID,
                                             CurrentAddress,
                                             CreatedAt) 
                       VALUES (
                                @PreferableOfficeLocationID,
                                @FirstPreferenceID,
                                @SecondPreferenceID,
                                @LastName,
                                @FirstName,
                                @VietnameseName,
                                @Gender,
                                @Nationality,
                                @DateOfBirth,
                                @PlaceOfBirth,
                                @Email,
                                @PasswordHash,
                                @DateOfIssue,
                                @PlaceOfIssue,
                                @Mobile,
                                @Street,
                                @Ward,
                                @CityID,
                                @CurrentAddress,
                                @CreatedAt)";
            await conn.ExecuteAsync(sql, user);
        }

        public async Task<bool> ExistsEmailAsync(string email)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            return await conn.ExecuteScalarAsync<int>(sql, new { Email = email }) > 0;
        }
        public async Task<bool> ExistsIDCardAsync(string IDCard)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "SELECT PasswordHash FROM Users";
            var hashes = await conn.QueryAsync<string>(sql);
            foreach (var hash in hashes)
            {
                if (BCrypt.Net.BCrypt.Verify(IDCard, hash))
                {
                    return true;
                }
            }

            return false;
        }
        public async Task UpdateAsync(int id)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = @"UPDATE Users SET
                     PreferableOfficeLocation = @PreferableOfficeLocation,
                     FirstPreference = @FirstPreference,
                     SecondPreference = @SecondPreference,
                     LastName = @LastName,
                     FirstName = @FirstName,
                     FullNameInVietnamese = @FullNameInVietnamese,
                     Gender = @Gender,
                     Nationality = @Nationality,
                     DateOfBirth = @DateOfBirth,
                     PlaceOfBirth = @PlaceOfBirth,
                     Email = @Email,
                     IdNumber = @IdNumber,
                     DateOfIssue = @DateOfIssue,
                     PlaceOfIssue = @PlaceOfIssue,
                     Mobile = @Mobile,
                     PermanentStreet = @PermanentStreet,
                     Ward = @Ward,
                     City = @City,
                     CurrentAddress = @CurrentAddress,
                     UpdatedAt = @UpdatedAt
                    WHERE ID = @Id";
            await conn.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.UtcNow});
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = @"UPDATE Users SET DeletedAt = @DeletedAt WHERE ID = @Id";
            await conn.ExecuteAsync(sql, new { Id = id, DeletedAt = DateTime.UtcNow });
        }
    }
}
