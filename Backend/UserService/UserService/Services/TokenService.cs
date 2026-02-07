using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.AppSettings;
using UserService.Interfaces.Services;

namespace UserService.Services;

public class TokenService : ITokenService
{
    private readonly JwtInformation _jwtInformation;

    public TokenService(JwtInformation jwtInformation)
    {
        _jwtInformation = jwtInformation;
    }

    public string GenerateToken(Guid id, string email, bool administrator)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.UTF8.GetBytes(_jwtInformation.TokenKey);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, id.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.Role, administrator ? "Admin" : "User")
        };
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtInformation.ExpiryTime),
            Issuer = _jwtInformation.Issuer,
            Audience = _jwtInformation.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        return tokenHandler.WriteToken(token);
    }
}