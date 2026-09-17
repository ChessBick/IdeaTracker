using FluentValidation;
using IdeaTracker.Application.Dtos;

namespace IdeaTracker.Application.Validators
{
    public class CreateIdeaRequestValidator : AbstractValidator<CreateIdeaRequest>
    {
        public CreateIdeaRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(4000);
            RuleFor(x => x.Tags)
                .Must(tags => tags == null || tags.Count <= 10)
                .WithMessage("A maximum of 10 tags is allowed.");
            RuleForEach(x => x.Tags).MaximumLength(50)
                .When(x => x.Tags is not null);
        }
    }
}
