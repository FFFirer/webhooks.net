using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;
using WebHooks.Shared.Paging;

namespace WebHooks.Service
{
    public class WorkService : IWorkService
    {
        private readonly IWorkRepository workRepo;

        public WorkService(IWorkRepository workRepository)
        {
            this.workRepo = workRepository;
        }

        public async Task<List<WorkDto>> ListAsync()
        {
            var datas = await workRepo.GetAll().AsNoTracking()
                .ToListAsync();

            var dtos = datas.Adapt<List<WorkDto>>();

            //datas.Adapt(dtos);

            return dtos;
        }

        public async Task<PagingResult<WorkDto>> PageAsync(PagingQuery pagingQuery)
        {
            var query = workRepo.GetAll().AsNoTracking();

            var result = new PagingResult<WorkDto>(pagingQuery);

            result.Total = await query.CountAsync();

            query = query.Paging(pagingQuery);

            var datas = await query.ToListAsync();

            result.Rows = datas.Adapt<List<WorkDto>>();

            return result;
        }

        public async Task RemoveAsync(Guid id)
        {
            await workRepo.RemoveAsync(id);
        }

        public async Task SaveAsync(WorkDto workDto)
        {
            var work = workDto.Adapt<Work>();

            if (work == null)
            {
                return;
            }

            if (work.Id == Guid.Empty)
            {
                var inserted = await workRepo.InsertAsync(work);
            }
            else
            {
                var updated = await workRepo.UpdateAsync(work);
            }
        }
    }
}
