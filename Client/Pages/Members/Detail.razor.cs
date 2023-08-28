using Microsoft.AspNetCore.Components;
using global::Shared.Members;
using MudBlazor;
using Shared.VirtualMachines;

namespace Client.Pages.Members;

public partial class Detail
{
    [Inject] private IMemberService MemberService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    [Parameter] public int Id { get; set; }

    private IEnumerable<VirtualMachineDto.Detail> virtualMachines = new List<VirtualMachineDto.Detail>();

    bool _isRequestingCancel = false;

    private MemberDto.Detail? member;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        //get member
        MemberRequest.Detail request = new() {Id = Id};
        Console.WriteLine("created MemberRequest.Detail. Id: " + Id);

        var memberResult = await MemberService.GetDetailAsync(request);
        Console.WriteLine("Got member from service. Name: " + memberResult.Member.Name);

        member = memberResult.Member;
        Console.WriteLine("Set member property to retrieved member.");

        //get VMs for which this member is responsible
        var vmsResult = await MemberService.GetVirtualMachinesFromMemberAsync(request);
        virtualMachines = vmsResult.VirtualMachines;
    }

    private void NavigateToEdit()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to edit page", Severity.Error); return; }
        NavigationManager.NavigateTo("/members/edit/" + Id);
    }
    private void NavigateToVMDetail(int id)
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to VM detail", Severity.Error); return; }
        NavigationManager.NavigateTo("/virtualmachines/" + id);
    }

    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    private async Task DeleteUserAsync()
    {
        MemberRequest.Delete request = new()
        {Id = Id};
        await MemberService.DeleteAsync(request);
        NavigationManager.NavigateTo("/members");
    }
}