using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateStudentVm
    {
        public string Name { get; set; } 
        public string Surname { get; set; } = null!;
        public Gender Gender { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; } = null!;       
        public bool IsFailed { get; set; }
        public byte TotalAttendance { get; set; }
        public bool IsGraduated { get; set; } = false;
        public bool? Degree { get; set; }
        // Relation Props //
        public Group? Group { get; set; }
        public int GroupId { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public string? Email { get; set; }
        public double? Point { get; set; }
        public ResetPasswordVm? ResetPasswordVm { get; set; }     
    }
}
