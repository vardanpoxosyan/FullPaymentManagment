namespace APICourse.DTO
{
    public sealed record RegisterResponseDto
    {
        public required int UserId { get; init; }
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Role { get; init; }
        public required DateTime  CreatedAt{ get; init; }
    }
}
