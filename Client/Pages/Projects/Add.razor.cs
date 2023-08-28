using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Customers;
using Shared.Projects;

namespace Client.Pages.Projects;

public partial class Add
{
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] public IProjectService ProjectService { get; set; }
    [Inject] public ICustomerService CustomerService { get; set; }

    private ProjectDto.Create newProject = new();
    private IEnumerable<CustomerDto.Index> customers = new List<CustomerDto.Index>();

    MudForm? form;
    bool success;
    bool _isRequestingCancel = false;
    string[] errors = Array.Empty<string>();
    CustomerDto.Index customer;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (CustomerService != null)
        {
            CustomerRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue
            };

            CustomerResult.Index result = await CustomerService.GetIndexAsync(request);
            customers = result.Customers!;
        }
    }

    private async Task CreateCustomerAsync()
    {
        //validate form
        await form!.Validate();
        if (!form.IsValid)
        {
            Snackbar.Add("Niet alle velden zijn geldig!", Severity.Error);
            return;
        }

        if (ProjectService != null)
        {
            newProject.CustomerId = customer.Id;

            //create project
            ProjectRequest.Create request = new() { Project = newProject };
            var result = await ProjectService.CreateAsync(request);

            if (NavigationManager != null)
            {
                //navigate to detail of created project
                NavigationManager!.NavigateTo("projects/" + result.Id);
            }
            else { Snackbar.Add("Failed to navigate to project detail.", Severity.Error); }
        }
        else { Snackbar.Add("Failed to create project.", Severity.Error); }
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