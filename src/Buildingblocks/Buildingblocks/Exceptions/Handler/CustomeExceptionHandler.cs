using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Buildingblocks.Exceptions.Handler
{
    public class CustomeExceptionHandler(ILogger<CustomeExceptionHandler> logger, IProblemDetailsService problemDetailsService , IHostEnvironment environment) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            // log the exception
            logger.LogError(exception, "An unhandled exception occurred , traceId {TraceId}",httpContext.TraceIdentifier);

            var elapsedMs = Activity.Current is not null ? (long) Activity.Current.Duration.TotalMilliseconds : 0;
            // map exception 
            var (statusCode, title) = MapException(exception);


            // prepare response to make response are standared in .net core 
            httpContext.Response.StatusCode = statusCode;

            // prepare problemdetails 
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Instance = httpContext.Request.Path,
                Type = Getproblemtype(statusCode),
                Detail = GetSafeErrorMessage(exception, httpContext),

            };

            problemDetails.Extensions["TraceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["TimeStamp"] = DateTime.UtcNow;
            problemDetails.Extensions["duration"] = $"{elapsedMs}ms";

            if (exception is BadRequestException badRequestEx && badRequestEx.Errors is not null)
            {
                problemDetails.Extensions["Errors"] = badRequestEx.Errors;
            }
            else if (exception is FluentValidation.ValidationException fluentEx)
            {
                problemDetails.Extensions["Errors"] = fluentEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
            }
            else
            {
                problemDetails.Extensions["Errors"] = new object();
            }

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext { ProblemDetails = problemDetails, HttpContext = httpContext, });
        }

        private static (int statuescode, string tiltle) MapException(Exception exception) => exception switch
        {
            AppException appex => ((int)(appex.HttpStatusCode), appex.Message),
            ArgumentNullException appex => ((int)HttpStatusCode.BadRequest, "Invalid argument null exception"),
            ArgumentException appex => ((int)HttpStatusCode.BadRequest, "Invalid argument exception"),
            UnauthorizedAccessException appex => ((int)HttpStatusCode.Unauthorized, "UnAuthorized exception"),
            _ => ((int)HttpStatusCode.InternalServerError, "Unknown handle exception")
        };
        private static string Getproblemtype(int statuscode) => statuscode switch
        {
            400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        };

        private static string GetSafeErrorMessage(Exception exception, HttpContext  context)
        {
            var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
            if (exception is AppException )
            {
                return exception.Message;
            }
            if (env.IsDevelopment())
            {
                return exception.ToString();
            }
            return exception is AppException ? exception.Message: "An error occurred while processing your request.";
        }
    }

    

}
