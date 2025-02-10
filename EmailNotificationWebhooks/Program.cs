using EmailNotificationWebhooks.Consumer;
using EmailNotificationWebhooks.Service;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace EmailNotificationWebhooks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           // builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddHttpClient<IEmailService,EmailService>();
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<WebHookConsumer>();
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq://localhost", c =>
                    {
                        c.Username("guest");
                        c.Password("guest");
                    });
                    config.ReceiveEndpoint("email-webhook-queue", e =>
                    {
                        e.ConfigureConsumer<WebHookConsumer>(context);
                    });
                });
            });
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");
            //configration rabbit-mq
            app.MapPost("/email-webhook", ([FromBody] EmailDTO email,IEmailService emailService) =>
            {
                string result=emailService.SendEmail(email);
                return Task.FromResult(result);
            });

            app.Run();
        }
    }
}
