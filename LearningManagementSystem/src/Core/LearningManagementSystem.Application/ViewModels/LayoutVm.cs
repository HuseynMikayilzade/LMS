using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class LayoutVm
    {
        public AppUser user { get; set; }
        public TeacherItemVm TeacherItemVm { get; set; }
        public StudentItemVm StudentItemVm { get; set; }
    }
}
