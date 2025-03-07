namespace SurveyBasket.Api.Abstractions;

public static class RegexPatterns
{
    public const string password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=(.*[a-z]))(?=(.*[A-Z]))(?=(.*)).{8,}";
}
