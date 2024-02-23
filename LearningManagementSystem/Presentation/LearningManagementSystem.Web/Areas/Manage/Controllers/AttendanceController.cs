using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AttendanceController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }
       

    }
}
