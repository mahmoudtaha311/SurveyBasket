namespace SurveyBasket.Api.Contracts.Authentication;

public record AuthResponse(
    string Id,
    string? Email,
    string FirstName,
    string LasttName,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration

        );

