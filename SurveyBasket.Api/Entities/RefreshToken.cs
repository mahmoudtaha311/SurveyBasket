namespace SurveyBasket.Api.Entities;
[Owned]
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresOn { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? RevokOn { get; set; }

    public bool IsExpires => DateTime.UtcNow >= ExpiresOn;
    public bool IsActive => RevokOn is null && !IsExpires;

}
