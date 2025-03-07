namespace SurveyBasket.Api.Entities;

public sealed class Answer
{
    public int AnswerId { get; set; }

    public string Content { get; set; } = string.Empty;

    public int QuestionId { get; set; }

    public bool IsActive { get; set; } = true;



    public Question question { get; set; } = default!;
}
