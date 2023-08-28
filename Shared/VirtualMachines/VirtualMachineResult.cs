using Shared.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.VirtualMachines;

public abstract class VirtualMachineResult
{
    public class Index
    {
        public IEnumerable<VirtualMachineDto.Detail>? VirtualMachines { get; set; }
        public int TotalAmount { get; set; }
    }

    public class Detail
    {
        public VirtualMachineDto.Detail VirtualMachine { get; set; }
    }

    public class Create
    {
        public int Id { get; set; }
    }

    public class Edit
    {
        public int Id { get; set; }
    }

    public class Delete
    {

    }


    public class Usage
    {
        public IEnumerable<VirtualMachineDto.Usage>? VirtualMachines { get; set; }

    }
}
