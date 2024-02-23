using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public Gender Gender  { get; set; }
        public DateTime BirthDay { get; set; }
        public string Image { get; set; } = "profil.png";
        public Role Role { get; set; }
        public bool? IsLogin { get; set; } 

    }
}