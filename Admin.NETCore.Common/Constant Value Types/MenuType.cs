

namespace Admin.NETCore.Common.Constant_Value_Types
{
    public class MenuType
    {
        public const string Catalog = "catalog"; // 目录
        public const string Menu = "menu"; // 菜单
        public const string Button = "button"; // 按钮

        public static readonly string[] AllTypes = { Catalog, Menu, Button };
    }

    public enum MenuType2
    {
        Catalog,
        Menu,
        Button
    }
}
