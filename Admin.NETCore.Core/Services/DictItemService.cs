using Microsoft.EntityFrameworkCore;
using Admin.NETCore.Common.Configs;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB;
using Admin.NETCore.Infrastructure.DB.Entities;


namespace Admin.NETCore.Core.Services
{
    public class DictItemService : IDictItemService
    {
        private readonly AppDbContext _context;


        public DictItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<DictItemVModel>> CreateOrUpdateDictItemAsync(DictItemVModel model)
        {
            // 先判断字典类型是否存在
            bool dictTypeExists = await _context.DictType.AnyAsync(m => m.Code == model.DictTypeCode);
            if (!dictTypeExists)
            {
                return ApiResult<DictItemVModel>.FailResult("字典类型不存在");
            }

            bool isNew = string.IsNullOrWhiteSpace(model.Id);

            // label 或 value 重复校验（排除当前记录的 Id）
            var exists = await _context.DictItem
                .Where(m => m.Id != model.Id && (m.Label == model.Label || m.Value == model.Value))
                .ToListAsync();

            if (exists.Any(m => m.Label == model.Label))
                return ApiResult<DictItemVModel>.FailResult("Label已存在");

            if (exists.Any(m => m.Value == model.Value))
                return ApiResult<DictItemVModel>.FailResult("Value已存在");

            // 编辑
            if (!isNew)
            {
                var existModel = await _context.DictItem.FindAsync(model.Id);
                if (existModel == null)
                    return ApiResult<DictItemVModel>.FailResult("字典项不存在");

                existModel.Label = model.Label;
                existModel.Value = model.Value;
                existModel.DictTypeCode = model.DictTypeCode;
                existModel.Sort = model.Sort;
                existModel.Status = model.Status;
                existModel.Builtin = model.Builtin;
                existModel.Notes = model.Notes;
                existModel.IsDelete = false;

                await _context.SaveChangesAsync();

                return ApiResult<DictItemVModel>.SuccessResult(model, "字典项更新成功");
            }
            else
            {
                // 新增
                var dbModel = new DictItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Label = model.Label,
                    Value = model.Value,
                    DictTypeCode = model.DictTypeCode,
                    Sort = model.Sort,
                    Status = model.Status,
                    Builtin = model.Builtin,
                    Notes = model.Notes,
                    IsDelete = false
                };
                await _context.DictItem.AddAsync(dbModel);
                await _context.SaveChangesAsync();

                model.Id = dbModel.Id;
                return ApiResult<DictItemVModel>.SuccessResult(model, "字典项创建成功");
            }
        }

        public async Task<ApiResult<string>> DeleteDictItemByIdsAsync(List<string> ids)
        {
            // 查找对应的记录
            var exists = await _context.DictItem
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            // 不管传入的 ids 是否都存在，只要能匹配的就删除, 不会报错，也不会终止操作
            if (!exists.Any())
            {
                return ApiResult<string>.FailResult("未找到任何要删除的字典项");
            }

            //// 如果找到的id和传入的不一致，不删除 （只要有一个 ID 不存在，就全部不删）
            //if (exists.Count != ids.Count)
            //{
            //    return ApiResult<string>.FailResult("部分字典项不存在，操作终止");
            //}

            _context.DictItem.RemoveRange(exists); // 物理删除
            //foreach (var item in exists)  // 逻辑删除
            //{
            //    item.IsDelete = true;
            //}
            await _context.SaveChangesAsync();

            return ApiResult<string>.SuccessResult("", "字典项删除成功");
        }


        public async Task<ApiResult<string>> DeleteDictItemByIdAsync(string id)
        {
            var ids = new List<string> { id };
            return await DeleteDictItemByIdsAsync(ids);
        }

        public async Task<ApiResult<DictItemVModel>> GetDictItemByIdAsync(string id)
        {
            var existModel = await _context.DictItem.FindAsync(id);
            if (existModel == null)
            {
                return ApiResult<DictItemVModel>.FailResult("字典项不存在");
            }
            var returnModel = new DictItemVModel
            {
                Id = existModel.Id,
                Label = existModel.Label,
                Value = existModel.Value,
                DictTypeCode = existModel.DictTypeCode,
                Sort = existModel.Sort,
                Status = existModel.Status,
                Builtin = existModel.Builtin,
                Notes = existModel.Notes,
                IsDelete = existModel.IsDelete,
            };
            return ApiResult<DictItemVModel>.SuccessResult(returnModel);
        }

        public async Task<PagedResult<DictItemListDTO>> GetDictItemListAsync(DictItemFilterModel filter)
        {
            // 参数校验
            filter.Page = filter.Page > 0 ? filter.Page : 1;
            filter.Limit = filter.Limit > 0 ? filter.Limit : GlobalConfigs.DefaultPageSize;

            var query = _context.DictItem.AsNoTracking().AsQueryable();
            /*
                 EF Core 默认会追踪查询出来的实体（用于之后的更新或删除）
                 如果只是读取数据，无需修改，可以用 .AsNoTracking() 提高性能
                 它会 减少内存开销 和 避免不必要的跟踪逻辑
             */

            //var query = _context.DictItem.Where(m => m.IsDelete == false).AsNoTracking().AsQueryable();
            //var query = _context.DictItem.Where(m => !m.IsDelete).AsNoTracking();

            // label/value模糊查询
            if (!string.IsNullOrEmpty(filter.Keyword))
                query = query.Where(m => m.Label.Contains(filter.Keyword) || m.Value.Contains(filter.Keyword));

            // 获取总数
            int total = await query.CountAsync();

            // 分页查询
            var list = await query
                .OrderByDescending(m => m.UpdateDate)
                .Skip((filter.Page - 1) * filter.Limit)
                .Take(filter.Limit)
                .AsSplitQuery()
                .Select(m => new DictItemListDTO
                {
                    Id = m.Id,
                    Label = m.Label,
                    Value = m.Value,
                    DictTypeCode = m.DictTypeCode,
                    Sort = m.Sort,
                    Status = m.Status,
                    Builtin = m.Builtin,
                    Notes = m.Notes,
                    IsDelete = m.IsDelete,
                })
                .ToListAsync();
            return PagedResult<DictItemListDTO>.SuccessResult(list, total);
        }

        public async Task<ApiResult<List<DictItemListDTO>>> GetDictItemListByCodeAsync(string code)
        {
            var query = _context.DictItem.Where(m => m.DictTypeCode == code).AsNoTracking().AsQueryable();

            var list = await query
                //.OrderBy(item => item.Sort) // 按 Sort 升序
                .OrderByDescending(m => m.Sort) // 按 Sort 降序
                .Select(m => new DictItemListDTO
                {
                    Id = m.Id,
                    Label = m.Label,
                    Value = m.Value,
                    DictTypeCode = m.DictTypeCode,
                    Sort = m.Sort,
                    Status = m.Status,
                    Builtin = m.Builtin,
                    Notes = m.Notes,
                    IsDelete = m.IsDelete,
                })
                .ToListAsync();

            return ApiResult<List<DictItemListDTO>>.SuccessResult(list);

        }
    }
}