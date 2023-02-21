using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePower.Data.Repository.IRepository;
using ThePowerData.Data;

namespace ThePower.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Membership = new MembershipRepository(_db);
            AppUser= new AppUserRepository(_db);
            OrderHeader= new OrderHeaderRepository(_db);
            OrderDetail = new OrderDeatilRepository(_db);
            ShoppingCart= new ShoppingCartRepository(_db);
        }

        public IMembershipRepository Membership { get; private set; }
        public IAppUserRepository AppUser { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
