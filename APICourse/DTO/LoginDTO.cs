namespace APICourse.DTO
{
    public sealed record LoginDTO
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
