using Microsoft.EntityFrameworkCore;
using Admin.NETCore.Common.Configs;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB;
using Admin.NETCore.Infrastructure.DB.Entities;


namespace Admin.NETCore.Core.Services
{
    public class DictTypeService : IDictTypeService
    {
        private readonly AppDbContext _context;


        public DictTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<DictTypeVModel>> CreateOrUpdateDictTypeAsync(DictTypeVModel model)
        {
            var result = new ApiResult<DictTypeVModel>();

            bool isNew = string.IsNullOrWhiteSpace(model.Id);

            // name 或 code 重复校验（排除当前记录的 Id）
            var exists = await _context.DictType
                .Where(m => m.Id != model.Id && (m.Name == model.Name || m.Code == model.Code))
                .ToListAsync();

            if (exists.Any(m => m.Name == model.Name))
                return result.Fail("Name已存在");

            if (exists.Any(m => m.Code == model.Code))
                return result.Fail("Code已存在");

            // 编辑
            if (!isNew)
            {
                var existModel = await _context.DictType.FindAsync(model.Id);
                if (existModel == null)
                    return result.Fail("字典类型不存在");

                existModel.Name = model.Name;
                existModel.Code = model.Code;
                existModel.Sort = model.Sort;
                existModel.Status = model.Status;
                existModel.Builtin = model.Builtin;
                existModel.Notes = model.Notes;
                existModel.IsDelete = false;

                await _context.SaveChangesAsync();

                return result.Success(model, "字典类型更新成功");
            }
            else
            {
                // 新增
                var dbModel = new DictType
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Code = model.Code,
                    Sort = model.Sort,
                    Status = model.Status,
                    Builtin = model.Builtin,
                    Notes = model.Notes,
                    IsDelete = false
                };
                await _context.DictType.AddAsync(dbModel);
                await _context.SaveChangesAsync();

                model.Id = dbModel.Id;
                return result.Success(model, "字典类型创建成功");
            }
        }

        public async Task<ApiResult<string>> DeleteDictTypeByIdsAsync(List<string> ids)
        {
            var result = new ApiResult<string>();

            // 查找对应的记录
            var exists = await _context.DictType
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            // 不管传入的 ids 是否都存在，只要能匹配的就删除, 不会报错，也不会终止操作
            if (!exists.Any())
            {
                return result.Fail("未找到任何要删除的字典类型");
            }

            //// 如果找到的id和传入的不一致，不删除 （只要有一个 ID 不存在，就全部不删）
            //if (exists.Count != ids.Count)
            //{
            //    return result.Fail("部分字典类型不存在，操作终止");
            //}

            _context.DictType.RemoveRange(exists); // 物理删除
            //foreach (var item in exists)  // 逻辑删除
            //{
            //    item.IsDelete = true;
            //}
            await _context.SaveChangesAsync();

            return result.Success("", "字典类型删除成功");
        }


        public async Task<ApiResult<string>> DeleteDictTypeByIdAsync(string id)
        {
            var ids = new List<string> { id };
            return await DeleteDictTypeByIdsAsync(ids);
        }

        public async Task<ApiResult<DictTypeVModel>> GetDictTypeByIdAsync(string id)
        {
            var result = new ApiResult<DictTypeVModel>();

            var existModel = await _context.DictType.FindAsync(id);
            if (existModel == null)
            {
                return result.Fail("字典类型不存在");
            }
            var returnModel = new DictTypeVModel
            {
                Id = existModel.Id,
                Name = existModel.Name,
                Code = existModel.Code,
                Sort = existModel.Sort,
                Status = existModel.Status,
                Builtin = existModel.Builtin,
                Notes = existModel.Notes,
                IsDelete = existModel.IsDelete,
            };
            return result.Success(returnModel);
        }

