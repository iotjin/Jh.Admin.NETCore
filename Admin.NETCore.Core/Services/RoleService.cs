using Microsoft.EntityFrameworkCore;
using Admin.NETCore.Common.Configs;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB;
using Admin.NETCore.Infrastructure.DB.Entities;


namespace Admin.NETCore.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;


        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<RoleVModel>> CreateRoleAsync(RoleVModel model)
        {
            //var result = new ApiResult<RoleVModel>();

            // 检查name、code是否重复（name或code任意一个都不能重复）
            //if (await _context.Role.AnyAsync(m => m.Name == model.Name))
            //{
            //    return ApiResult<RoleVModel>.FailResult("Name已存在");
            //}
            //if (await _context.Role.AnyAsync(m => m.Code == model.Code))
            //{
            //    return ApiResult<RoleVModel>.FailResult("Code已存在");
            //}

            var exists = await _context.Role
                .Where(m => m.Name == model.Name || m.Code == model.Code)
                .ToListAsync();

            if (exists.Any(m => m.Name == model.Name))
                return ApiResult<RoleVModel>.FailResult("Name已存在");

            if (exists.Any(m => m.Code == model.Code))
                return ApiResult<RoleVModel>.FailResult("Code已存在");

            var dbModel = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Code = model.Code,
                Status = model.Status,
                Builtin = model.Builtin,
                Notes = model.Notes,
                IsDelete = false
            };
            await _context.Role.AddAsync(dbModel);
            await _context.SaveChangesAsync();

            model.Id = dbModel.Id;
            return ApiResult<RoleVModel>.SuccessResult(model, "角色创建成功");
        }

        public async Task<ApiResult<RoleVModel>> UpdateRoleAsync(RoleVModel model)
        {
            var existModel = await _context.Role.FindAsync(model.Id);
            if (existModel == null)
            {
                return ApiResult<RoleVModel>.FailResult("角色不存在");
            }
            // 检查name、code是否与其他角色重复
            if (await _context.Role.AnyAsync(m => m.Name == model.Name && m.Id != model.Id))
            {
                return ApiResult<RoleVModel>.FailResult("Name已存在");
            }
            if (await _context.Role.AnyAsync(m => m.Code == model.Code && m.Id != model.Id))
            {
                return ApiResult<RoleVModel>.FailResult("Code已存在");
            }

            existModel.Name = model.Name;
            existModel.Code = model.Code;
            existModel.Status = model.Status;
            existModel.Builtin = model.Builtin;
            existModel.Notes = model.Notes;
            existModel.IsDelete = false;
            await _context.SaveChangesAsync();

            return ApiResult<RoleVModel>.SuccessResult(model, "角色更新成功");
        }

        public async Task<ApiResult<string>> DeleteRoleByIdAsync(string id)
        {
            var existModel = await _context.Role
                .Include(m => m.UserRoles)  // 不加Include会导致UserRoles为null
                .FirstOrDefaultAsync(m => m.Id == id);

            //var existModel = await _context.Role.FindAsync(id);
            if (existModel == null)
            {
                return ApiResult<string>.FailResult("角色不存在");
            }
            // 内置角色不能删除
            if (existModel.Builtin == 1)
            {
                return ApiResult<string>.FailResult("内置角色不能删除");
            }
            if (existModel.UserRoles.Count > 0)
            {
                return ApiResult<string>.FailResult("已经分配用户的角色不能删除");
            }

            _context.Role.Remove(existModel); // 物理删除
            // existModel.IsDelete = true; // 逻辑删除
            await _context.SaveChangesAsync();
            return ApiResult<string>.SuccessResult("", "角色删除成功");
        }

        public async Task<ApiResult<RoleVModel>> GetRoleByIdAsync(string id)
        {
            var existModel = await _context.Role.FindAsync(id);
            if (existModel == null)
            {
                return ApiResult<RoleVModel>.FailResult("角色不存在");
            }
            var returnModel = new RoleVModel
            {
                Id = existModel.Id,
                Name = existModel.Name,
                Code = existModel.Code,
                Status = existModel.Status,
                Builtin = existModel.Builtin,
                Notes = existModel.Notes,
                IsDelete = existModel.IsDelete
            };
            return ApiResult<RoleVModel>.SuccessResult(returnModel);
        }

        public async Task<PagedResult<RoleListDTO>> GetRoleListAsync(RoleFilterModel filter)
        {
            // 参数校验
            filter.Page = filter.Page > 0 ? filter.Page : 1;
            filter.Limit = filter.Limit > 0 ? filter.Limit : GlobalConfigs.DefaultPageSize;

            var query = _context.Role.AsNoTracking().AsQueryable();
            /*
                 EF Core 默认会追踪查询出来的实体（用于之后的更新或删除）
                 如果只是读取数据，无需修改，可以用 .AsNoTracking() 提高性能
                 它会 减少内存开销 和 避免不必要的跟踪逻辑
             */

            //var query = _context.Role.Where(m => m.IsDelete == false).AsNoTracking().AsQueryable();
            //var query = _context.Role.Where(m => !m.IsDelete).AsNoTracking();

            // name模糊查询
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(m => EF.Functions.Like(m.Name, $"%{filter.Name}%"));

            // code模糊查询
            if (!string.IsNullOrEmpty(filter.Code))
                query = query.Where(m => m.Code.Contains(filter.Code));

            // 获取总数
            int total = await query.CountAsync();

            // 分页查询
            var list = await query
                .OrderByDescending(m => m.UpdateDate)
                .Skip((filter.Page - 1) * filter.Limit)
                .Take(filter.Limit)
                .AsSplitQuery()
                .Select(m => new RoleListDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Code = m.Code,
                    Status = m.Status,
                    Builtin = m.Builtin,
                    Notes = m.Notes,
                })
                .ToListAsync();
            return PagedResult<RoleListDTO>.SuccessResult(list, total);
        }


        public async Task<ApiResult<List<RoleListDTO>>> GetRoleListByUserIdAsync(UserRolesVModel filter)
        {
            var user = await _context.User.FindAsync(filter.UserId);
            if (user == null)
            {
                return ApiResult<List<RoleListDTO>>.FailResult("用户不存在");
            }

            // 查已分配的角色列表
            //var roles = await _context.UserRole
            //    .Where(m => m.UserId == filter.UserId)
            //    .Select(m => m.Role)
            //    .ToListAsync();

            var query = _context.Role.AsNoTracking().Where(m => !m.IsDelete);

            // name模糊查询
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(m => m.Name.Contains(filter.Name));
            }

            // 获取用户已分配的角色ID列表
            var assignedRoleIds = await _context.UserRole
                .AsNoTracking()
                .Where(m => m.UserId == filter.UserId)
                .Select(m => m.RoleId)
                .ToListAsync();

            bool isAssigned = filter.AssignStatus == "1";
            query = query.Where(m => isAssigned == assignedRoleIds.Contains(m.Id));

            var list = await query
                 .Select(m => new RoleListDTO
                 {
                     Id = m.Id,
                     Name = m.Name,
                     Code = m.Code,
                     Status = m.Status,
                     Builtin = m.Builtin,
                     Notes = m.Notes,
                 })
                .ToListAsync();
            return ApiResult<List<RoleListDTO>>.SuccessResult(list);
        }
    }
}