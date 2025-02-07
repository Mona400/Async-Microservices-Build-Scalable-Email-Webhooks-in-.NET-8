using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using Shared.DTOs;
using Shared.Models;
using System.Text;

namespace OrderApi.Repository
{
    public class OrderRepo(OrderDbContext context, IPublishEndpoint publishEndpoint) : IOrder
    {
        public async Task<ServiceResponse> AddOrderAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var orderSummay = await GetSummaryAsync();
            var content = BuildOrderEmailBody(orderSummay.Id, orderSummay.ProductName, orderSummay.ProductPrice, orderSummay.Quantity, orderSummay.TotalAmount, orderSummay.Date);
            await publishEndpoint.Publish(new EmailDTO("Order Information", content));
            await ClearOrderTable();   
            return new ServiceResponse(true, "Order Added Successfully");
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await context.Orders.ToListAsync();
            return orders;
        }


        public async Task<OrderSummary> GetSummaryAsync()
        {
            var order = await context.Orders.FirstOrDefaultAsync();
            var products = await context.Products.ToListAsync();
            var productInfo = products.Find(x => x.Id == order!.ProductId);
            return new OrderSummary(
               order!.Id,
               order.ProductId,
               productInfo!.Name!,
               productInfo.Price,
               order.Quandtity,
               order.Quandtity * productInfo.Price,
               order.Date



                );
        }

        private string BuildOrderEmailBody(int orderId, string productName, decimal price, int orderQuantity, decimal totalAmount, DateTime Date)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1><strong>Order Information</strong></h1>");
            sb.AppendLine($"<p><strong>OrderId</strong>: {orderId} </p>");
            sb.AppendLine("<h2>Order Item: </h2>");
            sb.AppendLine("<ul>");
            sb.AppendLine($"<li> Name: {productName}</li>");
            sb.AppendLine($"<li> Price: {price}</li>");
            sb.AppendLine($"<li> Quantity: {orderQuantity}</li>");
            sb.AppendLine($"<li> Total Amount: {totalAmount}</li>");
            sb.AppendLine($"<li> Date: {Date}</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("Thank you for your order!");
            return sb.ToString();
        }
        private async Task ClearOrderTable()
        {
            context.Orders.Remove(await context.Orders.FirstOrDefaultAsync());
            await context.SaveChangesAsync();   
        }
    }
}
