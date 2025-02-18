using Entities.Enums;
using Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MethodsParameters
{
    public static class TokenConfig
    {
        #region Login

        private static readonly List<string> sections = new List<string>() { "Access:ApiPublica"};
        private static Dictionary<string, List<string>> _AccessList = new Dictionary<string, List<string>>();

        public static Dictionary<string, List<string>> AccessList(IConfiguration _configuration)
        {
            if (_AccessList.Count != sections.Count)
            {
                _AccessList.Clear();
                sections.ForEach(s =>
                {
                    var items = _configuration.GetSection(s);
                    if (items != null)
                    {
                        List<string> ids = items.GetChildren().Select(x => x.Value).ToList();
                        _AccessList.Add(s, ids);
                    }
                });
            }
            return _AccessList;
        }

       

        public static async Task<string> CreateTokenAsync(ApplicationUser user, IConfiguration _configuration, enumAccess enumAccess)
        {
            var claims = await GetClaims(user, enumAccess);
            var tokenOptions = GenerateTokenOptions(claims, _configuration);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private static SigningCredentials GetSigningCredentials(IConfiguration _configuration)
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private static async Task<List<Claim>> GetClaims(ApplicationUser user, enumAccess enumAccess)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, enumAccess.ToString()),
                new Claim(ClaimTypes.Sid, user.IdUsuario.ToString()),

            };
            return claims;
        }

        private static JwtSecurityToken GenerateTokenOptions(List<Claim> claims, IConfiguration _configuration)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
                signingCredentials: GetSigningCredentials(_configuration)
            );
            return tokenOptions;
        }


        #endregion
    }
}
