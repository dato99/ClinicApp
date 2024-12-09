using ClinicApp.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace ClinicApp.Auth
{
    public interface IJwtManager
    {
        Token GenerateToken(string email);
        Token GetToken(User user);
        Token GetToken(Doctor doctor);
        //Token GetToken(loginRequest loginRequest);
    }

    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Token GetToken(User user)
        {
            return GenerateToken(new Claim[]
            {
            new Claim("ID", user.Id.ToString(), ClaimValueTypes.Integer),
            new Claim("First_Name", user.FirstName, ClaimValueTypes.String),
            new Claim("Last_Name", user.LastName, ClaimValueTypes.String),
            new Claim("Email", user.Email, ClaimValueTypes.String),
            new Claim("Personal_Number", user.PersonalNumber, ClaimValueTypes.String),
            new Claim("Role", "User")
            });
        }

        public Token GetToken(Doctor doctor)
        {
            return GenerateToken(new Claim[]
            {
            new Claim("ID", doctor.ID.ToString(), ClaimValueTypes.Integer),
            new Claim("First_Name", doctor.FirstName, ClaimValueTypes.String),
            new Claim("Last_Name", doctor.LastName, ClaimValueTypes.String),
            new Claim("Email", doctor.Email, ClaimValueTypes.String),
            new Claim("Category", doctor.Category, ClaimValueTypes.String),
            new Claim("Role", "Doctor")
            });
        }

        //public Token GetToken(loginRequest loginRequest)
        //{
        //    // Avoid including sensitive information like passwords in tokens
        //    return GenerateToken(new Claim[]
        //    {
        //    new Claim("Email", loginRequest.Email, ClaimValueTypes.String)
        //    });
        //}
        public Token GenerateToken(string email)
        {
            // Generate a token based on the email
            return GenerateToken(new Claim[]
            {
            new Claim(ClaimTypes.Email, email, ClaimValueTypes.String),
            new Claim("Role", "User") // Assuming this is for users
            });
        }
        private Token GenerateToken(Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:TokenExpiryMinutes"] ?? "30")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { AccessToken = tokenHandler.WriteToken(token) };
        }
    }

    public class Token
    {
        public string AccessToken { get; set; }
    }
}
