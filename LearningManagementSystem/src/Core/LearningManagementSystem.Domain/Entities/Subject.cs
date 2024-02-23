using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Subject:BaseNameableEntity
    {

        // Relation prop //
        public ICollection<GroupSubject>? GroupSubjects { get; set; }
       
        public ICollection<Teacher>? Teachers { get; set; }
    }
}
