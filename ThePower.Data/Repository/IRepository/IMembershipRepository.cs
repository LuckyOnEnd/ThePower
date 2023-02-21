using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePower.Models;

namespace ThePower.Data.Repository.IRepository
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        void Update(Membership membership);
    }
}
