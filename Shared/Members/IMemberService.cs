
using Shared.VirtualMachines;

namespace Shared.Members;

public interface IMemberService
{
    Task<MemberResult.Index> GetIndexAsync(MemberRequest.Index request);
    Task<MemberResult.Detail> GetDetailAsync(MemberRequest.Detail request);
    Task<VirtualMachineResult.Index> GetVirtualMachinesFromMemberAsync(MemberRequest.Detail request);
    Task<MemberResult.Create> CreateAsync(MemberRequest.Create request);
    Task<MemberResult.Edit> EditAsync(MemberRequest.Edit request);
    Task DeleteAsync(MemberRequest.Delete request);
}
