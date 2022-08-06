using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Model.ViewModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalasUfsWeb.Middlewares
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
            var input = "";
            if (context.Request.HasFormContentType)
            {
                IFormCollection form;
                form = context.Request.Form; // sync
                                             // Or
                form = await context.Request.ReadFormAsync(); // async

                input = String.Join(',', form.ToArray());
            } else
            {
                String.Join(',', context.Request?.Query.ToArray());
            }
            var logRequest = new LogRequestModel
            {
                Date = DateTime.Now,
                Ip = context.Connection?.RemoteIpAddress?.ToString(),
                Url = $"{features?.Scheme}://{context.Request?.Host.Value}{features.RawTarget}",
                Input = input,
                StatusCode = context.Response?.StatusCode.ToString()
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
