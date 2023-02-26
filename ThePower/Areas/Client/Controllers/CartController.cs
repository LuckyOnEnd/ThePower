using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
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
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        [BindProperty]
        private int MemberId { get; set; }
        public CartController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Details(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Membership = _db.Membership.GetFirst(x => x.Id == id), 
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.AppUser = _db.AppUser.GetFirst(x => x.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.AppUser.Email;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.AppUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAdress = ShoppingCartVM.OrderHeader.AppUser.StreetAdress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.AppUser.City;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.AppUser.PostalCode;

            ShoppingCartVM.OrderHeader.OrderTotal = Convert.ToDouble(_db.Membership.GetFirst(x => x.Id == id).Price);
            MemberId = ShoppingCartVM.Membership.Id;

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPOST(ShoppingCartViewModel ShoppingCartVM, int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.Membership = _db.Membership.GetFirst(x => x.Id == id);

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.AppUserId = claim.Value;

            ShoppingCartVM.OrderHeader.OrderTotal = Convert.ToDouble(ShoppingCartVM.Membership.Price);

            _db.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _db.Save();

            OrderDetail orderDetail = new()
            {
                ProductId = ShoppingCartVM.Membership.Id,
                OrderId = ShoppingCartVM.OrderHeader.Id,
                Price = ShoppingCartVM.Membership.Price,
            };
            _db.OrderDetail.Add(orderDetail);
            _db.Save();

            // stripe API
            var domain = "https://localhost:7004/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (long)(ShoppingCartVM.OrderHeader.OrderTotal * 100),
                      Currency = "pln",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = ShoppingCartVM.Membership.Name,
                      },
                    },
                    Quantity = 1,
                  },
                },
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                Mode = "payment",
                SuccessUrl = domain+$"client/cart/OrderConfirm?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"client/home/index",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;
            ShoppingCartVM.OrderHeader.PaymentIntentId = session.PaymentIntentId;
            _db.OrderHeader.UpdateStripe(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _db.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        public IActionResult OrderConfirm(int id)
        {
            OrderHeader orderHeader = _db.OrderHeader.GetFirst(x => x.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            //check status

            if(session.PaymentStatus.ToLower() == "paid")
            {
                _db.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                _db.Save();
            }
            return View(id);
        }
    }
}
