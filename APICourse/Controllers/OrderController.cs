using APICourse.Data;
using APICourse.DTO;
using APICourse.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APICourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(AppDbContext appDbContext,IMapper mapper) : ControllerBase
    {
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminOrders(CancellationToken cancellationToken)
        {
            var orders = await appDbContext.Orders
                .Include(s => s.OrderItems).ThenInclude(s => s.Product)
                .AsNoTracking().OrderByDescending(s => s.OrderDate).ToListAsync(cancellationToken);

            if (!orders.Any())
            {
                return NotFound("Orders not found");
            }
            var response=mapper.Map<IEnumerable<OrderResponseDto>>(orders); 
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetMyOrder(CancellationToken cancellationToken)
        {
            var userId=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await appDbContext.Orders
                .Include(s => s.OrderItems).ThenInclude(s => s.Product)
                .AsNoTracking().Where(s => s.UserId == userId).OrderByDescending(s => s.OrderDate).ToListAsync(cancellationToken);

            var response=mapper.Map<IEnumerable<OrderResponseDto>>(orders); 
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken)
        {
            //JWt ից ստացանք UserId, որը կօգտագործենք Cart գտնելու համար
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
           
            // Ստուգում ենք, թե արդյոք օգտատիրոջը համապատասխանող Cart գոյություն ունի և եթե կա, ստանում ենք այն
            var cart = await appDbContext.Carts
              .Include(s => s.CartItems).ThenInclude(s =>s.Product).FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);
            
            //Ստուգում ենք, թե արդյոք Cart գոյություն ունի և եթե ոչ, վերադարձնում ենք NotFound
            if (cart==null)
            {
                return NotFound("Cart not found"); 
            }
            // Ստուգում ենք, թե արդյոք Cart դատարկ է վերադարձնում ենք BadRequest
            if (!cart.CartItems.Any())
            {
                return BadRequest("Cart is empty"); 
            }
            // Ստեղծում ենք նոր Order օբյեկտը և լրացնում ենք այն անհրաժեշտ տվյալներով
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
            };
            //Cart-ի բոլոր CartItems-ները վերածում ենք տվյալ OrderItems-ների և ավելացնում ենք դրանք նոր ստեղծված Order օբյեկտին
            foreach (var cartItem in cart.CartItems)
            {
                var orderitem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity=cartItem.Quantity, 
                    UnitPrice= cartItem.Product.Price
                };
                order.OrderItems.Add(orderitem);
                order.TotalAmount += cartItem.Product.Price * cartItem.Quantity;
            }
            appDbContext.Orders.Add(order);
            cart.CartItems.Clear(); //Մաքրում ենք CartItems, քանի որ դրանք արդեն տեղափոխվել են OrderItems
            await appDbContext.SaveChangesAsync(cancellationToken);
            var respone=mapper.Map<OrderResponseDto>(order);
            return Ok(respone);
        }
    }
}
