namespace APICourse.DTO
{
    public sealed record CartItemResponseDto
    {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
    }
}
