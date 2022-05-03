using Mapster;
using Microsoft.EntityFrameworkCore;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;
using WebHooks.Shared.Paging;

namespace WebHooks.Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepo;
        

        public GroupService(IGroupRepository groupRepository)
        {
            groupRepo = groupRepository;
        }

        public async Task<List<GroupDto>> ListAsync()
        {
            var datas = await groupRepo.GetAll().AsNoTracking().ToListAsync();

            var dtos = datas.Adapt<List<GroupDto>>();

            datas.Adapt(dtos);

            return dtos;
        }

        public async Task<PagingResult<GroupDto>> PageAsync(PagingQuery pagingQuery)
        {
            var query = groupRepo.GetAll().AsNoTracking();

            var result = new PagingResult<GroupDto>(pagingQuery);

            result.Total = await query.CountAsync();

            query = query.Paging(pagingQuery);

            var datas = await query.ToListAsync();

            result.Rows = datas.Adapt<List<GroupDto>>();

            return result;
        }

        public async Task RemoveAsync(Guid id)
        {
            var group = await groupRepo.GetAsync(id);
            if(group != null)
            {
                groupRepo.Set().Remove(group);
                await groupRepo.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(GroupDto group)
        {
            var current = await groupRepo.GetAsync(group.Id);

            if(current == null)
            {
                if(group.Id == Guid.Empty)
                {
                    current = group.Adapt<Group>();
                    group.Adapt(current);
                    // 新增
                    groupRepo.Set().Add(current);
                }
            }
            else
            {
                // 更新
                current.Name = group.Name;
                current.Description = group.Description;
            }

            await groupRepo.SaveChangesAsync();
        }
    }
}
