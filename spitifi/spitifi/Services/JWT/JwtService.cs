using Microsoft.AspNetCore.Identity;

namespace spitifi.Services.JWT;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;

    public JwtService(IConfiguration config, UserManager<IdentityUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }
    public async Task<string> GenerateToken(IdentityUser user) {
        
        var jwtSettings = _config.GetSection("Jwt");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),   // User ID
            new Claim(JwtRegisteredClaimNames.Email, user.Email),  // User Email - não será nulo pq é usado como UserName
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            
            // combine all roles into a single claim with comma separation
            new Claim(ClaimTypes.Role, string.Join(",", roles)) 

        };

        // assemble jwt token
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpireHours"])),
            signingCredentials: creds
        );
        
        // return serialized token to string format
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}