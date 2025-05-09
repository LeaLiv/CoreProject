
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using firstProject.Models;
using Microsoft.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace firstProject.Services
{
    public static class UserTokenService
    {
        private static UserService userService;//=new UserService();

    public static void InitializeUserService(IHostEnvironment env){
        if(userService == null)
        {
            userService = new UserService(env);
        }
    }
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ")
        );
        // private static Lazy<UserService> userService = new Lazy<UserService>(() =>
        // {
        //     var env = new HostEnvironment(); 
        //         });

        private static string issuer = "https://shoes.com";
        public static SecurityToken GetToken(List<Claim> claims) =>
    new JwtSecurityToken(
        issuer,
        issuer,
        claims,
        expires: DateTime.Now.AddDays(30.0),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );

        public static TokenValidationParameters
GetTokenValidationParameters() =>
new TokenValidationParameters
{
    ValidIssuer = issuer,
    ValidAudience = issuer,
    IssuerSigningKey = key,
    ClockSkew = TimeSpan.Zero // remove delay of token when expire
};
        public static string WriteToken(SecurityToken token) =>
       new JwtSecurityTokenHandler().WriteToken(token);

        public static User GetUserFromToken(string token)
        {
            string id = decoderToken(token);
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Token does not contain user id");
                return null;
            }
            return userService.Get(int.Parse(id));
        }

        public static string decoderToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token is null or empty");
                return null;
            }
            if (token.StartsWith("Bearer "))
                token = token.Substring(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                if (!tokenHandler.CanReadToken(token))
                {
                    Console.WriteLine("Token is invalid");
                    return null;
                }
                var jwtToken = tokenHandler.ReadJwtToken(token);
                if (jwtToken.Payload.ContainsKey("id"))
                    return jwtToken.Payload["id"].ToString();
                else
                {
                    Console.WriteLine("Token does not contain user id");
                    return null;
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}