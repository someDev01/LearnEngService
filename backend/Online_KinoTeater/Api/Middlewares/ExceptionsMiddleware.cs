using System.Net.Mail;

namespace Api.Middlewares;

public class ExceptionsMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(BadHttpRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Invalid Request format",
                message = "Ошибка, передан неккоректный формат данных"
            });
        }
        catch (SmtpException)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Mail unavailable",
                message = "Почта не доступна или была удалена"
            }); 
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Unauthorized",
                message = ex.Message
            });
        }
    }
}
