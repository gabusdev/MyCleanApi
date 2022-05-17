namespace Application.Identity.Tokens.TokenQueries;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);