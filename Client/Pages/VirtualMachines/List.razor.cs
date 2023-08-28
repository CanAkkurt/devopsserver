using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.VirtualMachines;

namespace Client.Pages.VirtualMachines
{
    public partial class List
    {
        [Inject] public IVirtualMachineService? _virtualMachineService { get; set; }
        [Inject] public NavigationManager? _navigationManager { get; set; }

        private IEnumerable<VirtualMachineDto.Detail> _vms;

        [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
        [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
        [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }
       
        private string _searchString = "";
        private List<string> _events = new();

        private Func<VirtualMachineDto.Detail, bool> _quickFilter => vm =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (vm.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (_virtualMachineService != null)
            {
                VirtualMachineRequest.Index request = new()
                {
                    Searchterm = Searchterm,
                    Page = Page.HasValue ? Page.Value : 1,
                    PageSize = PageSize.HasValue ? PageSize.Value : int.MaxValue,
                };

                VirtualMachineResult.Index result = await _virtualMachineService.GetIndexAsync(request);
                _vms = result.VirtualMachines!;
            }
        }

        private void onAdd()
        {
            _navigationManager?.NavigateTo("virtualmachines/add");
        }

        public void RowClicked(DataGridRowClickEventArgs<VirtualMachineDto.Detail> p)
        {
            // Example:
            _navigationManager?.NavigateTo($"/virtualmachines/{p.Item.Id}");
            //NavigationManager.NavigateTo($"/virtualmachines/{p.Item.Id}");
        }
    }
}
