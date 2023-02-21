using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThePower.Data.Repository.IRepository;
using ThePower.Models;
using ThePowerData.Data;

namespace ThePower.Data.Repository
{
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        private ApplicationDbContext _db;

        public MembershipRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Membership membership)
        {
            _db.Update(membership);
        }
    }
}
