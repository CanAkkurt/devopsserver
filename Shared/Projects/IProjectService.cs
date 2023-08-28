

using Shared.VirtualMachines;

namespace Shared.Projects
{
    public interface IProjectService
    {
        Task<ProjectResult.Index> GetIndexAsync(ProjectRequest.Index request);
        Task<ProjectResult.Detail> GetDetailAsync(ProjectRequest.Detail request);
        Task<VirtualMachineResult.Index> GetVirtualMachinesFromProjectAsync(ProjectRequest.Detail request);
        Task<ProjectResult.Create> CreateAsync(ProjectRequest.Create request);
        Task<ProjectResult.Edit> EditAsync(ProjectRequest.Edit request);
        Task DeleteAsync(ProjectRequest.Delete request);
    }
}
