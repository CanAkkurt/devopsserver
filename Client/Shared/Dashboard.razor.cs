using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.VirtualMachines;
using Shared.Projects;
using Shared.Customers;

namespace Client.Shared;

public partial class Dashboard
{
    //search and pagination params shared between project/vm list
    [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

    private string _searchString = "";
    private List<string> _events = new();//is this needed?

    //nav
    [Inject] public NavigationManager? _navigationManager { get; set; }

    [Inject] ISnackbar snackbar { get; set; }

    //show VMs, if true show VMs, if false show projects
    private bool showVMs = true;
    private void ToggleVmsOrProjects()
    {
        showVMs = !showVMs;
    }
    private string getAddButtonText()
    {
        if (showVMs) return "Voeg VM toe";
        return "Voeg project toe";
    }

    private int totalCustomer;
    private int totalVm;
    private int totalProject;
    private int totalVcpu;
    private double totalRam;
    private double totalStorage;


    //Later on add functionality to let admin set these values
    private int totalVcpuAvailable = 384;
    private double totalRamAvailable = 2048;
    private int totalStorageAvailable = 64;//TB


    //user service to get user count
    [Inject] public ICustomerService CustomerService { get; set; }

    //stuff specific to vm-list
    private IEnumerable<VirtualMachineDto.Detail> _vms = new List<VirtualMachineDto.Detail>();
    [Inject] public IVirtualMachineService VirtualMachineService { get; set; }

    private Func<VirtualMachineDto.Detail, bool> QuickFilterVM => vm =>
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (vm.Name!.Contains(_searchString, StringComparison.OrdinalIgnoreCase)) return true;

        return false;
    };
    //==============================


    //stuff specific to project-list
    private IEnumerable<ProjectDto.Index> _projects = new List<ProjectDto.Index>();
    [Inject] public IProjectService ProjectService { get; set; }

    private Func<ProjectDto.Index, bool> QuickFilterProject => pr =>
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (pr.Name!.Contains(_searchString, StringComparison.OrdinalIgnoreCase)) return true;

        return false;
    };
    //==============================

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        //initialize and populate list of VMs based on params
        if (VirtualMachineService != null)
        {
            VirtualMachineRequest.Index request = new()
            {
                Searchterm = Searchterm,
                Page = Page.HasValue ? Page.Value : 1,
                PageSize = PageSize.HasValue ? PageSize.Value : 25,
            };

            VirtualMachineResult.Index result = await VirtualMachineService.GetIndexAsync(request);
            _vms = result.VirtualMachines!;

            request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue,
            };
            result = await VirtualMachineService.GetIndexAsync(request);
            totalVm = result.TotalAmount;           
            totalVcpu = result.VirtualMachines!.Sum(vm => vm.VCPUamount);
            totalRam = result.VirtualMachines!.Sum(vm => vm.MemoryAmount);
            totalStorage = result.VirtualMachines!.Sum(vm => vm.StorageAmount)/1024;//TB
        }

        //initialize and populate list of projects based on params
        if (ProjectService != null)
        {
            ProjectRequest.Index request = new()
            {
                Searchterm = Searchterm,
                Page = Page.HasValue ? Page.Value : 1,
                PageSize = PageSize.HasValue ? PageSize.Value : 25,
            };

            ProjectResult.Index result = await ProjectService.GetIndexAsync(request);
            _projects = result.Projects!;

            //request with unlimited pagesize to get count
            //TODO: add service method to get purely the count for efficiency
            request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue,
            };
            result = await ProjectService.GetIndexAsync(request);
            totalProject = result.TotalAmount;
        }

        //get userscount
        if (CustomerService != null)
        {
            CustomerRequest.Index request = new()
            {
                Searchterm = "",
                Page = 1,
                PageSize = int.MaxValue,
            };
            CustomerResult.Index result = await CustomerService.GetIndexAsync(request);
            totalCustomer = result.TotalAmount;
        }
    }

    private void OnAddVM()
    {
        _navigationManager?.NavigateTo("virtualmachines/add");
    }
    private void OnAddProject()
    {
        _navigationManager?.NavigateTo("projects/add");
    }

    public void VMRowClicked(DataGridRowClickEventArgs<VirtualMachineDto.Detail> vm) 
    { 
        _navigationManager?.NavigateTo($"/virtualmachines/{vm.Item.Id}");
    }
    public void ProjectRowClicked(DataGridRowClickEventArgs<ProjectDto.Index> p)
    {
        _navigationManager?.NavigateTo($"/projects/{p.Item.Id}");
    }
}