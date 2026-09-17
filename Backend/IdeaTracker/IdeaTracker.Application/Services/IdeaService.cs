using FluentValidation;
using IdeaTracker.Application.Dtos;
using IdeaTracker.Application.Exceptions;
using IdeaTracker.Application.Repository;
using IdeaTracker.Domain.Entities;
using IdeaTracker.Domain.Enums;
using IdeaTracker.Shared.Responses;

namespace IdeaTracker.Application.Services
{
    public class IdeaService(
           IIdeaRepository repository,
           IValidator<CreateIdeaRequest> createValidator,
           IValidator<UpdateIdeaRequest> updateValidator,
           IValidator<ListIdeasRequest> listValidator) : IIdeaService
    {
        public async Task<IdeaDto> CreateIdea(CreateIdeaRequest request)
        {
            await createValidator.ValidateAndThrowAsync(request);

            var now = DateTime.UtcNow;
            var idea = new Idea
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                Tags = NormalizeTags(request.Tags),
                Status = IdeaStatus.Proposed,
                CreatedAt = now,
                UpdatedAt = now
            };

            repository.Add(idea);
            await repository.SaveChanges();

            return ToDto(idea);
        }

        public async Task<IdeaDto> GetIdea(int id)
        {
            var idea = await repository.GetById(id)
                ?? throw new NotFoundException($"Idea with id {id} was not found.");
            return ToDto(idea);
        }

        public async Task<PagedResult<IdeaDto>> ListIdeas(ListIdeasRequest request)
        {
            await listValidator.ValidateAndThrowAsync(request);

            IdeaStatus? status = string.IsNullOrWhiteSpace(request.Status)
                ? null
                : Enum.Parse<IdeaStatus>(request.Status, ignoreCase: true);

            var (items, totalCount) = await repository.ListIdeas(status, request.Page, request.PageSize);
            var dtos = items.Select(ToDto).ToList();

            return PagedResult<IdeaDto>.Create(dtos, request.Page, request.PageSize, totalCount);
        }

        public async Task<IdeaDto> UpdateIdea(int id, UpdateIdeaRequest request)
        {
            await updateValidator.ValidateAndThrowAsync(request);

            var idea = await repository.GetById(id)
                ?? throw new NotFoundException($"Idea with id {id} was not found.");

            idea.Title = request.Title.Trim();
            idea.Description = request.Description?.Trim();
            idea.Status = Enum.Parse<IdeaStatus>(request.Status, ignoreCase: true);
            idea.Tags = NormalizeTags(request.Tags);
            idea.UpdatedAt = DateTime.UtcNow;

            await repository.SaveChanges();

            return ToDto(idea);
        }

        public async Task DeleteIdea(int id)
        {
            var idea = await repository.GetById(id)
                ?? throw new NotFoundException($"Idea with id {id} was not found.");

            repository.Remove(idea);
            await repository.SaveChanges();
        }

        private static List<string> NormalizeTags(List<string>? tags) =>
            tags?.Select(t => t.Trim())
                 .Where(t => !string.IsNullOrWhiteSpace(t))
                 .Distinct(StringComparer.OrdinalIgnoreCase)
                 .ToList()
            ?? [];

        private static IdeaDto ToDto(Idea idea) => new()
        {
            Id = idea.Id,
            Title = idea.Title,
            Description = idea.Description,
            Status = idea.Status.ToString(),
            Tags = idea.Tags,
            CreatedAt = idea.CreatedAt,
            UpdatedAt = idea.UpdatedAt
        };
    }
}
