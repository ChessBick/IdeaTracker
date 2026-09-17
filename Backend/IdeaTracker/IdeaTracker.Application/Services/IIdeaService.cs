using IdeaTracker.Application.Dtos;
using IdeaTracker.Shared.Responses;

namespace IdeaTracker.Application.Services
{
    public interface IIdeaService
    {
        Task<IdeaDto> CreateIdea(CreateIdeaRequest request);
        Task<IdeaDto> GetIdea(int id);
        Task<PagedResult<IdeaDto>> ListIdeas(ListIdeasRequest request);
        Task<IdeaDto> UpdateIdea(int id, UpdateIdeaRequest request);
        Task DeleteIdea(int id);
    }
}
