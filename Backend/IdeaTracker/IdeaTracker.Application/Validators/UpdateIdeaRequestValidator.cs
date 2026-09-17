using FluentValidation;
using IdeaTracker.Application.Dtos;
using IdeaTracker.Domain.Enums;

namespace IdeaTracker.Application.Validators
{
    public class UpdateIdeaRequestValidator : AbstractValidator<UpdateIdeaRequest>
    {
        public UpdateIdeaRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(4000);
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(status => Enum.TryParse<IdeaStatus>(status, ignoreCase: true, out _))
                .WithMessage("Status must be one of: Proposed, InReview, Approved, Rejected.");
            RuleFor(x => x.Tags)
                .Must(tags => tags == null || tags.Count <= 10)
                .WithMessage("A maximum of 10 tags is allowed.");
            RuleForEach(x => x.Tags).MaximumLength(50)
                .When(x => x.Tags is not null);
        }
    }
}
