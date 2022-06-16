namespace Application.Identity.Tokens.Queries;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);