using IdeaTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Domain.Entities
{
    public class Idea
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IdeaStatus Status { get; set; } = IdeaStatus.Proposed;
        public List<string> Tags { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
