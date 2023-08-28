using Microsoft.AspNetCore.Components;
using Shared.Tickets;
using Shared.Customers;
using Shared.VirtualMachines;
using MudBlazor;

namespace Client.Pages.Tickets;

public partial class Add
{
    
    [Inject] public ITicketService TicketService { get; set; } = default!;
    [Inject] ISnackbar snackbar { get; set; }
    [Inject] public IVirtualMachineService VirtualMachineService { get; set; } = default!;
    [Inject] public ICustomerService CustomerService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public int VmId { get; set; }
    [Parameter] public int CustId { get; set; }

    private TicketDto.Create ticket = new();
    private VirtualMachineDto.Detail vm;
    private CustomerDto.Detail customer;
    

    MudChip selected;
    MudForm? form;
    bool _isRequestingCancel = false;
    bool success;
    string[] errors = { };

    private async Task CreateTicketAsync()
    {
        await form!.Validate();
        if (!form!.IsValid)
        {
            snackbar.Add("Not all fields are valid!", Severity.Error);
            return;
        }

        //Fill in properties of ticket
        ticket.Severity = selected.Text.ToUpper();//selected.Value.ToString();
        ticket.CreatedAt = DateTime.Now;
        ticket.CustomerId = customer.Id;

        TicketRequest.Create request = new TicketRequest.Create();
        request.Ticket = ticket;

        TicketResult.Create result = await TicketService.CreateAsync(request);

        int ticketId = result.Id;
        NavigationManager.NavigateTo($"virtualmachines/{VmId}");
    }

    protected override async Task OnInitializedAsync()
    {
        if(VirtualMachineService != null)
        {
            VirtualMachineRequest.Detail request = new VirtualMachineRequest.Detail();
            request.Id = VmId;
            VirtualMachineResult.Detail result = await VirtualMachineService!.GetDetailAsync(request);
            vm = result.VirtualMachine;
        }
        if (CustomerService != null)
        {
            CustomerRequest.Detail request = new()
            {
                Id = CustId
            };
            CustomerResult.Detail result = await CustomerService!.GetDetailAsync(request);
            customer = result.Customer;
        }
    }


    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    private void Reset()
    {
        _isRequestingCancel = !_isRequestingCancel;
        form!.Reset();
    }
}

