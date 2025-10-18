using Application.Exceptions;
using SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace DealersAndDistributors.Server.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = Result.Failure(Error.Problem("101", "Failed"));

                switch (error)
                {
                    case ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel = Result.Failure(Error.Problem(HttpStatusCode.BadRequest.ToString(), string.Join(",", e.Message)));
                        break;
                    case Application.Exceptions.ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel = Result.Failure(Error.Problem(HttpStatusCode.BadRequest.ToString(), string.Join(",", e.Errors)));
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel = Result.Failure(Error.Failure(HttpStatusCode.InternalServerError.ToString(), string.Empty));
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }

}
