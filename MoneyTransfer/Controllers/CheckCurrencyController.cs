using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoneyTransfer.Presentation.Controllers
{
    public class CheckCurrencyController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
