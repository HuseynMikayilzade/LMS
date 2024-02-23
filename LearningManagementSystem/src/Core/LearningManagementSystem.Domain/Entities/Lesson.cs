using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Lesson:BaseNameableEntity
    {
        public string Tittle { get; set; } = null!;
        public DateTime Date { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan EndDate { get; set; }
        public TimeSpan Duration { get; set; }

        // relation prop //
        public Subject Subject { get; set; } = null!;
        public int SubjectId { get; set; }

    }
}
