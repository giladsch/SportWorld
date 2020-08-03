using Microsoft.AspNetCore.Mvc;

namespace MovieLand.Controllers
{
    public class ErrorController : Controller
    {
        private const string DefaultErrorMessage = "An error has occured";

        public IActionResult Index(string error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.ErrorMessage = error;
            }
            else
            {
                ViewBag.ErrorMessage = DefaultErrorMessage;
            }

            return View();
        }
    }
}