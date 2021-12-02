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
        
        private UserRoles _requiredRoles;

        public RequireApiKeyFilter(ApiKeyManager apiKeyManager, UserRoles requiredRoles)
        {
            _apiKeyManager = apiKeyManager;
            _requiredRoles = requiredRoles;
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
            
            if(_requiredRoles != UserRoles.None && !Validator.UserHasRequiredRoles(identityUser, _requiredRoles))
                context.Result = new UnauthorizedObjectResult("You don't have the required roles to execute this action");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }

    public class RequireApiKey : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        private UserRoles _requiredRoles;

        public RequireApiKey(UserRoles requiredRoles = UserRoles.User)
        {
            _requiredRoles = requiredRoles;
        }
        
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            ApiKeyManager apiKeyManager = (ApiKeyManager)serviceProvider.GetService(typeof(ApiKeyManager));
            return new RequireApiKeyFilter(apiKeyManager, _requiredRoles);
        }
    }
}