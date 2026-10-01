namespace APICourse.DTO
{
    public sealed record OrderItemResponseDto
    {
        public required int ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int Quantity { get; init; }
        public required decimal Price { get; init; }
    }
}
