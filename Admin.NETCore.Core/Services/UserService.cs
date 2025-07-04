using Microsoft.EntityFrameworkCore;
using Admin.NETCore.Common.Configs;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB;
using Admin.NETCore.Infrastructure.DB.Entities;

namespace Admin.NETCore.Core.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;


        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<UserVModel>> CreateUserAsync(UserVModel model)
        {
            //var result = new ApiResult<UserVModel>();

            // 检查用户名是否已存在
            if (await _context.User.AnyAsync(m => m.LoginName == model.LoginName))
            {
                return ApiResult<UserVModel>.FailResult("登录名已存在");
            }

            var dbModel = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                LoginName = model.LoginName,
                Phone = model.Phone,
                UserNumber = model.UserNumber,
                DeptId = model.DeptId,
                UserExpiryDate = model.UserExpiryDate,
                Status = model.Status,
                Level = model.Level,
                Money = model.Money,
                Age = model.Age,
                Notes = model.Notes,
                IsDelete = false
            };
            await _context.User.AddAsync(dbModel);
            await _context.SaveChangesAsync();

            model.Id = dbModel.Id;
            return ApiResult<UserVModel>.SuccessResult(model, "用户创建成功");
        }

        public async Task<ApiResult<UserVModel>> UpdateUserAsync(UserVModel model)
        {
            var user = await _context.User.FindAsync(model.Id);
            if (user == null)
            {
                return ApiResult<UserVModel>.FailResult("用户不存在");
            }
            //    检查用户名是否与其他用户重复
            if (await _context.User.AnyAsync(m => m.LoginName == model.LoginName && m.Id != model.Id))
            {
                return ApiResult<UserVModel>.FailResult("登录名已存在");
            }

            user.Name = model.Name;
            user.LoginName = model.LoginName;
            user.Phone = model.Phone;
            user.UserNumber = model.UserNumber;
            user.DeptId = model.DeptId;
            user.UserExpiryDate = model.UserExpiryDate;
            user.Status = model.Status;
            user.Level = model.Level;
            user.Money = model.Money;
            user.Age = model.Age;
            user.Notes = model.Notes;
            user.IsDelete = false;
            await _context.SaveChangesAsync();

            var returnModel = model;
            returnModel.Id = user.Id;
            return ApiResult<UserVModel>.SuccessResult(returnModel, "用户更新成功");
        }

        public async Task<ApiResult<string>> DeleteUserByIdAsync(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return ApiResult<string>.FailResult("用户不存在");
            }
            _context.User.Remove(user); // 物理删除
            //user.IsDelete = true; // 逻辑删除
            await _context.SaveChangesAsync();
            return ApiResult<string>.SuccessResult("", "用户删除成功");
        }

        public async Task<ApiResult<UserVModel>> GetUserByIdAsync(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return ApiResult<UserVModel>.FailResult("用户不存在");
            }
            var returnModel = new UserVModel
            {
                Id = user.Id,
                Name = user.Name,
                LoginName = user.LoginName,
                Phone = user.Phone,
                UserNumber = user.UserNumber,
                DeptId = user.DeptId,
                UserExpiryDate = user.UserExpiryDate,
                Status = user.Status,
                Level = user.Level,
                Money = user.Money,
                Age = user.Age,
                Notes = user.Notes,
                IsDelete = user.IsDelete
            };
            return ApiResult<UserVModel>.SuccessResult(returnModel);
        }

        public async Task<PagedResult<UserDTO>> GetUserListAsync(UserFilterModel filter)
        {
            // 参数校验
            filter.Page = filter.Page > 0 ? filter.Page : 1;
            filter.Limit = filter.Limit > 0 ? filter.Limit : GlobalConfigs.DefaultPageSize;

            var query = _context.User.AsNoTracking().AsQueryable();
            /*
                 EF Core 默认会追踪查询出来的实体（用于之后的更新或删除）
                 如果只是读取数据，无需修改，可以用 .AsNoTracking() 提高性能
                 它会 减少内存开销 和 避免不必要的跟踪逻辑
             */
            //var query = _context.User.Where(m => m.IsDelete == false).AsNoTracking().AsQueryable();
            //var query = _context.User.Where(m => !m.IsDelete).AsNoTracking();

            // 姓名模糊查询
            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(m => EF.Functions.Like(m.Name, $"%{filter.Name}%"));

            // 员工编号模糊查询
            if (!string.IsNullOrEmpty(filter.UserNumber))
                query = query.Where(m => m.UserNumber.ToString().Contains(filter.UserNumber));

            // 部门ID模糊查询
            if (!string.IsNullOrEmpty(filter.DeptId))
                query = query.Where(m => m.DeptId == filter.DeptId);

            // 处理日期范围
            if (DateTime.TryParse(filter.StartDate, out DateTime startDate))
                query = query.Where(m => m.CreateTime >= startDate);

            if (DateTime.TryParse(filter.EndDate, out DateTime endDate))
                query = query.Where(m => m.CreateTime <= endDate);

            // 获取总数
            int total = await query.CountAsync();

            // 分页查询
            var users = await query
                .OrderByDescending(m => m.UpdateDate)
                .Skip((filter.Page - 1) * filter.Limit)
                .Take(filter.Limit)
                .AsSplitQuery()
                .Select(m => new UserDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    LoginName = m.LoginName,
                    Phone = m.Phone,
                    UserNumber = m.UserNumber,
                    DeptId = m.DeptId,
                    UserExpiryDate = m.UserExpiryDate,
                    Status = m.Status,
                    Level = m.Level,
                    Money = m.Money,
                    Age = m.Age,
                    Notes = m.Notes,
                    IsDelete = m.IsDelete
                })
                .ToListAsync();

            return PagedResult<UserDTO>.SuccessResult(users, total);
        }


        public async Task<ApiResult<string>> AssignRoleAsync(string userId, List<string> roleIds)
        {
            var user = await _context.User
                .Include(m => m.UserRoles)  // 不加Include会导致UserRoles为null
                .FirstOrDefaultAsync(m => m.Id == userId);

            //var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return ApiResult<string>.FailResult("用户不存在");
            }

            // 先删除用户所有角色
            _context.UserRole.RemoveRange(user.UserRoles);

            // 再添加新角色
            var newUserRoles = roleIds.Select(roleId => new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignStatus = 1,
                AssignedDate = DateTime.UtcNow
            });

            await _context.UserRole.AddRangeAsync(newUserRoles);
            await _context.SaveChangesAsync();
            return ApiResult<string>.SuccessResult("", "分配角色成功");
        }

    }
}