using BCrypt.Net;
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
    public class UserService
    {
        private readonly IUserRepository _repo;
        
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<User?> GetUserByEmail(string email) => await _repo.GetByEmailAsync(email);
        public async Task<IEnumerable<User>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task RegisterUser(RegisterDTO dto)
        {
            if (await ExistsEmailAsync(dto.Email))
                throw new Exception("Email already exists.");

            var user = new User
            {
                PreferableOfficeLocationID = dto.PreferableOfficeLocation.Value,
                FirstPreferenceID = dto.FirstPreference.Value,
                SecondPreferenceID = dto.SecondPreference,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                VietnameseName = dto.VietnameseName,
                Gender = dto.Gender,
                Nationality = dto.Nationality,
                DateOfBirth = dto.DateOfBirth,
                PlaceOfBirth = dto.PlaceOfBirth,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.IdCardNumber),
                DateOfIssue = dto.DateOfIssue,
                PlaceOfIssue = dto.PlaceOfIssue,
                Mobile = dto.Mobile,
                Street = dto.Street,
                Ward = dto.Ward,
                CityID = dto.City.Value,
                CurrentAddress = dto.CurrentAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(user);
        }
        public async Task UpdateUser(User user)
        {
            
        }
        public async Task<bool> ExistsEmailAsync(string email) => await _repo.ExistsEmailAsync(email);
        public async Task<bool> ExistsIDCardAsync(string IDCard) => await _repo.ExistsIDCardAsync(IDCard);
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;
        }
    }
}
