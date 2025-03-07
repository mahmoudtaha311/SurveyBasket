namespace SurveyBasket.Api.Contracts.Authentication;

public record RefreshTokenRequest(string token, string refreshToken);
