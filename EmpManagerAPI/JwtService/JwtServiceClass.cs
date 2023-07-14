using EmpManagerAPI.JwtService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpManagerAPI.JwtService
{
    public class JwtServiceClass
    {
        public String SecretKey { get; set; }
        public int TokenDuration { get; set; }

        private readonly IConfiguration config;
        public JwtServiceClass(IConfiguration _config)
        {
            config = _config;
            this.SecretKey = config.GetSection("jwtConfig").GetSection("Key").Value;
            this.TokenDuration = Int32.Parse(config.GetSection("jwtConfig").GetSection("Duration").Value);
        }
        public String GenerateToken(String id, String name, String email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));

            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var payload = new[]
            {
                new Claim("id",id),
                new Claim("firstname",name),
                new Claim("email",email)
             };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: payload,
                expires: DateTime.Now.AddMinutes(TokenDuration),
                signingCredentials: signature
        );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
