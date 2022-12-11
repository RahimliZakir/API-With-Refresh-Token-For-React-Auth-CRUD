using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace Application.WebAPI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            int userId = Convert.ToInt32(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return userId;
        }

        public static int GetUserId(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.HttpContext.User.GetUserId();
        }

        public static int GetUserId(this IHttpContextAccessor ctx)
        {
            return ctx.HttpContext.User.GetUserId();
        }
    }
}
