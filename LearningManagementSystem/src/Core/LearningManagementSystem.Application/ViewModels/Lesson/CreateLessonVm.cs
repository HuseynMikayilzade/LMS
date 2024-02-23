using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class CreateLessonVm
    {
        public string Name { get; set; } = null!;
        public string Tittle { get; set; } = null!;
        public DateTime Date { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan EndDate { get; set; }
        // relation prop //
        public int SubjectId { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
