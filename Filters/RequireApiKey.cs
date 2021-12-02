using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords.Filters
{
    public class RequireApiKeyFilter : IActionFilter
    {
        private const string ApiKeyHeaderName = "X-API-Key";

        private ApiKeyManager _apiKeyManager;

        public RequireApiKeyFilter(ApiKeyManager apiKeyManager)
        {
            _apiKeyManager = apiKeyManager;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string apiKeyToTest = context.HttpContext.Request.Headers[ApiKeyHeaderName];
            if (string.IsNullOrEmpty(apiKeyToTest))
            {
                context.Result = new UnauthorizedObjectResult("This endpoint requires a API key");
                return;
            }

            if (!_apiKeyManager.TryAuthenticateFromApiKey(apiKeyToTest, out RoboUser user, out IdentityRoboUser identityUser))
            {
                context.Result = new UnauthorizedObjectResult("The provided API key was not valid");
                return;
            }

            context.RouteData.Values["apiKeyRoboUser"] = user;
            context.RouteData.Values["apiKeyIdentityUser"] = identityUser;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }

    public class RequireApiKey : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            ApiKeyManager apiKeyManager = (ApiKeyManager)serviceProvider.GetService(typeof(ApiKeyManager));
            return new RequireApiKeyFilter(apiKeyManager);
        }
    }
}