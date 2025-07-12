using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;
using EClaim.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace EClaim.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, IConfiguration config)
        {
            _dbContext = context;
            _config = config;
        }

        public async Task<User> GetUser(int id)
        {

            var users = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == id);
            return users;
        }

        public async Task<IEnumerable<User>> GetAllUser(UserSearchDto userSearchDto)
        {

            var users = _dbContext.Users.AsQueryable();

            if (userSearchDto.Id > 0)
                users = users.Where(c => c.Id == userSearchDto.Id);

            if (!string.IsNullOrEmpty(userSearchDto.FullName))
                users = users.Where(c => c.FullName.Contains(userSearchDto.FullName));

            if (!string.IsNullOrEmpty(userSearchDto.Email))
                users = users.Where(c => c.FullName.Equals(userSearchDto.Email));

            if (!string.IsNullOrEmpty(userSearchDto.Phone))
                users = users.Where(c => c.FullName.Equals(userSearchDto.Phone));

            if (userSearchDto.Role != null && Enum.IsDefined(typeof(Role), userSearchDto.Role))
                users = users.Where(c => c.Role == userSearchDto.Role);

            if (userSearchDto.IsEmailVerified != null)
                users = users.Where(c => c.IsEmailVerified == userSearchDto.IsEmailVerified);

            //if (userSearchDto.UserId > 0)
            //{
            //    var user = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == userSearchDto.UserId);

            //    if (user != null && user.Role == Role.Claimant)
            //    {
            //        users = users.Where(s => s.Id == userSearchDto.UserId);
            //    }
            //    else if (user != null && user.Role == Role.Adjuster)
            //    { 

            //    }
            //}

            return users;
        }

        public async Task<User> EditAsync(UserSearchDto userSearchDto)
        {
            var user = await _dbContext.Users
                        .FirstOrDefaultAsync(s => s.Id == userSearchDto.Id);
            if (user == null)
            {
                throw new ApplicationException("User not exists");
            }

            user.FullName = userSearchDto.FullName;
            user.Email = userSearchDto.Email;
            user.Phone = userSearchDto.Phone;
            user.Address = userSearchDto.Address;
            user.Role = (Role)userSearchDto.Role;

            if (userSearchDto.IsEmailVerified == true)
            {
                user.IsEmailVerified = true;
            }
            else
            {
                user.IsEmailVerified = false;
            }

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
