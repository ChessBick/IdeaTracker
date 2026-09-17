using IdeaTracker.Application.Repository;
using IdeaTracker.Domain.Entities;
using IdeaTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IdeaTracker.Infrastructure.Persistence
{
    public class IdeaRepository(IdeaTrackerDbContext db) : IIdeaRepository
    {
        public void Add(Idea idea) => db.Ideas.Add(idea);

        public void Remove(Idea idea) => db.Ideas.Remove(idea);

        public Task<Idea?> GetById(int id) =>
            db.Ideas.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<(List<Idea> Items, int TotalCount)> ListIdeas(
            IdeaStatus? status, int page, int pageSize)
        {
            var query = db.Ideas.AsNoTracking().AsQueryable();
            if (status is not null)
                query = query.Where(i => i.Status == status);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Task SaveChanges() => db.SaveChangesAsync();
    }
}
