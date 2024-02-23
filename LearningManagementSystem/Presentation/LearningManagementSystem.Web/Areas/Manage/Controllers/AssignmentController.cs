using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _service;

        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ActivateTask(int id)
        {
            await _service.ActivateAsync(id);
            return Ok(new returnData {statusCode = 200,message = "Success" });
        }
        public async Task<IActionResult> DeActivateTask(int id)
        {
            await _service.DeActivateAsync(id);
            return Ok(new returnData { statusCode = 200, message = "Success" });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateAssignmentVm data)
        {
            if(await _service.CreateAsync(data, ModelState))
            {
                return Ok(new returnData { statusCode = 200, message = "Success" });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorMessage = string.Empty;
                foreach (var item in errors)
                {
                    if (!item.ErrorMessage.Contains("GroupId"))
                    {
                        errorMessage += item.ErrorMessage + "\n";
                    }
                }
                return Ok(new returnData { statusCode = 400, message = errorMessage });
            }
        }
        public class returnData
        {
            public int statusCode { get; set; }
            public string message { get; set; }
        }
    }
}
