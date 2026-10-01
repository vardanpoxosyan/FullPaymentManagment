namespace APICourse.DTO
{
    public sealed record RegisterDTO
    {
        public required string UserName { get; init; } 
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
