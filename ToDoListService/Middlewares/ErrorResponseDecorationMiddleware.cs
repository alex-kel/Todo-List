using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoListService.DTOs;

namespace ToDoListService.Middlewares;

public class ErrorResponseDecorationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using var memoryStream = new MemoryStream();
        var bodyStream = context.Response.Body;
        context.Response.Body = memoryStream;

        await next(context);

        memoryStream.Seek(0, SeekOrigin.Begin);
        if (!IsSuccessStatusCode(context.Response.StatusCode))
        {
            using var streamReader = new StreamReader(memoryStream);
            var responseBody = await streamReader.ReadToEndAsync();
            var decoratedResponseBody = GetDecoratedResponseBody(context.Response.StatusCode, responseBody);

            using var newStream = new MemoryStream();
            var sw = new StreamWriter(newStream);
            await sw.WriteAsync(decoratedResponseBody);
            await sw.FlushAsync();
            newStream.Seek(0, SeekOrigin.Begin);
            await newStream.CopyToAsync(bodyStream);
        }
        else
        {
            await memoryStream.CopyToAsync(bodyStream);
        }
    }

    private string GetDecoratedResponseBody(int httpStatusCode, string responseBody)
    {
        try
        {
            var json = JObject.Parse(responseBody);
            json.Remove("status");
            json.Remove("type");

            var errorResponse = new CommonErrorResponseDto<object>
            {
                StatusCode = httpStatusCode,
                Status = ((HttpStatusCode)httpStatusCode).ToString(),
                FailureDetails = json.ToObject<object>()
            };
            return JObject.FromObject(errorResponse).ToString();
        }
        catch (JsonReaderException e)
        {
            // Provided response body is not json and should be handled as plain text
            var errorResponse = new CommonErrorResponseDto<string>
            {
                StatusCode = httpStatusCode,
                Status = ((HttpStatusCode)httpStatusCode).ToString(),
                FailureDetails = responseBody
            };
            return JObject.FromObject(errorResponse).ToString();
        }
        
    }
    
    private static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode is >= 200 and <= 299;
    }
}

public static class ErrorResponseDecorationMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorResponseDecoration(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorResponseDecorationMiddleware>();
    }
}