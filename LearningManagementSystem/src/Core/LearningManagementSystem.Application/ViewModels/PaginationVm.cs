using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class PaginationVm<T> where T : class
    {
        public ICollection<T> Items { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }
}