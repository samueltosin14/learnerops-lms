using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace learneropsLms.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}