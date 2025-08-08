using Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositoryes
{
    public class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected DatabaseContext Context { get; }
        protected DbSet<T> EntitySet { get; }

        protected BaseRepository(DatabaseContext databaseContext)
        {
            Context = databaseContext;
            EntitySet = Context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await EntitySet.ToListAsync();
        }
    }
}
