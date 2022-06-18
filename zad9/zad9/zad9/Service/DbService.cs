using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zad9.Models;

namespace zad9.Service
{
    public class DbService : IDbService
    {
        readonly MainDbContext _dbContext;

        public DbService(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  void saveUser(User user)
        {
            _dbContext.Users.Add(user);
           _dbContext.SaveChanges();
        }
    }
}
