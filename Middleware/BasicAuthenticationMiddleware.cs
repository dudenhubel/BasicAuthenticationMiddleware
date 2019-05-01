using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthenticationMiddleware
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authorizationHeader = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Authorization Header is not present in your request.");
                return;
            }

            if (!authorizationHeader.StartsWith("Basic "))
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Authorization Header is not valid for Basic Authentication.");
                return;
            }

            string authenticationToken = authorizationHeader.Substring("Basic ".Length).Trim();
            string decodedAuthenticationToken;
            try
            {
                decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            }
            catch (FormatException formatException)
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync(formatException.Message);
                return;
            }

            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
            string username;
            string password;
            try
            {
                username = usernamePasswordArray[0];
                password = usernamePasswordArray[1];
            }
            catch (IndexOutOfRangeException)
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Invalid value for Authentication Header.");
                return;
            }

            // Custom logic for checking username and password
            if (username == "myusername" && password == "mypassword")
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }
    }
}
