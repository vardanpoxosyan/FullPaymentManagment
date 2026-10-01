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
    public class PaymentController(AppDbContext appDbContext,IMapper mapper) : ControllerBase
    {
        [HttpGet("AdminPayments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminPayments(CancellationToken cancellationToken)
        {

            var payments = await appDbContext.Payments
                .Include(p => p.Order)
                .AsNoTracking()
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
            if (!payments.Any())
            {
                return NotFound("Payments not found");
            }
            var response = mapper.Map<IEnumerable<PaymantResponseDto>>(payments);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetMyPayments(CancellationToken cancellationToken)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var payments = await appDbContext.Payments
                .Include(p => p.Order)
                .AsNoTracking()
                .Where(p => p.Order.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync(cancellationToken);
            if (!payments.Any())
            {
                return NotFound("Payments not found");
            }
            var response = mapper.Map<IEnumerable<PaymantResponseDto>>(payments);
            return Ok(response);
        }
        [HttpPost("{orderId}")]
        public async Task<IActionResult> CreatePayment(
    int orderId,
    CancellationToken cancellationToken)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
          var order=await appDbContext.Orders.FirstOrDefaultAsync(s=>s.Id==orderId&&s.UserId==userId,cancellationToken);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            if (order.Status != OrderStatus.Pending)
            {
                return BadRequest("Order cannot be paid");
            }
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };
            appDbContext.Payments.Add(payment);
            await appDbContext.SaveChangesAsync(cancellationToken);
            var response = mapper.Map<PaymantResponseDto>(payment);

            return Ok(response);
        }
    }
}
