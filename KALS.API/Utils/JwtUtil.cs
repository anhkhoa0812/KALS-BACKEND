using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KALS.Domain.Entity;
using Microsoft.IdentityModel.Tokens;

namespace KALS.API.Utils;

public class JwtUtil
{
    public JwtUtil()
    {
        
    }
    public static string GenerateJwtToken(User user, Tuple<string, Guid> guidClaim)
    {
        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        // byte[] keyBytes = new byte[32];
        // using (var rng = new RNGCryptoServiceProvider())
        // {
        //     rng.GetBytes(keyBytes);
        // }
        SymmetricSecurityKey secrectKey = 
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KitAndLabSystemSecretKeyForJWTToken"));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
        List<Claim> claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };
        
        if(guidClaim != null) claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
        var expires = DateTime.Now.AddDays(30);
        var token = new JwtSecurityToken("KitAndLabSystem", null, claims, notBefore: DateTime.Now, expires, credentials);
        return jwtHandler.WriteToken(token);
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "KitAndLabSystem",
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KitAndLabSystemSecretKeyForJWTToken")),
        };
        var tokenHander = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHander.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}