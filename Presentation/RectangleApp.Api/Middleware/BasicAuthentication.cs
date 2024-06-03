using Project.Core.Settings;
using System.Net;
using System.Text;

namespace RectangleApp.Api.Middleware
{
    public class BasicAuthentication
    {
        private readonly RequestDelegate _next;

        public BasicAuthentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                var rawCredentials = authHeader.Substring(6);
                var convertedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(rawCredentials));
                var userNameAndPassword = convertedCredentials.Split(':');
                var username = userNameAndPassword[0];
                var password = userNameAndPassword[1];

                if (username != AppSettings.Settings.Username || password != AppSettings.Settings.Password)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(ex.ToString());
            }

            return;
        }
    }
}
