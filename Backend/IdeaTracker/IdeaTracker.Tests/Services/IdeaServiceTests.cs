using FluentValidation;
using IdeaTracker.Application.Dtos;
using IdeaTracker.Application.Services;
using IdeaTracker.Application.Validators;
using IdeaTracker.Test.FakeInitailization;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Test.Services
{
    public class IdeaServiceTests
    {
        private static IdeaService CreateService(out FakeIdeaRepository repository)
        {
            repository = new FakeIdeaRepository();
            return new IdeaService(
                repository,
                new CreateIdeaRequestValidator(),
                new UpdateIdeaRequestValidator(),
                new ListIdeasRequestValidator());
        }

        [Fact]
        public async Task CreateIdea_WithValidRequest_ReturnsIdeaWithProposedStatus()
        {
            var service = CreateService(out _);
            var request = new CreateIdeaRequest
            {
                Title = "Offline-first field reporting",
                Description = "Enable mobile users to capture reports offline and sync later.",
                Tags = ["mobile", "sync"]
            };

            var result = await service.CreateIdea(request);

            Assert.True(result.Id > 0);
            Assert.Equal("Proposed", result.Status);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(["mobile", "sync"], result.Tags);
        }

        [Fact]
        public async Task CreateIdea_WithEmptyTitle_ThrowsValidationException()
        {
            var service = CreateService(out _);
            var request = new CreateIdeaRequest { Title = "" };

            await Assert.ThrowsAsync<ValidationException>(() => service.CreateIdea(request));
        }

        [Fact]
        public async Task ListIdeas_WithStatusFilter_ReturnsOnlyMatchingIdeas()
        {
            var service = CreateService(out var repository);
            await service.CreateIdea(new CreateIdeaRequest { Title = "Idea A" });
            var created = await service.CreateIdea(new CreateIdeaRequest { Title = "Idea B" });
            await service.UpdateIdea(created.Id, new UpdateIdeaRequest
            {
                Title = created.Title,
                Status = "Approved"
            });

            var result = await service.ListIdeas(new ListIdeasRequest { Status = "Approved" });

            Assert.Single(result.Items);
            Assert.Equal("Approved", result.Items[0].Status);
        }
    }
}
