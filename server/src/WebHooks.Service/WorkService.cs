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
using WebHooks.Service.Exceptions;
using WebHooks.Service.Interfaces;
using WebHooks.Shared.Paging;

namespace WebHooks.Service
{
    public class WorkService : IWorkService
    {
        private readonly IWorkRepository workRepo;
        private readonly ISettingService _settings;

        public WorkService(IWorkRepository workRepository, ISettingService settingService)
        {
            this.workRepo = workRepository;
            this._settings = settingService;
        }

        public async Task<WorkDto?> GetAsync(Guid id)
        {
            var data = await workRepo.GetAsync(id);

            if (data == null) return null;

            return data.Adapt<WorkDto>();
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

        public async Task SaveAsync(WorkDto? workDto)
        {
            if(workDto == null)
            {
                return;
            }

            var work = workDto.Adapt<Work>();

            if (work == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(work.WorkingDirectory))
            {
                var baseSetting = await _settings.GetBasicSettingAsync();

                if(baseSetting.BaseWorkDirectory == null)
                {
                    throw new WorkRunningException($"请设置基础的工作目录");
                }
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
