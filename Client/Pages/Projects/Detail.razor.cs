using Client.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;
using Shared.Projects;
using Shared.Tickets;
using Shared.VirtualMachines;
using System;

namespace Client.Pages.Projects;

public partial class Detail
{
    [Inject] private IProjectService ProjectService { get; set; }
    [Inject] private IVirtualMachineService VirtualMachineService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    [Parameter] public int Id { get; set; }

    //private IList<TicketDto.
    private IEnumerable<VirtualMachineDto.Detail> virtualMachines = new List<VirtualMachineDto.Detail>();
    private Dictionary<int, IEnumerable<TicketDto.Index>> ticketsByVmId = new();
    private ProjectDto.Detail project;

    bool _isRequestingCancel = false;
    bool projectIsActive = false;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (ProjectService != null)
        {
            ProjectRequest.Detail request = new() { Id = Id };
            var projectResult = await ProjectService.GetDetailAsync(request);//get project
            project = projectResult.Project; 

            var vmResult = await ProjectService.GetVirtualMachinesFromProjectAsync(request);//get vms from project
            virtualMachines = vmResult.VirtualMachines;
        }

        VirtualMachineRequest.Detail vmRequest = new();
        foreach (var vm in virtualMachines)
        {
            vmRequest.Id = vm.Id;
            var ticketResult = await VirtualMachineService.GetTicketsFromVirtualMachineAsync(vmRequest);
            ticketsByVmId.Add(vm.Id, ticketResult.Tickets);
            Console.WriteLine($"added {ticketResult.Tickets.Count()} tickets to dictionary");
        }

        //check if project is active based on startdate and enddate
        projectIsActive = DateTime.Compare(project.StartDate, DateTime.Now) <= 0 && DateTime.Compare(project.EndDate, DateTime.Now) > 0;
    }

    private void NavigateToEdit()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to edit page", Severity.Error); return; }
        NavigationManager.NavigateTo("/projects/edit/" + Id);
    }
    private void NavigateToVMDetail(int id)
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to VM detail", Severity.Error); return; }
        NavigationManager.NavigateTo("/virtualmachines/" + id);
    }
    //TODO: ticket detail nav
    private void NavigateToCustomerDetail()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to VM detail", Severity.Error); return; }
        NavigationManager.NavigateTo("/customers/" + project.CustomerId);
    }

    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    public async Task DeleteProjectAsync()
    {
        ProjectRequest.Delete request = new() { Id = Id };
        await ProjectService.DeleteAsync(request);
        NavigationManager.NavigateTo("/projects");
    }
}