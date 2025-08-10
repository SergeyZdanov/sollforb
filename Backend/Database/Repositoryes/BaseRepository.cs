using Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected DatabaseContext Context { get; }
        protected DbSet<T> EntitySet { get; }

        protected BaseRepository(DatabaseContext databaseContext)
        {
            Context = databaseContext;
            EntitySet = Context.Set<T>();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            var result = await EntitySet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await EntitySet.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await EntitySet.ToListAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var obj = await EntitySet.FindAsync(id);
            EntitySet.Remove(obj);
            await Context.SaveChangesAsync();
        }
    }
}
