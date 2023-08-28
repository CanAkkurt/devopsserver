

namespace Shared.VirtualMachines;

public static class VirtualMachineRequest
{
    public class Index
    {
        public string? Searchterm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }

    public class Detail
    {
        public int Id { get; set; }
    }

    public class Create
    {
        public VirtualMachineDto.Create VirtualMachine { get; set; }
    }

    public class Edit
    {
        public int Id { get; set; }
        public VirtualMachineDto.Update VirtualMachine { get; set; }
    }

    public class Delete
    {
        public int Id { get; set; }
    }
}
