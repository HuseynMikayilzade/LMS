using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class StudentResponseVm
    {
        public string? Response { get; set; }
        public IFormFile? AttachedFile { get; set; }
        public int? AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public bool IsSended { get; set; }
        public bool IsReplied { get; set; }



        public string? TeacherNote { get; set; }
        public double Point { get; set; }
    }
}
