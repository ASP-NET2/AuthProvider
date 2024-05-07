using AuthApi.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace AuthApi.Services
{
    public class UserService(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task <IdentityUser> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null) {
              return user;
            }
            return null!;
      
        }

        public async Task <IdentityUser> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync (x => x.Id == id);
            if (user != null)
            {
                return user;
            }
            return null!;
        }

        public async Task <List<IdentityUser>> GetIdentityUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateUser(IdentityUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(IdentityUser user)
        {
            var existingUser = await GetUserByEmail (user.Id);
            if(existingUser != null)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("User not found ");
            }
           
        }

        public async Task DeleteUser(string Username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

    }
}
