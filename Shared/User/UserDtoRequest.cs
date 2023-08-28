using Shared.VirtualMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.User
{
    public static class UserDtoRequest
    {
        public class Index
        {
            public string? Searchterm { get; set; }
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 25;
        }

        public class Edit
        {
            public int Id { get; set; }
            public UserDto.update user  { get; set; }
        }
    }
}
