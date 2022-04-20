using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : BasicController
    {
        private readonly IGroupService _groupService;
        
        public GroupController(IGroupService groupService)
        {
            this._groupService = groupService;
        }

        [HttpGet]
        public async Task<List<GroupDto>> List()
        {
            var groups = await _groupService.ListAsync();
            return groups;
        }
    }
}