        public async Task<PagedResult<DictTypeListDTO>> GetDictTypeListAsync(DictTypeFilterModel filter)
        {
            // 参数校验
            filter.Page = filter.Page > 0 ? filter.Page : 1;
            filter.Limit = filter.Limit > 0 ? filter.Limit : GlobalConfigs.DefaultPageSize;

            var query = _context.DictType.AsNoTracking().AsQueryable();
            /*
                 EF Core 默认会追踪查询出来的实体（用于之后的更新或删除）
                 如果只是读取数据，无需修改，可以用 .AsNoTracking() 提高性能
                 它会 减少内存开销 和 避免不必要的跟踪逻辑
             */

            //var query = _context.DictType.Where(m => m.IsDelete == false).AsNoTracking().AsQueryable();
            //var query = _context.DictType.Where(m => !m.IsDelete).AsNoTracking();

            // name/code模糊查询
            if (!string.IsNullOrEmpty(filter.Keyword))
                query = query.Where(m => m.Name.Contains(filter.Keyword) || m.Code.Contains(filter.Keyword));

            // 获取总数
            int total = await query.CountAsync();

            // 分页查询
            var list = await query
                .OrderByDescending(m => m.UpdateDate)
                .Skip((filter.Page - 1) * filter.Limit)
                .Take(filter.Limit)
                .AsSplitQuery()
                .Select(m => new DictTypeListDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Code = m.Code,
                    Sort = m.Sort,
                    Status = m.Status,
                    Builtin = m.Builtin,
                    Notes = m.Notes,
                    IsDelete = m.IsDelete,
                })
                .ToListAsync();
            return PagedResult<DictTypeListDTO>.SuccessResult(list, total);
        }

        // 查询多个字典类型及该类型所对应的字典项，查询结果根据sort升序
        public async Task<ApiResult<DictTypesAndItemsDTO>> GetDictTypesAndItemsAsync(string codes)
        {
            var result = new ApiResult<DictTypesAndItemsDTO>();

            if (string.IsNullOrWhiteSpace(codes))
            {
                return result.Fail("codes不能为空");
            }

            // 分割并去重
            var codeList = codes.Split(',', StringSplitOptions.RemoveEmptyEntries) // 返回数组元素移除空字符串元素（不包含空字符串元素）
                                   .Select(c => c.Trim())
                                   .Distinct()
                                   .ToList();

            if (!codeList.Any())
                return result.Fail("codes不能为空");

            // 查询所有包含这些code的字典项(字典项必须是未删除的)
            var dictItems = await _context.DictItem
                .AsNoTracking()
                .Where(item => codeList.Contains(item.DictTypeCode) && !item.IsDelete)
                .OrderBy(item => item.Sort) // 按 Sort 升序
                .ToListAsync();

            // 按 DictTypeCode 分组并处理成返回的格式
            var dictResult = dictItems
                .GroupBy(item => item.DictTypeCode)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(item => new DictItemSimpleDto
                    {
                        Id = item.Id,
                        Label = item.Label,
                        Value = item.Value
                    }).ToList()
                );

            // 转换为DTO类型
            var dict = new DictTypesAndItemsDTO();
            foreach (var item in dictResult)
            {
                dict[item.Key] = item.Value;
            }


            //var dict = new DictTypesAndItemsDTO();
            //foreach (var group in dictItems.GroupBy(item => item.DictTypeCode))
            //{
            //    dict[group.Key] = group.Select(item => new DictItemSimpleDto
            //    {
            //        Id = item.Id,
            //        Label = item.Label,
            //        Value = item.Value
            //    }).ToList();
            //}

            return result.Success(dict);

        }


        public async Task<ApiResult<Dictionary<string, List<DictItemSimpleDto>>>> GetDictTypesAndItems2Async(string codes)
        {
            var result = new ApiResult<Dictionary<string, List<DictItemSimpleDto>>>();

            if (string.IsNullOrWhiteSpace(codes))
            {
                return result.Fail("codes不能为空");
            }

            // 分割并去重
            var codeList = codes.Split(',', StringSplitOptions.RemoveEmptyEntries) // 返回数组元素移除空字符串元素（不包含空字符串元素）
                                   .Select(c => c.Trim())
                                   .Distinct()
                                   .ToList();

            if (!codeList.Any())
                return result.Fail("codes不能为空");

            // 查询所有包含这些code的字典项(字典项必须是未删除的)
            var dictItems = await _context.DictItem
                .AsNoTracking()
                .Where(item => codeList.Contains(item.DictTypeCode) && !item.IsDelete)
                .OrderBy(item => item.Sort) // 按 Sort 升序
                .ToListAsync();

            // 按 DictTypeCode 分组并处理成返回的格式
            var dict = dictItems
                .GroupBy(item => item.DictTypeCode)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(item => new DictItemSimpleDto
                    {
                        Id = item.Id,
                        Label = item.Label,
                        Value = item.Value
                    }).ToList()
                );

            return result.Success(dict);
        }

    }
}