namespace Feedback.Api.Models;

public class Message
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public int ContactId { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}