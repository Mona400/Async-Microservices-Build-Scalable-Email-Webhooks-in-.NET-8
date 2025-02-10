using MassTransit;
using Shared.DTOs;

namespace EmailNotificationWebhooks.Consumer
{
    public class WebHookConsumer(HttpClient client) : IConsumer<EmailDTO>
    {
        public async Task Consume(ConsumeContext<EmailDTO> context)
        {
            var result = await client.PostAsJsonAsync("https:localhost:7078/email-webhook", new EmailDTO(context.Message.Title, context.Message.Content));
            result.EnsureSuccessStatusCode();
        }
    }
}
