using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;

namespace WebHooks.UI.Pages.Group
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IGroupService _service;
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public GroupDto? Group { get; set; }

        public IndexModel(IGroupService service)
        {
            _service = service;
        }

        public async Task OnGet()
        {
            this.Groups = await _service.ListAsync();
        }

        public async Task<IActionResult> OnPostCreateGroup(GroupDto group)
        {
            await _service.SaveAsync(group);

            return new JsonResult(null);
        }

        public async Task OnPostDeleteGroup(Guid guid)
        {
            await _service.RemoveAsync(guid);
        }
    }
}
