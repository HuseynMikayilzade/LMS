using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class TeacherResponseVm
    {     
        [Range(0, 100, ErrorMessage = "Point must be between 0 and 100")]
        public double Point { get; set; }
        public string? Note { get; set; }
        public StudentResponse? StudentResponse { get; set; }

    }
}
