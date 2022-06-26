using Microsoft.EntityFrameworkCore;
using WebHooks.Data.DbContexts;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;

namespace WebHooks.Data.Repositories
{
    public class SettingRepository : Repository<Setting, Guid>, ISettingRepository
    {
        public SettingRepository(WebHooksDataContext context) : base(context)
        {

        }

        public async Task<List<Setting>> GetListAsync(string section, params string[] subsections)
        {
            var query = this.Set().AsNoTracking().Where(a => a.Section == section);

            if (subsections.Any())
            {
                query = query.Where(a => subsections.Contains(a.SubSection));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Setting>> GetOneAsync(string section)
        {
            return await this.Set().AsNoTracking()
                                         .Where(a => a.Section == section)
                                         .ToListAsync();
        }

        public async Task<long> SaveListAsync(List<Setting> settings)
        {
            var toAdd = settings.Where(a => a.Id == Guid.Empty).Select(a => { a.CreatedAt = DateTime.UtcNow; return a; });
            var toUpdate = settings.Where(a => a.Id != Guid.Empty);

            var olds = await this.Set().Where(a => toUpdate.Select(b => b.Id).Contains(a.Id))
                    .ToListAsync();

            var oldHasUpdated = olds.Join(toUpdate, newone => newone.Id, oldone => oldone.Id, (oldone, newone) =>
            {
                oldone.Value = newone.Value;
                oldone.Description = newone.Description;
                oldone.ModifedAt = DateTime.UtcNow;
                return oldone;
            }).ToList();

            await this.Set().AddRangeAsync(toAdd.ToList());

            return await SaveChangesAsync();
        }

        public async Task<long> SaveOrDeleteAsync(List<Setting> toSave, List<Setting> toDelete)
        {
            // Save
            if (toSave.Any())
            {
                var toAdd = toSave.Where(a => a.Id == Guid.Empty).Select(a => { a.CreatedAt = DateTime.UtcNow; return a; });
                var toUpdate = toSave.Where(a => a.Id != Guid.Empty).Select(a => { a.ModifedAt = DateTime.UtcNow; return a; });

                var inDB = await this.Set().Where(a => toUpdate.Select(b => b.Id).Contains(a.Id))
                    .ToListAsync();

                var hasUpdated = inDB.Join(toUpdate, newone => newone.Id, oldone => oldone.Id, (oldone, newone) =>
                {
                    oldone.Value = newone.Value;
                    oldone.Description = newone.Description;
                    return oldone;
                }).ToList();

                await this.Set().AddRangeAsync(toAdd.ToList());

            }


            // Delete
            if (toDelete.Any())
            {
                var inDB = this.Set().Where(a => toDelete.Select(b => b.Id).Contains(a.Id));

                this.Set().RemoveRange(inDB);

            }

            return await SaveChangesAsync();
        }
    }
}
