using Admin.NETCore.Core.ViewModels.Base;

namespace Admin.NETCore.Core.ViewModels
{
    public class DictTypeListDTO : BaseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public int Sort { get; set; }

        public int Status { get; set; }

        public int Builtin { get; set; }

        public string? Notes { get; set; }
    }



    public class DictItemSimpleDto
    {
        public string Id { get; set; } = null!;
        public string Label { get; set; } = null!;
        public string Value { get; set; } = null!;
    }

    public class DictTypesAndItemsDTO : Dictionary<string, List<DictItemSimpleDto>>
    {
    }

    //public class DictTypesAndItemsDTO
    //{
    //    public Dictionary<string, List<DictItemSimpleDto>> Data { get; set; } = new();
    //}

}
