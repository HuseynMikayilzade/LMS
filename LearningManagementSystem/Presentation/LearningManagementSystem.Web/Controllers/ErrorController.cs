using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage(string error)
        {
            return View(model: error);
        }

    }
}
