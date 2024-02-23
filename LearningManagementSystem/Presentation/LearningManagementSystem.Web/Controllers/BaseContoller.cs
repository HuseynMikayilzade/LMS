using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult RedirectToIndexBasedOnRole()
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return RedirectToAction("Index", "Teacher");
            }
            else if (User.IsInRole(Role.Student.ToString()))
            {
                return RedirectToAction("Index", "Student");
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "manage");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
