using Application.Common.HeaderKey;
using Application.Configs.Admin;
using Microsoft.Extensions.Options;

namespace Api.Filters;

public class AdminHeaderKeyFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var config = context.HttpContext.RequestServices
            .GetRequiredService<IOptions<AdminSettings>>().Value;

        var headerName = HeaderExtensions.ToKebabCaseHeaderName(nameof(config.XAdminKey));

        var hasHeader = context.HttpContext.Request.Headers
            .TryGetValue(headerName, out var headerValue);
        if (!hasHeader) return Results.Forbid();   

        if (headerValue != config.XAdminKey) 
            return Results.Forbid();

        return await next(context);
    }
}
