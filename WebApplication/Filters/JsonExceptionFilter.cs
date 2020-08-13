using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using WebApplication.Helpers;

namespace WebApplication.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public IHostEnvironment Environment { get; }
        public JsonExceptionFilter(IHostEnvironment environment)
        {
            Environment = environment;
        }
        public void OnException(ExceptionContext context)
        {
            var error = new ApiError();
            if (Environment.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.ToString();
            }

            error.Message = "服务器错误";
            error.Detail = context.Exception.Message;

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
