using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace MDFusionLabHaute.API.App_Start
{
    internal class JWTTokenValidationConfig : DelegatingHandler
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly string JWTSECRETKEY;
        private readonly string JWTVALIDISSUER;
        private readonly string JWTVALIDAUDIENCE;
        #endregion

        #region Constructor
        public JWTTokenValidationConfig(IConfiguration configuration)
        {
            _configuration = configuration;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
            JWTSECRETKEY = _configuration.GetConnectionString("JWTSecretKey");
            JWTVALIDISSUER = _configuration.GetConnectionString("JWTValidIssuer");
            JWTVALIDAUDIENCE = _configuration.GetConnectionString("JWTValidAudience");
        }
        #endregion

        /// <summary>
        /// GET JWT TOKENS FROM THE REQUEST HEADERS 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            HttpStatusCode statusCode;
            string token;
            //determine whether a jwt exists or not
            if (request.Method.Method == "OPTIONS" || !TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                //allow requests with no token - whether a action method needs an authentication can be set with the claimsauthorization attribute
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                string secretKey = JWTSECRETKEY;
                var now = DateTime.UtcNow;
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                string JWTValidIssuer = JWTVALIDISSUER;
                if (String.IsNullOrEmpty(JWTValidIssuer))
                    JWTValidIssuer = JWTVALIDISSUER;

                string JWTValidAudience = JWTVALIDAUDIENCE;
                if (String.IsNullOrEmpty(JWTValidAudience))
                    JWTValidAudience = JWTVALIDAUDIENCE;

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    LifetimeValidator = this.LifetimeValidator,
                    ValidIssuer = JWTValidIssuer,
                    ValidAudience = JWTValidAudience
                };
                //extract and assign the user of the jwt

                try
                {
                    //HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);
                    Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                    return base.SendAsync(request, cancellationToken);
                }
                catch { return base.SendAsync(request, cancellationToken); }

            }
            catch (SecurityTokenValidationException ex)
            {
                statusCode = HttpStatusCode.Unauthorized;

            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;

            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}