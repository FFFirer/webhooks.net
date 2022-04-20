using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.API.Models;
using WebHooks.API.Models.Inputs;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;
using WebHooks.Shared.CustomExceptions;

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

        [HttpGet("[action]")]
        public async Task<List<GroupDto>> List()
        {
            var groups = await _groupService.ListAsync();
            return groups;
        }

        [HttpPost("[action]")]
        public async Task Save(GroupDto groupDto)
        {
            await _groupService.SaveAsync(groupDto);
        }

        [HttpPost("[action]")]
        public async Task Remove(RemoveGroupInput input)
        {
            await _groupService.RemoveAsync(input.Id);
        }
    }
}
