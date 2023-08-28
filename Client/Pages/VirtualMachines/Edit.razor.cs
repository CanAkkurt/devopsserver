using Domain;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Services.Projects;
using Services.VirtualMachines;
using Shared.Customers;
using Shared.Members;
using Shared.Projects;
using Shared.VirtualMachines;

namespace Client.Pages.VirtualMachines;

public partial class Edit
{
    [Parameter] public int Id { get; set; }

    [Inject] ISnackbar snackbar { get; set; }
    [Inject] private NavigationManager? NavigationManager { get; set; }
    [Inject] public IVirtualMachineService? VirtualMachineService { get; set; }
    [Inject] public IMemberService? MemberService { get; set; }
    [Inject] public IProjectService? ProjectService { get; set; }
    [Inject] public ICustomerService? CustomerService { get; set; }

    private VirtualMachineDto.Detail vmDetail = new();
    private VirtualMachineDto.Update vmToUpdate = new();

    private IEnumerable<CustomerDto.Index> customers = new List<CustomerDto.Index>();
    private IEnumerable<ProjectDto.Index> projects = new List<ProjectDto.Index>();
    private IEnumerable<MemberDto.Index> members = new List<MemberDto.Index>();

    MudForm? form;
    bool _success;
    bool _isRequestingCancel;
    string[] errors = Array.Empty<string>();
    DateTime? startDate;
    DateTime? endDate;

    protected override async Task OnParametersSetAsync()
    {
        if(VirtualMachineService != null)
        {
            //get VM
            VirtualMachineRequest.Detail request = new() { Id = Id };
            var result = await VirtualMachineService.GetDetailAsync(request);
            vmDetail = result.VirtualMachine;

            //fill update DTO for form-binding
            vmToUpdate = new()
            {
                Id = vmDetail.Id,
                Name = vmDetail.Name,
                State = vmDetail.State,
                VCPUamount = vmDetail.VCPUamount,
                MemoryAmount = vmDetail.MemoryAmount,
                StorageAmount = vmDetail.StorageAmount,
                StartDate = vmDetail.StartDate,
                EndDate = vmDetail.EndDate,
                Modus = vmDetail.Modus,
                Hostname = vmDetail.Hostname,
                FQDN = vmDetail.FQDN,
                RequestDate = vmDetail.RequestDate,
                BackupFrequency = vmDetail.BackupFrequency,
                Availability = vmDetail.Availability,
                HighAvailability = vmDetail.HighAvailability,
                FysiekeServer = vmDetail.FysiekeServer,
                RelationCustomerDescription = vmDetail.RelationCustomerDescription,
                Project = vmDetail.Project,
                Creator = vmDetail.Creator,
                Customer = vmDetail.Customer
            };

            //Console.WriteLine(vmToUpdate.Project!.Name);
            //Console.WriteLine(vmToUpdate.Moderator!.Name);
            //Console.WriteLine(vmToUpdate.Customer!.Name);

            startDate = vmToUpdate.StartDate;
            endDate = vmToUpdate.EndDate;
        }

        if(ProjectService != null)
        {
            ProjectRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue
            };

            var result = await ProjectService.GetIndexAsync(request);
            projects = result.Projects!;
        }

        if (MemberService != null)
        {
            MemberRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue
            };

            var result = await MemberService.GetIndexAsync(request);
            members = result.Members!;
        }

        if (CustomerService != null)
        {
            CustomerRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue
            };

            var result = await CustomerService.GetIndexAsync(request);
            customers = result.Customers!;
        }
    }

    private async Task UpdateVirtualMachineAsync()
    {
        //validate form
        await form!.Validate();
        if (!form!.IsValid)
        {
            snackbar.Add("Not all fields are valid!", Severity.Error);
            return;
        }

        //can't bind directly using "@bind-value=..."
        vmToUpdate.StartDate = Convert.ToDateTime(startDate);
        vmToUpdate.EndDate = Convert.ToDateTime(endDate);

        if(VirtualMachineService != null)
        {
            VirtualMachineRequest.Edit request = new() { Id = Id, VirtualMachine = vmToUpdate };
            var result = await VirtualMachineService!.EditAsync(request);
            if (NavigationManager != null)
            {
                //navigate to detail of updated VM
                NavigationManager!.NavigateTo($"virtualmachines/{Id}");
            }
            else { snackbar.Add("Failed to navigate to customer detail.", Severity.Error); }
        }
        else { snackbar.Add("Failed to update VM.", Severity.Error); }  
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