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
using Client.Shared;
using global::Shared.Members;
using global::Shared.Projects;
using MudBlazor;
using global::Shared.Customers;

namespace Client.Pages.Customers
{
    public partial class List
    {
        [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
        [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
        [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

        private IEnumerable<CustomerDto.Index> _customers = new List<CustomerDto.Index>();
        [Inject] public ICustomerService? CustomerService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private string _searchString = "";
        private List<string> _events = new();

        private Func<CustomerDto.Index, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (x.Name != null && x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.Email != null && x.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.PhoneNumber != null && x.PhoneNumber.ToString()!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            CustomerRequest.Index request = new()
            {Searchterm = Searchterm, Page = Page.HasValue ? Page.Value : 1, PageSize = PageSize.HasValue ? PageSize.Value : int.MaxValue };

            CustomerResult.Index result = await CustomerService.GetIndexAsync(request);
            _customers = result.Customers;
        }

        private void OnAdd()
        {
            NavigationManager?.NavigateTo("customers/add");
        }

        private void RowClicked(DataGridRowClickEventArgs<CustomerDto.Index> p)
        {
            NavigationManager?.NavigateTo($"/customers/{p.Item.Id}");
        }
    }
}