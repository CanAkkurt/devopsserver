using Microsoft.AspNetCore.Components;
using global::Shared.Customers;
using MudBlazor;

namespace Client.Pages.Customers;

public partial class Edit
{
    [Inject] ISnackbar snackbar { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] public ICustomerService? CustomerService { get; set; }

    [Parameter] public int Id { get; set; }

    private CustomerDto.Detail customerDetail = new();
    private CustomerDto.Update customerToUpdate = new();
    private IEnumerable<CustomerDto.Index> customersForBackupContact = new List<CustomerDto.Index>();

    MudForm? form;
    bool success;
    bool _isRequestingCancel = false;
    string[] errors = Array.Empty<string>();
    CustomerDto.Index backupContact;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (CustomerService != null)
        {
            CustomerRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue
            };

            CustomerResult.Index result = await CustomerService.GetIndexAsync(request);
            customersForBackupContact = result.Customers!;

            CustomerRequest.Detail detailRequest = new() { Id = Id };
            var detailResult = await CustomerService.GetDetailAsync(detailRequest);
            customerDetail = detailResult.Customer;

            customerToUpdate = new()
            {
                Id = customerDetail.Id,
                Name = customerDetail.Name,
                Email = customerDetail.Email,
                PhoneNumber = customerDetail.PhoneNumber,
                Department = customerDetail.Department,
                ExternType = customerDetail.ExternType,
                Education = customerDetail.Education,
                BackupContactId = customerDetail.BackupContactId,
            };
            backupContact = customersForBackupContact.First(bc => bc.Id == customerToUpdate.BackupContactId);
        }
    }

    private async Task UpdateCustomerAsync()
    {
        //validate form
        await form!.Validate();
        if (!form.IsValid)
        {
            snackbar.Add("Niet alle velden zijn geldig!", Severity.Error);
            return;
        }

        if (CustomerService != null)
        {
            customerToUpdate.BackupContactId = backupContact.Id;

            //create customer
            CustomerRequest.Edit request = new() { Id = customerToUpdate.Id, Customer = customerToUpdate};
            var result = await CustomerService.EditAsync(request);
            if (NavigationManager != null)
            {
                //navigate to detail of created customer
                NavigationManager!.NavigateTo("customers/" + customerToUpdate.Id);
            }
            else { snackbar.Add("Failed to navigate to customer detail.", Severity.Error); }
        }
        else { snackbar.Add("Failed to update customer.", Severity.Error); }
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