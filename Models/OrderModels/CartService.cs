using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Data;
using Microsoft.EntityFrameworkCore;


namespace web_development_course.Models.OrderModels
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<List<Object>> GetUpdatedCartByUserId(int UserId)
        {
            List<OrderItem> OrderItems = await GetOrderItems(UserId, null);
            List<Object> updatedCart = new List<Object>();

            foreach (OrderItem orderItem in OrderItems)
            {
                List<String> ProductImages = orderItem.ProductType.Product.ProductImages.Select(pi => Convert.ToBase64String(pi.ImageData)).ToList();
                updatedCart.Add(new
                {
                    Id = orderItem.Id,
                    ProductId = orderItem.ProductType.Product.Id,
                    ProductName = orderItem.ProductType.Product.Name,
                    DiscountPercentage = orderItem.ProductType.Product.DiscountPercentage,
                    ProductQuantity = orderItem.ProductType.Quantity,
                    ProductImages = ProductImages,
                    ProductPrice = orderItem.ProductType.Product.Price,
                    Color = orderItem.ProductType.Color.Name,
                    Size = orderItem.ProductType.Size.ToString(),
                    Amount = orderItem.Amount,
                    TotalPrice = orderItem.TotalPrice
                });
            }

            return updatedCart;
        }

        public async Task<List<OrderItem>> GetOrderItemsByUserName(string UserName)
        {
            User user = await _context.User.FirstOrDefaultAsync(v => (v.FirstName + " " + v.LastName) == UserName);
            if (user == null)
            {
                throw new ArgumentException("User was not found");

            }

            return await GetOrderItems(user.Id, null);
        }

        public async Task<List<OrderItem>> GetOrderItems(int? UserId, int? OrderId)
        {
            Order order;
            if (UserId != null)
            {
                order = await GetOrCreateCartForUser((int)UserId);
            }
            else if (OrderId != null)
            {
                order = await _context.Order.FindAsync((int)OrderId);
            }
            else
            {
                throw new ArgumentException("Must provide user ID or an order ID");
            }

            return await _context.OrderItem
                .Include(oi => oi.ProductType)
                .ThenInclude(pd => pd.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(oi => oi.ProductType)
                .ThenInclude(pd => pd.Color)
                .Where(oi => oi.OrderId == order.Id)
                .ToListAsync();
        }

        public async Task<Order> GetOrCreateCartForUser(int UserId)
        {
            Order userCart = await _context.Order
                .Where(o => o.UserId == UserId && o.IsCart)
                .FirstOrDefaultAsync();

            if (userCart == null)
            {
                // The user does not have a cart, creating a new one
                userCart = new Order { UserId = UserId, IsCart = true, OrderItems = new List<OrderItem>() };
                _context.Add(userCart);
                await _context.SaveChangesAsync();
            }

            return userCart;
        }
    }
}
