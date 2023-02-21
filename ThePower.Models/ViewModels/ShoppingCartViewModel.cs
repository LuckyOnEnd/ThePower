using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePower.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public Membership Membership { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
