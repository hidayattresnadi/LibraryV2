using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Context;

namespace WebAPITutorial.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MyDbContext _db;
        private readonly DbSet<T> _dbSet;
        public Repository(MyDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }
        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return true;
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> queries = _dbSet.Where(expression);
            return await queries.ToListAsync();
        }
        public bool Remove(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }
        public bool RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return true;
        }
        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
