using APICourse.Data;
using APICourse.DTO;
using APICourse.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.XPath;

namespace APICourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
       
            var carts = await appDbContext.Carts
                .Include(s=>s.CartItems).AsNoTracking().FirstOrDefaultAsync(s=>s.UserId==int.Parse(userId!));
            if (carts == null)
            {
                return NotFound("Cart not found");
         
            }
            var response = new CartResponseDto
            {
                CartId = carts.CartId,
                Items = carts.CartItems.Select(x => new CartItemResponseDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Cart (AddToCartDto addToCartDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await appDbContext.Carts.FirstOrDefaultAsync(s => s.UserId ==int.Parse(userId!));
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = int.Parse(userId!)
                };
                appDbContext.Carts.Add(cart);
                await appDbContext.SaveChangesAsync();
            }
            var product = await appDbContext
                .Products.FirstOrDefaultAsync(s => s.ProductId == addToCartDto.ProductId);
            if (product is null)
            {
                return NotFound("Product not found.");
            }
            var cartitem = await appDbContext.CartItems.FirstOrDefaultAsync(s => 
                s.CartId == cart.CartId&& s.ProductId == addToCartDto.ProductId);
            if (cartitem is not null)
            {
                cartitem.Quantity += addToCartDto.Quantity;
            }
            else
            {
                cartitem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Quantity = addToCartDto.Quantity
                };
               await appDbContext.CartItems.AddAsync(cartitem);
            }
            await appDbContext.SaveChangesAsync();
            return Ok(new CartItemResponseDto
            {
                 ProductId= cartitem.ProductId,
                 Quantity=cartitem.Quantity
            });
        }
        [HttpPut]
        public async Task<IActionResult> CartEdit(int cartitemid,CartUpdateDto cartUpdateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
            var cartitem = await appDbContext.CartItems.Include(s => s.Cart)
                .FirstOrDefaultAsync(s => s.CartItemId == cartitemid && s.Cart.UserId == int.Parse(userId!));
            if (cartitem == null)
            {
                return NotFound("Cart item not found");
            }
            if (cartUpdateDto.Quantity <= 0)
            {
                return BadRequest("Quanity must be greater than 0");
            }
            cartitem.Quantity =cartUpdateDto.Quantity;
            await appDbContext.SaveChangesAsync();
            return Ok("Quantity updated seccessfully");
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveCartItem(int cartitemId)
        {
            if (cartitemId <= 0)
            {
                return BadRequest("Id must be greater than zero");
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var cartItem = await appDbContext.CartItems
                .Include(s => s.Cart)
                .SingleOrDefaultAsync(s =>
                    s.CartItemId == cartitemId &&
                    s.Cart.UserId == userId);
            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }
            appDbContext.CartItems.Remove(cartItem);
            await appDbContext.SaveChangesAsync();
            return Ok("Cart item removed successfully");
        }
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await appDbContext.Carts.FirstOrDefaultAsync(s =>s.UserId ==int.Parse( userId!));
            if (cart == null)
            {
                return NotFound("Cart Now Empty");
            }
            var cartitems = await appDbContext.CartItems.Where(s => s.Cart.CartId == cart.CartId).ToListAsync();
            appDbContext.CartItems.RemoveRange(cartitems);
            await appDbContext.SaveChangesAsync();

            return Ok("Clear cart");

        }
    }
}
