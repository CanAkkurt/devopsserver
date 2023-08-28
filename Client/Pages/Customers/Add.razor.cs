using Microsoft.AspNetCore.Components;
using global::Shared.Customers;
using MudBlazor;

namespace Client.Pages.Customers;

public partial class Add
{
    [Inject] ISnackbar snackbar { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] public ICustomerService? CustomerService { get; set; }

    private CustomerDto.Create newCustomer = new();
    private IEnumerable<CustomerDto.Index> customersForBackupContact = new List<CustomerDto.Index>();

    MudForm? form;
    bool success;
    bool _isRequestingCancel = false;
    string[] errors = Array.Empty<string>();
    CustomerDto.Index backupContact;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if(CustomerService != null)
        {
            CustomerRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue
            };

            CustomerResult.Index result = await CustomerService.GetIndexAsync(request);
            customersForBackupContact = result.Customers!;
        }
    }

    private async Task CreateCustomerAsync()
    {
        //validate form
        await form!.Validate();
        if (!form.IsValid)
        {
            snackbar.Add("Niet alle velden zijn geldig!", Severity.Error);
            return;
        }

        if(CustomerService != null)
        {
            newCustomer.BackupContactId = backupContact.Id;

            //create customer
            CustomerRequest.Create request = new() { Customer = newCustomer };
            var result = await CustomerService.CreateAsync(request);

            if (NavigationManager != null)
            {
                //navigate to detail of created customer
                NavigationManager!.NavigateTo("customers/" + result.Id);
            }
            else{ snackbar.Add("Failed to navigate to customer detail.", Severity.Error); }
        }
        else{ snackbar.Add("Failed to create customer.", Severity.Error); }
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