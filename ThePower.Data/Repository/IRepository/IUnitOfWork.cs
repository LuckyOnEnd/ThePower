using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePower.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IMembershipRepository Membership { get; }
        IAppUserRepository AppUser { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IShoppingCartRepository ShoppingCart { get; }
        void Save();
    }
}
