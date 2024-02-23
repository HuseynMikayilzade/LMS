using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateTeacherVm
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public Gender Gender { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
        [DataType(DataType.Date)]     
        public DateTime Birthday { get; set; }      
        public string PhoneNumber { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Subject>? Subjects { get; set; }
        public int? SubjectId { get; set; }
        public ICollection<int>? GroupIds { get; set; }   
        public ICollection<Group>? Groups { get; set; }
        public string? Email { get; set; }
        public ResetPasswordVm? ResetPasswordVm { get; set; }
    }
}
