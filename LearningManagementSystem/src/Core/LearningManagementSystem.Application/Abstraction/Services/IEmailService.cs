using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string ToEmail, string subject, string body, bool ishtml);
    }
}
