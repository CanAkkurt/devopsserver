using Microsoft.AspNetCore.Components;
using global::Shared.Projects;
using MudBlazor;
using global::Shared.Customers;
using Shared.Tickets;
using Domain;
using Shared.VirtualMachines;
using Append.Blazor.Sidepanel;

namespace Client.Pages.Customers;

public partial class Detail
{
    [Inject] private ICustomerService? _customerService { get; set; }
    [Inject] private IProjectService? _projectService { get; set; }
    [Inject] private IVirtualMachineService? _virtualMachineService { get; set; }
    [Inject] public ITicketService _ticketService { get; set; } = default!;

    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] ISnackbar snackbar { get; set; }

    [Parameter] public int Id { get; set; }

    [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

    private List<TicketDto.Index> Tickets = new List<TicketDto.Index>();
    private IEnumerable<ProjectDto.Index> projects = new List<ProjectDto.Index>();
    private Dictionary<int, IEnumerable<VirtualMachineDto.Detail>> vmsByProjectId = new();
    private CustomerDto.Detail backupContact;

    //for ticketdetail
    private TicketDto.Index currentTicket;
    private SidepanelComponent sidepanel;

    bool _isRequestingCancel = false;
    bool isOpen;
    bool userIsActive = false;

    private CustomerDto.Detail customer;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_customerService != null)
        {
            CustomerRequest.Detail request = new() { Id = Id };
            var result = await _customerService.GetDetailAsync(request);
            customer = result.Customer;

            var projectResult = await _customerService.GetProjectsFromCustomerAsync(request);
            projects = projectResult.Projects!;

            ProjectRequest.Detail projectRequest = new();
            foreach (var project in projects)
            {
                projectRequest.Id = project.Id;
                var vmResult = await _projectService.GetVirtualMachinesFromProjectAsync(projectRequest);
                vmsByProjectId.Add(project.Id, vmResult.VirtualMachines);
            }
            CheckIsActive();
            await GetBackupContact();
        }

        //get tickets
        foreach (var project in vmsByProjectId)
        {
            foreach (var vm in project.Value)
            {
                VirtualMachineRequest.Detail request = new() { Id = vm.Id };
                var result = await _virtualMachineService.GetTicketsFromVirtualMachineAsync(request);
                var tickets = result.Tickets;
                Tickets.AddRange(tickets);
            }
        }

        /*if (_ticketService != null)
        {
            TicketRequest.Index ticketRequest = new()
            {
                Page = 1,
                PageSize = 2,
                Searchterm = ""
            };
            TicketResult.Index result = await _ticketService.GetIndexAsync(ticketRequest);
            tickets = result.Tickets!.Where(t => t.CustomerId == customer.Id);
        }*/


        //Debug for loading-spinner --- uncomment to see spinner
        //customer = null;
        //--------
    }

    //protected async override Task OnInitializedAsync()
    //{
    //    if (_ticketService != null)
    //    {
    //        TicketRequest.Index request = new()
    //        {
    //            Searchterm = Searchterm,
    //            Page = Page.HasValue ? Page.Value : 1,
    //            PageSize = PageSize.HasValue ? PageSize.Value : int.MaxValue,
    //        };

    //        TicketResult.Index result = await _ticketService!.GetIndexAsync(request);

    //        //TODO: where moet naar de service laag !!!
    //        tickets = result.Tickets.Where(t => t.CustomerId == Id);
    //    }
    //}

    private async Task GetVMsFromProject(int id)
    {
        VirtualMachineRequest.Index request = new()
        {
            Page = 1,
            PageSize = int.MaxValue,
            Searchterm = ""
        };
        var result = await _virtualMachineService.GetIndexAsync(request); //replace with call to get VMs by project ID instead of all vms
    }
    private async Task GetBackupContact()
    {
        CustomerRequest.Detail request = new() { Id = customer.BackupContactId };
        var result = await _customerService.GetDetailAsync(request);
        backupContact = result.Customer;
    }

    private void CheckIsActive()
    {
        bool result = false;
        //TODO: check whether user has an active VMs   
        foreach (var project in vmsByProjectId)
        {
            foreach (var vm in project.Value)
            {
                if (DateTime.Compare(vm.StartDate, DateTime.Now) <= 0 && DateTime.Compare(vm.EndDate, DateTime.Now) > 0)
                {
                    result = true;
                }
            }
        }
        userIsActive = result;
    }

    private void NavigateToEdit()
    {
        if (NavigationManager == null) { snackbar.Add("Failed to navigate to edit page", Severity.Error); return; }
        NavigationManager.NavigateTo("/customers/edit/" + Id);
    }
    private void NavigateToTicketDetail(int id)
    {
        if (NavigationManager == null) { snackbar.Add("Failed to navigate to ticket detail", Severity.Error); return; }
        NavigationManager.NavigateTo("/tickets/" + id);
    }
    private void NavigateToVMDetail(int id)
    {
        if (NavigationManager == null) { snackbar.Add("Failed to navigate to VM detail", Severity.Error); return; }
        NavigationManager.NavigateTo("/virtualmachines/" + id);
    }
    private void NavigateToBackupContact()
    {
        if (NavigationManager == null) { snackbar.Add("Failed to navigate to backup contact", Severity.Error); return; }
        if (backupContact == null) { snackbar.Add("Could not navigate to backup contact: backup contact doesn't exist", Severity.Error); return; }
        NavigationManager.NavigateTo("customers/" + backupContact?.Id);
    }


    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    private async Task DeleteUserAsync()
    {
        CustomerRequest.Delete request = new() { Id = Id };
        await _customerService.DeleteAsync(request);
        NavigationManager.NavigateTo("/customers");
    }

    //for ticketdetail
    public void Open(int id)
    {
        currentTicket = Tickets.First(t => t.Id == id);
        currentTicket.CustomerId = Id;
        sidepanel.Open();
    }
    public void Close()
    {
        sidepanel.Close();
    }
}