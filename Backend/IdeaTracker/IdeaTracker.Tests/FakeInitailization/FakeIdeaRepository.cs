using IdeaTracker.Application.Repository;
using IdeaTracker.Domain.Entities;
using IdeaTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Test.FakeInitailization
{
    public class FakeIdeaRepository : IIdeaRepository
    {
        private readonly List<Idea> _ideas = [];
        private int _nextId = 1;

        public void Add(Idea idea)
        {
            idea.Id = _nextId++;
            _ideas.Add(idea);
        }

        public void Remove(Idea idea) => _ideas.Remove(idea);

        public Task<Idea?> GetById(int id) =>
            Task.FromResult(_ideas.FirstOrDefault(i => i.Id == id));

        public Task<(List<Idea> Items, int TotalCount)> ListIdeas(IdeaStatus? status, int page, int pageSize)
        {
            var query = _ideas.AsEnumerable();
            if (status is not null)
                query = query.Where(i => i.Status == status);

            var total = query.Count();
            var items = query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult((items, total));
        }

        public Task SaveChanges() => Task.CompletedTask;
    }
}
