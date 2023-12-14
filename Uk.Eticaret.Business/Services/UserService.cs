using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Business.Services.Abstract;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)

        {
            _context = context;
        }

        public void CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUserAsync(int id)
        {
            var userToDelete = _context.Users.Find(id);

            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
            }
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            var user = await _context.Users
                .Include(e => e.Addresses)
                .ToListAsync();
            return user;
        }

        public void UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}