using LearningManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Student:BaseNameableEntity
    {   
        public string Surname { get; set; } = null!;
        public Gender Gender { get; set; }
        public string? Image { get; set; } 
        public DateTime Birthday { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public double Point { get; set; }
        public double Avarage { get; set; }
        public bool IsFailed { get; set; }
        public byte TotalAttendance { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsGraduated { get; set; } = false;
        public bool? Degree { get; set; }

        // Relation Props //
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public Group? Group { get; set; } 
        public int? GroupId { get; set; }
          

    }
}