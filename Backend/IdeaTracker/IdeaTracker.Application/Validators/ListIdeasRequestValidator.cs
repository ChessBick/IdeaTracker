using FluentValidation;
using IdeaTracker.Application.Dtos;
using IdeaTracker.Domain.Enums;

namespace IdeaTracker.Application.Validators
{
    public class ListIdeasRequestValidator : AbstractValidator<ListIdeasRequest>
    {
        public ListIdeasRequestValidator()
        {
            RuleFor(x => x.Status)
                .Must(status => string.IsNullOrWhiteSpace(status)
                    || Enum.TryParse<IdeaStatus>(status, ignoreCase: true, out _))
                .WithMessage("Status must be one of: Proposed, InReview, Approved, Rejected.");
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
