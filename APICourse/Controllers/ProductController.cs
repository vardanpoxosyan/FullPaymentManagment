using APICourse.Data;
using APICourse.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext appDbContext,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var products = await appDbContext.Products
                .AsNoTracking().ToListAsync(cancellationToken);
            var map = mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(map); 
        }
    }
}
