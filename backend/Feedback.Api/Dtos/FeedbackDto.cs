namespace Feedback.Api.Dtos;

public class FeedbackDto
{
    public int TopicId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string MessageText { get; set; } = string.Empty;
    public string Captcha { get; set; } = string.Empty;
}