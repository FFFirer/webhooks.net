using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using WebHooks.API.Models;
using WebHooks.API.ResultWrapper;
using WebHooks.Shared.CustomExceptions;

namespace WebHooks.API
{
    /// <summary>
    /// 请求结果包装
    /// </summary>
    public class ResultWrapperFilter : IActionFilter
    {
        private readonly IResultWrapper _wrapper;

        public ResultWrapperFilter(IResultWrapper resultWrapper)
        {
            _wrapper = resultWrapper;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Result is ObjectResult)
            {
                context.Result = _wrapper.WrapSuccessful(context.Result);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
