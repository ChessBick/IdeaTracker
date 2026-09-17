using IdeaTracker.Domain.Entities;
using IdeaTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IdeaTracker.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task SeedAsync(IdeaTrackerDbContext dbContext, CancellationToken cancellationToken = default)
        {
            if (await dbContext.Ideas.AnyAsync(cancellationToken))
            {
                return;
            }

            var now = DateTime.Now;

            var ideas = new List<Idea>
            {
                new()
                {
                    Title = "Dark mode support",
                    Description = "Add a dark theme toggle to the web client for better usability at night.",
                    Status = IdeaStatus.Approved,
                    Tags = ["ui", "frontend", "accessibility"],
                    CreatedAt = now.AddDays(-14),
                    UpdatedAt = now.AddDays(-10)
                },
                new()
                {
                    Title = "Export ideas to CSV",
                    Description = "Allow users to export the full idea list to a CSV file for offline sharing.",
                    Status = IdeaStatus.InReview,
                    Tags = ["export", "backend"],
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-8)
                },
                new()
                {
                    Title = "Real-time notifications",
                    Description = "Notify users in real time when an idea they follow changes status.",
                    Status = IdeaStatus.Proposed,
                    Tags = ["notifications", "signalr"],
                    CreatedAt = now.AddDays(-7),
                    UpdatedAt = now.AddDays(-7)
                },
                new()
                {
                    Title = "Duplicate idea detection",
                    Description = "Use fuzzy matching to warn users when they submit a similar idea to an existing one.",
                    Status = IdeaStatus.Rejected,
                    Tags = ["search", "ml"],
                    CreatedAt = now.AddDays(-5),
                    UpdatedAt = now.AddDays(-4)
                },
                new()
                {
                    Title = "Voting on ideas",
                    Description = "Let users upvote or downvote ideas to help prioritize the roadmap.",
                    Status = IdeaStatus.Approved,
                    Tags = ["engagement", "voting"],
                    CreatedAt = now.AddDays(-3),
                    UpdatedAt = now.AddDays(-1)
                },
                new()
                {
                    Title = "Mobile app companion",
                    Description = "Build a lightweight mobile app to browse and submit ideas on the go.",
                    Status = IdeaStatus.Proposed,
                    Tags = ["mobile", "roadmap"],
                    CreatedAt = now.AddDays(-1),
                    UpdatedAt = now
                }
            };

            await dbContext.Ideas.AddRangeAsync(ideas, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
