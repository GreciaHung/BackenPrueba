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
    public class PolicyRepository : GenericRepository<APIContext,Policy>
    {
      public async Task<Policy> GetOneAsync(int policyId)
        {
            var result = await GetAll().FirstOrDefaultAsync(x => x.Id == policyId);
            return result;
        }

        public bool PolicyExists(int id)
        {
            return GetAll().Count(e => e.Id == id) > 0;
        }
    }
}