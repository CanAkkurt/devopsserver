using Append.Blazor.Sidepanel;
using global::Shared.Tickets;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages.Tickets;

public partial class Detail
{
    [Inject] public ITicketService? TicketService { get; set; } = default!;
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] public ISidepanelService Sidepanel { get; set; }


    [Parameter] public int id { get; set; }
    private TicketDto.Detail ticket;
    protected override async Task OnInitializedAsync()
    {
        if (TicketService != null)
        {
            TicketRequest.Detail request = new TicketRequest.Detail();
            request.Id = id;
            TicketResult.Detail result = await TicketService!.GetDetailAsync(request);
            ticket = result.Ticket;
        }
    }


    private void Edit()
    {
        if (NavigationManager == null) { Snackbar.Add("Failed to navigate to edit ticket", Severity.Error); return; }
        NavigationManager.NavigateTo("/tickets/edit/" + ticket.Id);
    }

    private void Close()
    {
        Sidepanel?.Close();
    }
}