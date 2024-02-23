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
    public class RegisterVm
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]

        public string Surname { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        [Required]

        public string EmailAdress { get; set; } = null!;
        [Required]

        public string Password { get; set; } = null!;
        [Required]

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public Gender Gender { get; set; }
        [Required]

        public DateTime BirthDay { get; set; }
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string PhoneNumber { get; set; } = null!;
        public IFormFile? Photo { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
