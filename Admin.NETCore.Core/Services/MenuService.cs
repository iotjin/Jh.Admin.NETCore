using Microsoft.EntityFrameworkCore;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Core.ViewModels;
using Admin.NETCore.Core.ViewModels.Base;
using Admin.NETCore.Infrastructure.DB;
using Admin.NETCore.Infrastructure.DB.Entities;
using System.Collections.Generic;


namespace Admin.NETCore.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDbContext _context;

        public MenuService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<MenuVModel>> CreateOrUpdateMenuAsync(MenuVModel model)
        {
            var result = new ApiResult<MenuVModel>();

            // 检查同级菜单名称不能重复
            bool nameExists = await _context.Menu
                .AnyAsync(m => m.ParentId == model.ParentId && m.Title == model.Title && m.Id != model.Id);
            if (nameExists)
            {
                return result.Fail("同级菜单名称已存在，请修改后重试");
            }

            bool isNew = string.IsNullOrWhiteSpace(model.Id);

            // code 重复校验（排除当前记录的 Id）
            var exists = await _context.Menu
                .Where(m => m.Id != model.Id && (m.Code == model.Code))
                .ToListAsync();

            if (exists.Any(m => m.Code == model.Code))
                return result.Fail("Code已存在");

            // 编辑
            if (!isNew)
            {
                var existModel = await _context.Menu.FindAsync(model.Id);
                if (existModel == null)
                    return result.Fail("菜单不存在");

                existModel.MenuType = model.MenuType;
                existModel.Title = model.Title;
                existModel.Code = model.Code;
                existModel.ParentId = model.ParentId;
                existModel.ParentTitle = model.ParentTitle;
                existModel.Sort = model.Sort;
                existModel.Icon = model.Icon;
                existModel.Hidden = model.Hidden;
                existModel.Component = model.Component;
                existModel.IsDelete = false;

                await _context.SaveChangesAsync();

                return result.Success(model, "菜单更新成功");
            }
            else
            {
                // 新增
                var dbModel = new Menu
                {
                    Id = Guid.NewGuid().ToString(),
                    MenuType = model.MenuType,
                    Title = model.Title,
                    Code = model.Code,
                    ParentId = model.ParentId,
                    ParentTitle = model.ParentTitle,
                    Sort = model.Sort,
                    Icon = model.Icon,
                    Hidden = model.Hidden,
                    Component = model.Component,
                    IsDelete = false
                };
                await _context.Menu.AddAsync(dbModel);
                await _context.SaveChangesAsync();

                model.Id = dbModel.Id;
                return result.Success(model, "菜单创建成功");
            }
        }

        public async Task<ApiResult<string>> DeleteMenuByIdsAsync(List<string> ids)
        {
            var result = new ApiResult<string>();

            // 查找对应的记录
            var exists = await _context.Menu
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            // 不管传入的 ids 是否都存在，只要能匹配的就删除, 不会报错，也不会终止操作
            if (!exists.Any())
            {
                return result.Fail("未找到任何要删除的菜单");
            }

            // 判断是否存在子菜单
            bool hasChild = await _context.Menu
                .AnyAsync(m => m.ParentId != null && ids.Contains(m.ParentId));

            if (hasChild)
            {
                return result.Fail("存在子菜单，禁止删除，请先删除子菜单");
            }


            //// 如果找到的id和传入的不一致，不删除 （只要有一个 ID 不存在，就全部不删）
            //if (exists.Count != ids.Count)
            //{
            //    return result.Fail("部分菜单不存在，操作终止");
            //}

            _context.Menu.RemoveRange(exists); // 物理删除
            //foreach (var item in exists)  // 逻辑删除
            //{
            //    item.IsDelete = true;
            //}
            await _context.SaveChangesAsync();

            return result.Success("", "菜单删除成功");
        }


        public async Task<ApiResult<string>> DeleteMenuByIdAsync(string id)
        {
            var ids = new List<string> { id };
            return await DeleteMenuByIdsAsync(ids);
        }

        public async Task<ApiResult<MenuListDTO>> GetMenuByIdAsync(string id)
        {
            var result = new ApiResult<MenuListDTO>();

            //var existModel = await _context.Menu.FindAsync(id);
            var existModel = await _context.Menu.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id); // 只读数据用AsNoTracking提高性能
            if (existModel == null)
            {
                return result.Fail("菜单不存在");
            }

            // 查询所有菜单
            var allMenus = await _context.Menu
                .AsNoTracking()
                .OrderBy(m => m.Sort)
                .Select(m => new MenuListDTO
                {
                    Id = m.Id,
                    MenuType = m.MenuType,
                    Title = m.Title,
                    Code = m.Code,
                    ParentId = m.ParentId,
                    ParentTitle = m.ParentTitle,
                    Sort = m.Sort,
                    Icon = m.Icon,
                    Hidden = m.Hidden,
                    Component = m.Component,
                    IsDelete = m.IsDelete,
                    Children = new List<MenuListDTO>()
                })
                .ToListAsync();

            if (allMenus.Count == 0)
                return result.Success(new MenuListDTO());

            // 建立字典
            var menuDict = allMenus.ToDictionary(m => m.Id!);

            // 构造树结构
            foreach (var menu in allMenus)
            {
                if (!string.IsNullOrEmpty(menu.ParentId) && menuDict.TryGetValue(menu.ParentId, out var parent))
                {
                    menu.ParentTitle = parent.Title;
                    parent.Children!.Add(menu);
                }
            }

            // 返回指定 id 对应的菜单（包含其子菜单）
            if (menuDict.TryGetValue(id, out var targetMenu))
            {
                return result.Success(targetMenu);
            }
            return result.Fail("菜单不存在");
        }

        public async Task<ApiResult<List<MenuListDTO>>> GetAllMenuTreeListAsync(MenuFilterModel filter)
        {
            var result = new ApiResult<List<MenuListDTO>>();

            // 1. 查询并过滤
            var menus = await _context.Menu
                .AsNoTracking()
                .Where(m =>
                    // 如果 filter.Title 或 filter.Code 为空，则不过滤，如果不为空，则进行模糊查询
                    (string.IsNullOrEmpty(filter.Title) || m.Title.Contains(filter.Title)) &&
                    (string.IsNullOrEmpty(filter.Code) || m.Code.Contains(filter.Code))
                )
                .OrderBy(m => m.Sort)
                .Select(m => new MenuListDTO
                {
                    Id = m.Id,
                    MenuType = m.MenuType,
                    Title = m.Title,
                    Code = m.Code,
                    ParentId = m.ParentId,
                    ParentTitle = m.ParentTitle,
                    Sort = m.Sort,
                    Icon = m.Icon,
                    Hidden = m.Hidden,
                    Component = m.Component,
                    IsDelete = m.IsDelete,
                    Children = new List<MenuListDTO>()
                })
                .ToListAsync();

            if (menus.Count == 0)
                return result.Success(new List<MenuListDTO>());

            // 2. 建立字典
            var menuDict = menus.ToDictionary(m => m.Id!);

            // 3. 构造树结构
            var treeList = new List<MenuListDTO>();
            foreach (var menu in menus)
            {
                if (!string.IsNullOrEmpty(menu.ParentId) && menuDict.TryGetValue(menu.ParentId, out var parent))
                { // 有父节点
                    menu.ParentTitle = parent.Title;
                    parent.Children!.Add(menu);
                }
                else
                { // 无父节点
                    treeList.Add(menu);
                }
            }

            return result.Success(treeList);
        }

        public async Task<ApiResult<List<MenuListDTO>>> GetMenuInfoByRoleIdAsync(string roleId)
        {
            // 先不实现
            return ApiResult<List<MenuListDTO>>.SuccessResult(new List<MenuListDTO>());
        }

        public async Task<ApiResult<List<MenuListDTO>>> GetMenuInfoByUserIdAsync(string userId)
        {
            // 先不实现
            return ApiResult<List<MenuListDTO>>.SuccessResult(new List<MenuListDTO>());
        }
    }
}