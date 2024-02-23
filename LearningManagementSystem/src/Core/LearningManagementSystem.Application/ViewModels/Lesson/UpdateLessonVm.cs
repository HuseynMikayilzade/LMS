using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateLessonVm
    {
        public string Name { get; set; }
        public string Tittle { get; set; } 
        public DateTime Date { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan EndDate { get; set; }
        // relation prop //
        public int SubjectId { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
