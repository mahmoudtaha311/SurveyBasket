namespace SurveyBasket.Api.Abstractions;

public static class OptionsRateLimiting
{
    public const string ConcurrencyPolicy = "concurrency";
    public const int ConcurrencyPermitLimit = 1000;
    public const int ConcurrencyQueueLimit = 100;





    public const string UserLimitPolicy = "userLimit";
    public const int UserLimitPermitLimit = 2;
    public const int UserLimitQueueLimit = 1;
    public const int UserLimitWindow = 20;

    public const string IpLimitPolicy = "ipLimit";
    public const int IpLimitPermitLimit = 2;
    public const int IpLimitQueueLimit = 1;
    public const int IpLimitWindow = 20;

}
