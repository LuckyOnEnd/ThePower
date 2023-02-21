using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePower.Data.Repository.IRepository;
using ThePower.Models;

namespace ThePower.Data.Repository
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
    }
}
