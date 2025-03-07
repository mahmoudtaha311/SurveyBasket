namespace SurveyBasket.Api.Contracts.Authentication;

public record RegisterRequest(
    string Email,
    string PassWord,
    string FirstName,
    string LastName


    );
