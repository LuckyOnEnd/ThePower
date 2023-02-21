using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThePower.Data.Repository.IRepository;
using ThePower.Models;

namespace ThePowerWeb.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _db;
       
        public HomeController(ILogger<HomeController> logger, IUnitOfWork db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetMemberships()
        {
            IEnumerable<Membership> objs = _db.Membership.GetAll();
            return View(objs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}