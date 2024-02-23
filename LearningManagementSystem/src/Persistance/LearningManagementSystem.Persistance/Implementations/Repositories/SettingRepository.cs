using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Repositories
{
    public class SettingRepository:ISettingRepo
    {
        private readonly AppDbContext _context;

        public SettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IEnumerable<Setting>> GetAllSetting()
        {
            return await _context.Settings.ToListAsync();
        }

        public async Task<Setting> GetSettingById(int id)
        {
            return await _context.Settings.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task Update(Setting setting)
        {
            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
        }
        public async Task<Dictionary<string,string>> GetSettingsDictionary()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }
    }
}
