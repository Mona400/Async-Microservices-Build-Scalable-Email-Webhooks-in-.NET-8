using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using Shared.DTOs;
using Shared.Models;

namespace ProductApi.Repository
{
    public class ProductRepo(ProductDbContext context,IPublishEndpoint publishEndpoint) : IProduct
    {
        public async Task<ServiceResponse> AddProductAsync(Product product)
        {
           context.Products.Add(product);   
            await context.SaveChangesAsync();
           await publishEndpoint.Publish(product);
            return new ServiceResponse(true,"Product Added Successfully");
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            var products = context.Products.ToListAsync();
            return products;
        }
    }
}
