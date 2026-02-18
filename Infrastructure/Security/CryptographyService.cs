using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Port;
namespace poketra_vyrt_api.Infrastructure.Security;

public class CryptographyService(IConfiguration configuration): ICryptographyService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password.Trim());
    }
    
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password.Trim(), hashedPassword);
    }

    public string GeneraAccessToken(WalletUser user)
    {
        var claims = CreateClaims(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private Claim[] CreateClaims(WalletUser user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("PhoneNumber", user.PhoneNumber),
            new Claim("AccountStatus", user.Status.ToString())
        ];
    }
    
}