namespace SurveyBasket.Api.Contracts.User;

public record changePasswordRequest(
    string CurrentPassword,
    string NewPassword
    );
