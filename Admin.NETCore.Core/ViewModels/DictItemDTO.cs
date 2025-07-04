using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.ViewModels
{
    public class DictItemListDTO : BaseModel
    {
        public string Id { get; set; } = null!;

        public string Label { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string DictTypeCode { get; set; } = null!;

        public int Sort { get; set; }

        public int Status { get; set; }

        public int Builtin { get; set; }

        public string? Notes { get; set; }
    }
}
