
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Consumer;
using OrderApi.Data;
using OrderApi.Repository;

namespace OrderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region Connection to database
            builder.Services.AddDbContext<OrderDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));

            });
            #endregion
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductConsumer>();
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq://localhost", c =>
                    {
                        c.Username("guest");
                        c.Password("guest");
                    });
                    config.ReceiveEndpoint("product-queue", e =>
                    {
                        e.ConfigureConsumer<ProductConsumer>(context);
                    });
                });
            });
            builder.Services.AddScoped<IOrder,OrderRepo>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
