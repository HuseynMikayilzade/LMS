using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool isTracking = false, bool queryFilter = false, params string[] includes);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>? orderexpression = null,
            bool isDescending = false,
            bool isTracking = false,
            bool queryFilter = false,
            int skip = 0,
            int take = 0,
            params string[] includes);
        Task<bool> IsExist(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int id, bool isTracking = false, bool queryFilter = false, params string[] includes);
        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool isTracking = false, bool queryFilter = false, params string[] includes);
        Task AddAsync(T entity); 
        void Delete(T entity);
        void Update(T entity);
        Task SaveChangesAsync();
        void Includes(T entity,params string[] includes);
        Task<ICollection<E>> GetEntity<E>() where E : class;
       
    }
}