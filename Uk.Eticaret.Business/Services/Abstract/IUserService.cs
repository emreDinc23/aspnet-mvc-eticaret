using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Services.Abstract
{
    public interface IUserService
    {
        Task<List<User>> GetAllUserAsync();

        void CreateUserAsync(User user);

        void UpdateUserAsync(User user);

        void DeleteUserAsync(int id);
    }
}