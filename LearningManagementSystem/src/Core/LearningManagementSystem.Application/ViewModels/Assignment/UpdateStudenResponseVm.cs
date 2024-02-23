using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateStudenResponseVm
    {
        public string? Response { get; set; }
		public string? AttachedFilePath { get; set; }
		public IFormFile? AttachedFile { get; set; }
        public int? AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
    }
}
