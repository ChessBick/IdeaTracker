using IdeaTracker.Domain.Entities;
using IdeaTracker.Domain.Enums;

namespace IdeaTracker.Application.Repository
{
    public interface IIdeaRepository
    {
        void Add(Idea idea);
        void Remove(Idea idea);
        Task<Idea?> GetById(int id);
        Task<(List<Idea> Items, int TotalCount)> ListIdeas(IdeaStatus? status, int page, int pageSize);
        Task SaveChanges();
    }
}
