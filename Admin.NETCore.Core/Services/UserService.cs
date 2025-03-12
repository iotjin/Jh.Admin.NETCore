

using Microsoft.EntityFrameworkCore;
using Admin.NETCore.Common.Configs;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB;
using Admin.NETCore.Infrastructure.DB.Entities;

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

        var returnModel = model;
        returnModel.Id = dbModel.Id;
        return ApiResult<UserVModel>.SuccessResult(returnModel, "用户创建成功");
    }

    public async Task<ApiResult<UserVModel>> UpdateUserAsync(UserVModel model)
    {
        var user = await _context.User.FindAsync(model.Id);
        if (user == null)
        {
            return ApiResult<UserVModel>.FailResult("用户不存在");
        }
        //    检查用户名是否与其他用户重复
        if (await _context.User.AnyAsync(m => m.LoginName == user.LoginName && m.Id != user.Id))
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
        _context.User.Remove(user);
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

    public async Task<PagedResult<User>> GetUserListAsync(UserFilterModel filter)
    {
        // 参数校验
        filter.Page = filter.Page > 0 ? filter.Page : 1;
        filter.Limit = filter.Limit > 0 ? filter.Limit : GlobalConfigs.DefaultPageSize;

        //var query = _context.User.AsQueryable();
        //var query = _context.User.Where(m => m.IsDelete == false).AsQueryable();
        var query = _context.User.Where(m => !m.IsDelete);

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
            .ToListAsync();
        return PagedResult<User>.SuccessResult(users, total);
    }

}
