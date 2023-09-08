using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DotNet7.Template.Api.HttpClients;

namespace DotNet7.Template.Api.Filters
{
    public class VerifyAccessTokenAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private SingleSignOnClient _singleSignOnClient;

        public VerifyAccessTokenAuthorizeAttribute(SingleSignOnClient httpClient)
        {
            _singleSignOnClient = httpClient;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool authorized = await _verifyAccessToken(accessToken);

            if (!authorized)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private async Task<bool> _verifyAccessToken(string? accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            _singleSignOnClient.AddHeaders("Authorization", accessToken);

            var result = await _singleSignOnClient.VerifyAuthorization();

            return result.IsSuccessStatusCode;
        }
    }
}
