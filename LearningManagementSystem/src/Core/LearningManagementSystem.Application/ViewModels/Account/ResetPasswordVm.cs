using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class ResetPasswordVm
    {
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        [Required,DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!; 
        [Required, DataType(DataType.Password),Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = null!;   
    }
}
