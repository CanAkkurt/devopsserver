using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Customers;
using Shared.Projects;

namespace Client.Pages.Projects;

public partial class Edit
{
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] public IProjectService ProjectService { get; set; }

    [Parameter] public int Id { get; set; }

    private ProjectDto.Detail project;
    private ProjectDto.Update projectToUpdate = new();

    MudForm? form;
    bool success;
    bool _isRequestingCancel = false;
    string[] errors = Array.Empty<string>();

    protected override async Task OnParametersSetAsync()
    {
        await base.OnInitializedAsync();

        if (ProjectService != null)
        {
            ProjectRequest.Detail request = new() { Id = Id };
            var result = await ProjectService.GetDetailAsync(request);
            project = result.Project;

            projectToUpdate.Name = project.Name;
            projectToUpdate.State = project.State;
        }
    }

    private async Task UpdateCustomerAsync()
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
            //update project
            ProjectRequest.Edit request = new() { Project = projectToUpdate, Id = Id };
            var result = await ProjectService.EditAsync(request);

            if (NavigationManager != null)
            {
                //navigate to detail of updated project
                NavigationManager!.NavigateTo("projects/" + result.Id);
            }
            else { Snackbar.Add("Failed to navigate to project detail.", Severity.Error); }
        }
        else { Snackbar.Add("Failed to update project.", Severity.Error); }
    }

    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    private void Reset()
    {
        _isRequestingCancel = !_isRequestingCancel;
        //form!.Reset();
        projectToUpdate.Name = project.Name;
        projectToUpdate.State = project.State;
    }
}