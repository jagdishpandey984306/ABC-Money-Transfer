using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoneyTransfer.Presentation.Custom
{
    public class CustomLoginRedirectAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _redirectUrl;

        public CustomLoginRedirectAttribute(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult(_redirectUrl);
            }
        }
    }
}
