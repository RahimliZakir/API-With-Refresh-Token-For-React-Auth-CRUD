using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Application.WebAPI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static bool IsValid(this IActionContextAccessor ctx)
        {
            if (ctx.ActionContext is null)
                return false;

            return ctx.ActionContext.ModelState.IsValid;
        }
    }
}
