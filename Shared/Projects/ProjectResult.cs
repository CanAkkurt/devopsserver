

using Shared.Customers;

namespace Shared.Projects
{
    public class ProjectResult
    {
        public class Index
        {
            public IEnumerable<ProjectDto.Index>? Projects { get; set; }
            public int TotalAmount { get; set; }
        }

        public class Detail
        {
            public ProjectDto.Detail Project { get; set; }
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
    }
}
