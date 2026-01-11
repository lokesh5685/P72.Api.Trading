using P72.Api.Trading.Models;
using P72.Api.Trading.Models.Response;
using P72.Api.Common.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace P72.Api.Trading.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate? _next;
        private readonly IConfigManager? _configuration;
        private const string TiaaCacheKey = "TiaaMetadata";
        public AuthenticationMiddleware(RequestDelegate? next, IConfigManager? configuration)
        {
            _next = next;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            bool validateToken = Convert.ToBoolean(_configuration?.GetConfigurationSection("ValidateToken").Value);
            if (validateToken)
            {
                if (token == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var response = new TradApiErrorResponse((int)HttpStatusCode.Unauthorized,
                    "Missing Token");
                    await context.Response.WriteAsJsonAsync<TradApiErrorResponse>(response);
                }
                else
                {
                    var validateTokenResponse = ValidateToken(token);
                    if (!validateTokenResponse.Item1)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        var response = new TradApiErrorResponse((int)HttpStatusCode.Unauthorized,
                        validateTokenResponse.Item2);
                        await context.Response.WriteAsJsonAsync<TradApiErrorResponse>(response);
                    }
                    else
                    {
                        await _next!(context);
                    }
                }
            }
            else
            {
                await _next!(context);
            }
        }
        private JsonWebKey? GetSecretKey(string keyId)
        {
            var metadataUrl = _configuration?.GetConfigurationSection("TiaaMetadataUrl").Value;
            var tiaaKeys = new Metadata();

            if (tiaaKeys == null)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = client.GetAsync(metadataUrl);
                        var tiaaKeysResponse = response.Result.Content.ReadAsStringAsync().Result;
                        tiaaKeys = JsonConvert.DeserializeObject<Metadata>(tiaaKeysResponse);
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentNullException("Error while getting keys from JWKS \r\n" + ex.StackTrace);
                }
                finally
                {
                    tiaaKeys = null; 
                }
            }
            var secretKey = tiaaKeys?.keys?.FirstOrDefault(t => t.Kid == keyId);
            return secretKey;
        }
        private Tuple<bool, string> ValidateToken(string token)
        {
            bool isValidToken = false;
            string message = string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var decodedToken = tokenHandler.ReadJwtToken(token);
                var secretKeyId = decodedToken.Header.Kid;
                var secretKey = GetSecretKey(secretKeyId);
                if (secretKey != null)
                {
                    JsonWebKey jsonWebKey = new JsonWebKey(JsonConvert.SerializeObject(secretKey));
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jsonWebKey,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateActor = true,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                    isValidToken = true;
                }
                else
                {
                    message = String.Format("Key with Id {0} not found in JWKS", secretKeyId);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Tuple.Create(isValidToken, message);
        }
       
    }

}
