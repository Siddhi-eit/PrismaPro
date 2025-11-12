using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MDFusionLabHaute.API.Helper
{
    public class JWTHelper
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly string JWTSECRETKEY = "6wSAGBSZh0zv4sZI63qD7KijhD4d4yLh8L4wd7maG2Yq6Coa6iq3R8Pump76j6Tm29le323Sk5j3ZTMbqe3bYu3db2X48mDbsxup8RiD9Ws6nsZMtJIxBIvvvEK59qHA";
        private readonly string JWTVALIDISSUER = "http://localhost";
        private readonly string JWTVALIDAUDIENCE = "http://localhost";
        #endregion

        #region Constructor
        public JWTHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }
        #endregion

        #region Methods
        public string GenerateJSONWebToken(string userName, string roleName)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);
            //DateTime expires = DateTime.UtcNow.AddMinutes(1);

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = JWTSECRETKEY;
            var jwtIssuer = JWTVALIDISSUER;
            var jwtAudience = JWTVALIDAUDIENCE;

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
            var now = DateTime.UtcNow;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var claims = new[] {
            //    new Claim("custom:UserName", userName),
            //    new Claim("custom:RoleID",roleId.ToString())
            //};
            ClaimsIdentity claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role,roleName)
            });

            var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(issuer: jwtIssuer,
                       audience: jwtAudience,
                       subject: claims,
                       notBefore: issuedAt,
                       expires: expires,
                       signingCredentials: credentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
        #endregion
    }
}