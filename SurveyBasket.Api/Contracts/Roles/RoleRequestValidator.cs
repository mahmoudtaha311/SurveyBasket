namespace SurveyBasket.Api.Contracts.Roles;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 200);


        RuleFor(x => x.Premissions)
           .NotEmpty()
           .NotNull();
        RuleFor(x => x.Premissions)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("you cannot add duplicated permissionss for the same role")
            .When(x => x.Premissions != null);
    }
}
