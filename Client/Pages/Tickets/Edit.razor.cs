using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Tickets;
using Shared.VirtualMachines;

namespace Client.Pages.Tickets;

public partial class Edit
{

    [Parameter] public int id { get; set; }
    [Inject] public ITicketService TicketService { get; set; } = default!;
    [Inject] ISnackbar snackbar { get; set; }
    [Inject] public IVirtualMachineService VirtualMachineService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    private TicketDto.Detail ticket = new();
    //private VirtualMachineDto.Detail vm = new();

    MudChip selected;
    MudForm? form;
    bool _isRequestingCancel = false;
    bool success;
    string[] errors = { };

    protected override async Task OnInitializedAsync()
    {
        if (TicketService != null)
        {
            TicketRequest.Detail request = new TicketRequest.Detail();
            request.Id = id;
            TicketResult.Detail result = await TicketService!.GetDetailAsync(request);
            ticket = result.Ticket;
        }

        /*if (VirtualMachineService != null)
        {

            VirtualMachineRequest.Detail request = new VirtualMachineRequest.Detail();
            //Console.WriteLine("VM ID IN EDIT:" + ticket.VirtualMachineId);
            //request.Id = ticket.VirtualMachineId;
            VirtualMachineResult.Detail result = await VirtualMachineService!.GetDetailAsync(request);
            vm = result.VirtualMachine;
        }*/
    }

    private async Task UpdateTicketAsync()
    {
        await form!.Validate();
        if (!form!.IsValid)
        {
            snackbar.Add("Not all fields are valid!", Severity.Error);
            return;
        }

        TicketRequest.Edit request = new TicketRequest.Edit();
        request.Ticket = new TicketDto.Update()
        {
            Title = ticket.Title,
            Description = ticket.Description,
            Severity = ticket.Severity,
            State = ticket.State,
            LastUpdatedAt = DateTime.Now
        };

        TicketResult.Edit result = await TicketService.EditAsync(request);

        int ticketId = result.Id;
        NavigationManager.NavigateTo($"virtualmachines/{id}");
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