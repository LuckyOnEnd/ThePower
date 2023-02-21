using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThePower.Data.Repository;
using ThePower.Data.Repository.IRepository;
using ThePower.Models;
using ThePower.Models.ViewModels;
using ThePower.Utility;

namespace ThePower.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _db;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        private int MemberId { get; set; }
        public CartController(IUnitOfWork db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartViewModel  = new()
            {
                Membership = _db.Membership.GetFirst(x => x.Id == id), 
                OrderHeader = new()
            };

            ShoppingCartViewModel.OrderHeader.AppUser = _db.AppUser.GetFirst(x => x.Id == claim.Value);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.AppUser.Email;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.AppUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.StreetAdress = ShoppingCartViewModel.OrderHeader.AppUser.StreetAdress;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.AppUser.City;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.AppUser.PostalCode;

            ShoppingCartViewModel.OrderHeader.OrderTotal = Convert.ToDouble(_db.Membership.GetFirst(x => x.Id == id).Price);

            return View(ShoppingCartViewModel);
        }

        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPOST()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel.Membership = _db.Membership.GetFirst(x => x.Id == MemberId);
            
            ShoppingCartViewModel.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartViewModel.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartViewModel.OrderHeader.AppUserId = claim.Value;

            ShoppingCartViewModel.OrderHeader.OrderTotal = Convert.ToDouble(ShoppingCartViewModel.Membership.Price);

            _db.OrderHeader.Add(ShoppingCartViewModel.OrderHeader);
            _db.Save();

            OrderDetail orderDetail = new()
            {
                ProductId = ShoppingCartViewModel.Membership.Id,
                OrderId = ShoppingCartViewModel.OrderHeader.Id,
                Price = ShoppingCartViewModel.Membership.Price,
            };
            _db.OrderDetail.Add(orderDetail);
            _db.Save();


            return RedirectToAction("Home", "GetMemberships");
        }
    }
}
