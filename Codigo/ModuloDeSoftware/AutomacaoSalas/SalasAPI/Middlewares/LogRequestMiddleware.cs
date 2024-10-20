using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Model.ViewModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalasAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public LogRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogRequestService logRequestService)
        {

            var features = context.Features.Get<IHttpRequestFeature>();
            string? input;
            if (context.Request.HasFormContentType)
            {
                IFormCollection form;
                form = await context.Request.ReadFormAsync();

                input = string.Join(',', form.ToArray());
            }
            else
            {
                context.Request.EnableBuffering();
                var body = await new System.IO.StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
                input = string.IsNullOrEmpty(body) ? "" : "body{" + body + "}.";
            }
            var logRequest = new LogRequestModel
            {
                Date = DateTime.Now,
                Ip = context.Connection.RemoteIpAddress.ToString(),
                Url = $"{features?.Scheme}://{context.Request?.Host.Value}{features?.RawTarget}",
                Input = input,
                StatusCode = context.Response.StatusCode.ToString(),
                Origin = "API"
            };
            
            logRequestService.Insert(logRequest);

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LogRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogRequestMiddleware>();
        }
    }
}

