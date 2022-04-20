using Microsoft.AspNetCore.Mvc;

namespace WebHooks.API.ResultWrapper
{
    public interface IResultWrapper
    {
        IActionResult? WrapSuccessful(IActionResult? result);

        IActionResult WrapFailure(Exception exception);
    }
}
