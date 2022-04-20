using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using WebHooks.API.Models;
using WebHooks.API.ResultWrapper;
using WebHooks.Shared.CustomExceptions;

namespace WebHooks.API.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger logger;
        private readonly IResultWrapper wrapper;

        public ApiExceptionFilter(ILoggerFactory loggerFactory, IResultWrapper wrapper)
        {
            logger = loggerFactory.CreateLogger("exception-filter");
            this.wrapper = wrapper;
        }

        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            string error;
            bool isNonBusinessException = false;
            if(context.Exception is DataValidationException)
            {
                error = "数据校验异常";
            }
            else if(context.Exception is BusinessException)
            {
                error = "业务异常";
            }
            else
            {
                isNonBusinessException = true;
                error = "其他内部异常";
            }

            logger.LogError(context.Exception, error);

            context.Result = wrapper.WrapFailure(context.Exception);

            if (isNonBusinessException)
            {
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}
