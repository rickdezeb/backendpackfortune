using Microsoft.AspNetCore.Http;
using Packfortune.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Packfortune.Logic
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Something unforeseen happened..";

            switch (ex)
            {
                case UserNotFoundException _:
                    message = ex.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case NegativePriceException _:
                    message = ex.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case NoNameException _:
                    message = ex.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case InvalidImageExtensionException _:
                    message = ex.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    break;
            }

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
            };

            string jsonResponse = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
