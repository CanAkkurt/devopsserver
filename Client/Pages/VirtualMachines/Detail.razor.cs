using Microsoft.AspNetCore.Components;
using MudBlazor;
using global::Shared.VirtualMachines;
using Shared.VirtualMachines.Ports;
using Domain;
using Append.Blazor.Sidepanel;
using Shared.Tickets;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using Client.Shared.Services;
using Shared.Customers;

namespace Client.Pages.VirtualMachines;

public partial class Detail
{
    [Inject] private IVirtualMachineService VirtualMachineService { get; set; }
    [Inject] public ITicketService TicketService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] ISidepanelService Sidepanel { get; set; }

    [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

    [Parameter] public int Id { get; set; }

    private VirtualMachineDto.Detail vm;
    private CustomerDto.Detail customer;

    private IEnumerable<VirtualMachineDto.Detail> tableItem = new List<VirtualMachineDto.Detail>();
    private IList<PortDto> ports = new List<PortDto>();
    private IEnumerable<TicketDto.Index> _tickets;

    private TicketDto.Index currentTicket;
    private SidepanelComponent sidepanel;

    bool isOpen;

    bool _isRequestingCancel = false;


    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        VirtualMachineRequest.Detail request = new() { Id = Id };
        var response = await VirtualMachineService.GetDetailAsync(request);
        vm = response.VirtualMachine;
        tableItem = tableItem.Append(vm);

        if (VirtualMachineService != null)
        {
            //VirtualMachineRequest.Detail request = new() { Id }
            var customerResult = await VirtualMachineService!.GetCustomerFromVirtualMachineAsync(request);
            customer = customerResult.Customer;
        }

        if (VirtualMachineService != null)
        {
            VirtualMachineRequest.Detail ticketRequest = new() { Id = Id };
            var result = await VirtualMachineService.GetTicketsFromVirtualMachineAsync(ticketRequest);
            _tickets = result.Tickets;
        }

        //TODO: get ports from service
        ports.Add(new PortDto() { Type = "Webserver", Nummer = 80 });//temp -- replace with actual ports from VM
        ports.Add(new PortDto() { Type = "FTP", Nummer = 21 });//temp
        ports.Add(new PortDto() { Type = "Applicatie", Nummer = 15700 });//temp
    }

    private string ActiveText()
    {
        if (DateTime.Compare(vm.StartDate, DateTime.Now) <= 0
            && DateTime.Compare(vm.EndDate, DateTime.Now) > 0)
        //&& DateTime.Compare(vm.StartDate, vm.EndDate) < 0)
        {
            return "Actief";
        }
        return "Inactief";
    }
    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    private void MailtoBackupContact()
    {
        var debugEmail = "test@email.com";
        //TODO: get backup contact email from customer
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to backup contact", Severity.Error); return; }
        NavigationManager.NavigateTo("mailto:" + debugEmail);
    }

    private void NavigateToEdit()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to edit page", Severity.Error); return; }
        NavigationManager.NavigateTo("/virtualmachines/edit/" + Id);
    }
    private void NavigateToProjectDetail()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to project detail", Severity.Error); return; }
        if (vm!.Project == null) { Snackbar.Add("Failed to navigate to project detail: vm doesn't have a linked project", Severity.Error); return; }
        NavigationManager.NavigateTo("/projects/" + vm.Project.Id);
    }
    private void NavigateToMemberDetail()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to member detail", Severity.Error); return; }
        if (vm!.Creator == null) { Snackbar.Add("Failed to navigate to member detail: vm doesn't have a linked member", Severity.Error); return; }
        NavigationManager.NavigateTo("/members/" + vm.Creator.Id);
    }
    private void NavigateToCustomerDetail()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to customer detail", Severity.Error); return; }
        if (vm!.Customer == null) { Snackbar.Add("Failed to navigate to customer detail: vm doesn't have a linked customer", Severity.Error); return; }
        NavigationManager.NavigateTo("/customers/" + vm.Customer.Id);
    }
    private void NavigateToCreateTicket()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to create ticket", Severity.Error); return; }
        NavigationManager.NavigateTo("/tickets/add/" + vm.Id + "/" + customer.Id);
    }

    private async Task DeleteVMAsync()
    {
        VirtualMachineRequest.Delete request = new() { Id = Id };
        await VirtualMachineService!.DeleteAsync(request);
        NavigationManager!.NavigateTo("/virtualmachines");
    }

    public void Open(int id)
    {
        currentTicket = _tickets.First(t => t.Id == id);
        //currentTicket.VirtualMachineId = Id;
        sidepanel.Open();
    }
    public void Close()
    {
        sidepanel.Close();
    }

}