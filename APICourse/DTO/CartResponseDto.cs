namespace APICourse.DTO
{
    public sealed record CartResponseDto
    {
            public int CartId { get; set; }
            public List<CartItemResponseDto> Items { get; set; } = [];
    }
}
