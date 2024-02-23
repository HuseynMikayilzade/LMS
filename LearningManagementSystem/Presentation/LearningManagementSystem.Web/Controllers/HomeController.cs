
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LearningManagementSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToIndexBasedOnRole();
            }
            return View();
        }   
    }
}