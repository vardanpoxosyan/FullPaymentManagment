    namespace APICourse.DTO
    {
        public sealed record LoginResponseDto
        {
            public required string Role { get; init; }  
            public required bool IsSuccess { get; init; } 
            public required string AccessToken { get; init; }  
        }
    }
