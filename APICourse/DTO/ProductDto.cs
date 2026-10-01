namespace APICourse.DTO
{
    public sealed record ProductDto
    {
        public required string ProductName { get; init; }
        public required decimal ProductPrice { get; init; }
        public required int ProductStock { get; init; }
    }
}
