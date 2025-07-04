
using System.ComponentModel.DataAnnotations;

namespace Admin.NETCore.Common.Validators
{
    /// <summary>
    /// 用于验证 ID 列表是否非空，且每个 ID 为指定长度的非空字符串
    /// </summary>
    public class IdListValidatorAttribute : ValidationAttribute
    {
        private readonly int _idLength;

        /// <param name="idLength">每个 ID 的长度，默认 36</param>
        /// <param name="errorMessage">自定义错误信息（可选）</param>
        public IdListValidatorAttribute(int idLength = 36, string? errorMessage = null)
        {
            _idLength = idLength;

            // 如果用户没有显式设置 ErrorMessage，就用默认内容
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ErrorMessage = errorMessage;
            else
                ErrorMessage = $"ID 列表不能为空，且每个 ID 必须为 {_idLength} 位有效字符串。";
        }

        public override bool IsValid(object? value)
        {
            if (value is not IEnumerable<string> list || !list.Any())
                return false;

            return list.All(id => !string.IsNullOrWhiteSpace(id) && id.Length == _idLength);
        }
    }
}
