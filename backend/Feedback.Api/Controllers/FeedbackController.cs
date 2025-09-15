using Microsoft.AspNetCore.Mvc;
using Feedback.Api.Models;
using Feedback.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Feedback.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] FeedbackDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Captcha != "0096")
                return BadRequest("Неверная CAPTCHA");

            var existingContact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Email == dto.Email && c.Phone == dto.Phone);

            Contact contact;

            if (existingContact == null)
            {
                contact = new Contact
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone
                };
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
            }
            else
            {
                contact = existingContact;
            }

            var message = new Message
            {
                TopicId = dto.TopicId,
                ContactId = contact.Id,
                MessageText = dto.MessageText,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                id = message.Id,
                topicId = message.TopicId,
                contact = new { contact.Name, contact.Email, contact.Phone },
                messageText = message.MessageText,
                createdAt = message.CreatedAt
            });
        }
    }
}