using Application.Contracts;
using Dapper;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> :IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;
        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            await Task.CompletedTask;
        }
 
        public async Task DeleteAsync(T entity)
        {
              _db.Set<T>().Remove(entity);
              await _db.SaveChangesAsync();
              await Task.CompletedTask;
        }
        public IQueryable<T> FindAll()
        {
            return  _db.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _db.Set<T>().Where(expression);
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
            await Task.CompletedTask;
        }

    }
}
