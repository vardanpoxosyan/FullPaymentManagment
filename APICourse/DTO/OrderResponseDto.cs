using APICourse.Models;

namespace APICourse.DTO
{
    public sealed record OrderResponseDto
    {
        public required int OrderId { get; init; }
        public required DateTime OrderDate { get; init; }
        public  required decimal TotalAmount { get; init; }
        public required OrderStatus OrderStatus{ get; init; }
    }
}
