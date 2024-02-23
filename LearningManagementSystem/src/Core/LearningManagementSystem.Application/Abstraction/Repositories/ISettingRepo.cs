using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Repositories
{
    public interface ISettingRepo
    {
        Task<IEnumerable<Setting>> GetAllSetting();
        Task<Setting> GetSettingById(int id);
        Task Update(Setting setting);
        Task<Dictionary<string, string>> GetSettingsDictionary();
    }
}
