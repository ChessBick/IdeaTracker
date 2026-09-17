using IdeaTracker.Application.Dtos;
using IdeaTracker.Application.Services;
using IdeaTracker.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IdeaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/ideas")]
    public class IdeasController(IIdeaService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<IdeaDto>>>> List(
            [FromQuery] ListIdeasRequest request)
        {
            var result = await service.ListIdeas(request);
            return Ok(new ApiResponse<PagedResult<IdeaDto>>
            {
                Data = result,
                Success = true
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<IdeaDto>>> Get(int id)
        {
            var dto = await service.GetIdea(id);
            return Ok(new ApiResponse<IdeaDto>
            {
                Data = dto,
                Success = true
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<IdeaDto>>> Create(
            [FromBody] CreateIdeaRequest request)
        {
            var created = await service.CreateIdea(request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new ApiResponse<IdeaDto>
            {
                Data = created,
                Success = true
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<IdeaDto>>> Update(
            int id, [FromBody] UpdateIdeaRequest request)
        {
            var dto = await service.UpdateIdea(id, request);
            return Ok(new ApiResponse<IdeaDto>
            {
                Data = dto,
                Success = true
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteIdea(id);
            return NoContent();
        }
    }
}
