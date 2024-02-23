using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Repositories.GenericRepo
{
    public class Repository<T> : IRepository<T>where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public IQueryable<T> GetAll(bool isTracking = false, bool queryFilter = false, params string[] includes)
        {
            IQueryable<T> entity = _dbSet;
            if (queryFilter) entity = entity.IgnoreQueryFilters();
            entity = Includes(entity, includes);
            return isTracking ? entity : entity.AsNoTracking();
        }

        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null,
         Expression<Func<T, object>>? orderexpression = null,
         bool isDescending = false, bool queryFilter = false, bool isTracking = false,
         int skip = 0, int take = 0, params string[] includes)
        {
            IQueryable<T> entity = _dbSet;

            if (expression != null) entity = entity.Where(expression);
            if (orderexpression != null)
            {
                if (isDescending) entity = entity.OrderByDescending(orderexpression);
                else entity = entity.OrderBy(orderexpression);
            }
            if (skip != 0) entity = entity.Skip(skip);
            if (take != 0) entity = entity.Take(take);
            entity = Includes(entity, includes);
            if (queryFilter) entity = entity.IgnoreQueryFilters();
            return isTracking ? entity : entity.AsNoTracking();
        }
        public async Task<bool> IsExist(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task<T> GetByIdAsync(int id, bool isTracking = false, bool queryFilter = false, params string[] includes)
        {
            IQueryable<T> entity = _dbSet.Where(t => t.Id == id);
            if (queryFilter) entity = entity.IgnoreQueryFilters();
            if (!isTracking) entity.AsNoTracking();
            entity = Includes(entity, includes);
            return await entity.FirstOrDefaultAsync();
        }

        public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool isTracking = false, bool queryFilter = false, params string[] includes)
        {
            IQueryable<T> entity = _dbSet.Where(expression);
            if (queryFilter) entity = entity.IgnoreQueryFilters();
            if (!isTracking) entity.AsNoTracking();
            entity = Includes(entity, includes);
            return await entity.FirstOrDefaultAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async void Update(T entity)
        {
            _dbSet.Update(entity);         
        }
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        private static IQueryable<T> Includes(IQueryable<T> entity, params string[] includes)
        {
            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    entity = entity.Include(includes[i]);
                }
            }
            return entity;
        }

        public void  Includes (T entity,params string[] includes)
        {
            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    _dbSet.Include(includes[i]);
                }
            }
        }
        public async Task<ICollection<E>> GetEntity<E>() where E : class
        {
           return await _context.Set<E>().ToListAsync();
        }


    }
}
