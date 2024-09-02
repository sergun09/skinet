﻿using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.RequestHelpers;

[AttributeUsage(AttributeTargets.All)]
public class InvalidateCache(string pattern) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if(resultContext.Exception is null || resultContext.ExceptionHandled) 
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            await cacheService.RemoveCacheByPattern(pattern);
        }
    }
}
