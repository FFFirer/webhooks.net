using Microsoft.AspNetCore.Mvc;

namespace WebHooks.API.ResultWrapper
{
    public interface IResultWrapper
    {
        IActionResult? WrapSuccessful(IActionResult? result);

        IActionResult? WrapSuccessfulEmpty();

        IActionResult WrapFailure(Exception exception);
    }
}
