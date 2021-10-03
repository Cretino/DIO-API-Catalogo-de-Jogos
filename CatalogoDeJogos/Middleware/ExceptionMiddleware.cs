using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext Context)
        {
            try
            {
                await Next(Context);
            }
            catch
            {
                await HandleExceptionAsync(Context);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext Context)
        {
            Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await Context.Response.WriteAsJsonAsync(new { Message = "Ocorreu um erro durante a solicitação, por favor, tente novamente mais tarde." });
        }
    }
}
