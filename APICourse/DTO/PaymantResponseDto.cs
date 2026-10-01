using APICourse.Models;

namespace APICourse.DTO
{
    public sealed record PaymantResponseDto
    {
        public required int Id { get;init; }
        public required int OrderId { get;init; }
        public required decimal Amount { get; init; }    
        public required PaymentStatus Status{ get; init; }
        public required DateTime PaymentDate { get; init; }

    }
}
