using Microsoft.AspNetCore.Mvc;

namespace Practical10.Middleware
{
    public class RequestLoggingMiddleware : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
