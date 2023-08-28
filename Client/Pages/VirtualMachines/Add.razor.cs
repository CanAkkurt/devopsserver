using Microsoft.AspNetCore.Components;
using MudBlazor;
using Domain;
using Shared.Customers;
using Shared.VirtualMachines;
using Shared.Projects;
using Shared.Members;

namespace Client.Pages.VirtualMachines;

public partial class Add
{
    [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

    [Inject] ISnackbar snackbar { get; set; }
    [Inject] private NavigationManager? _navigationManager { get; set; }
    [Inject] public IVirtualMachineService? _virtualMachineService { get; set; }
    [Inject] public ICustomerService? _customerService { get; set; }
    [Inject] public IMemberService? _memberService { get; set; }
    [Inject] public IProjectService? _projectService { get; set; }

    private VirtualMachineDto.Create virtualMachine = new();
    private IEnumerable<CustomerDto.Index> _customers = new List<CustomerDto.Index>();
    private IEnumerable<MemberDto.Index> _members = new List<MemberDto.Index>();
    private IEnumerable<ProjectDto.Index> _projects = new List<ProjectDto.Index>();

    MudForm? form;
    bool success;
    bool _isRequestingCancel = false;
    string[] errors = { };
    DateTime? startDate;
    DateTime? endDate;


    private Modus[] _listModus =
    {
        Modus.IAAS,
        Modus.PAAS,
        Modus.SAAS,
        Modus.Sjabloon1,
        Modus.Sjabloon2
    };

    private async Task<IEnumerable<Modus>> SearchModus(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return _listModus;
        return _listModus.Where(x => x.ToString().Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (_customerService != null)
        {
            CustomerRequest.Index request = new()
            {
                Searchterm = Searchterm,
                Page = Page.HasValue ? Page.Value : 1,
                PageSize = PageSize.HasValue ? PageSize.Value : 25,
            };

            CustomerResult.Index result = await _customerService.GetIndexAsync(request);
            _customers = result.Customers!;
        }

        if (_projectService != null)
        {
            ProjectRequest.Index request = new()
            {
                Searchterm = Searchterm,
                Page = Page.HasValue ? Page.Value : 1,
                PageSize = PageSize.HasValue ? PageSize.Value : 25,
            };

            ProjectResult.Index result = await _projectService.GetIndexAsync(request);
            _projects = result.Projects!;
        }

        if (_memberService != null)
        {
            MemberRequest.Index request = new()
            {
                Searchterm = Searchterm,
                Page = Page.HasValue ? Page.Value : 1,
                PageSize = PageSize.HasValue ? PageSize.Value : 25,
            };

            MemberResult.Index result = await _memberService.GetIndexAsync(request);
            _members = result.Members!;
            Console.WriteLine(_members.First().Name);
        }
    }

    private async Task CreateVirtualMachineAsync()
    {
        await form!.Validate();
        if (!form!.IsValid)
        {
            snackbar.Add("Not all fields are valid!", Severity.Error);
            return;
        }

        //can't bind directly using "@bind-value=..."
        VirtualMachineRequest.Create request = new();
        request.VirtualMachine = virtualMachine;
        request.VirtualMachine.StartDate = Convert.ToDateTime(startDate);
        request.VirtualMachine.EndDate = Convert.ToDateTime(endDate);

        var result = await _virtualMachineService!.CreateAsync(request);
        _navigationManager!.NavigateTo($"virtualmachines/{result.Id}");
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