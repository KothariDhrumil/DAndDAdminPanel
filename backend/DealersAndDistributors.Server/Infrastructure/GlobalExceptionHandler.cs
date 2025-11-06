using Application.Exceptions;
using Azure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Net;

namespace DealersAndDistributors.Server.Infrastructure;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");
        var response = httpContext.Response;
        var responseModel = Result.Failure(Error.Problem("101", "Failed"));

        switch (exception)
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
                responseModel = Result.Failure(Error.Failure(HttpStatusCode.InternalServerError.ToString(), exception.Message));
                break;
        } 

        await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);

        return true;
    }
}
