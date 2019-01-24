using API.Core;
using API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Repositories
{
    public class UserRepository : GenericRepository<APIContext,User>
    {
        public async Task<User> GetOne(int userId)
        {
            var result = await GetAll().FirstOrDefaultAsync(x => x.Id == userId);
            return result;
        }

        public bool UserExists(int id)
        {
            return GetAll().Count(e => e.Id == id) > 0;
        }
    }
}