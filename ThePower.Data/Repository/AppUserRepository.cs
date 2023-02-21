using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePower.Models;
using ThePowerData.Data;

namespace ThePower.Data.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private ApplicationDbContext _db;

        public AppUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
