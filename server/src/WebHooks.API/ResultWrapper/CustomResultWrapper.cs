using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models;
using WebHooks.Shared.CustomExceptions;

namespace WebHooks.API.ResultWrapper
{
    public class CustomResultWrapper : IResultWrapper
    {
        public IActionResult WrapFailure(Exception exception)
        {
            var apiResult = new ApiResult()
            {
                Success = false,
                Result = null
            };

            if(exception is DataValidationException)
            {
                apiResult.Error = $"数据校验错误: {exception.Message}";
            }
            else if(exception is BusinessException)
            {
                apiResult.Error = $"业务异常: {exception.Message}";
            }
            else
            {
                apiResult.Error = "服务器内部错误";
            }

            var result = new ObjectResult(apiResult);
            return result;
        }

        public IActionResult? WrapSuccessful(IActionResult? result)
        {
            if(result == null)
            {
                return result;
            }

            if(result is ObjectResult objectResult && objectResult.Value is not ApiResult)
            {
                var apiResult = new ApiResult()
                {
                    Success = true,
                    Error = null,
                    Result = objectResult.Value
                };

                return new ObjectResult(apiResult);
            }

            return result;
        }

        public IActionResult? WrapSuccessfulEmpty()
        {
            var emptyResult = new ApiResult()
            {
                Success = true,
                Error = null,
                Result = null,
            };

            return new ObjectResult(emptyResult);
        }
    }
}
