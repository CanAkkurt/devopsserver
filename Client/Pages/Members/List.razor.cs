using Microsoft.AspNetCore.Components;
using global::Shared.Members;
using MudBlazor;
using Shared.Customers;

namespace Client.Pages.Members
{
    public partial class List
    {
        [Parameter, SupplyParameterFromQuery]  public string? Searchterm { get; set; }
        [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
        [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

        private IEnumerable<MemberDto.Index> _members = new List<MemberDto.Index>();
        [Inject] public IMemberService MemberService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private string _searchString = "";
        private List<string> _events = new();
        private Func<MemberDto.Index, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (x.Name != null && x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.Email != null && x.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        };
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (MemberService != null)
            {
                MemberRequest.Index request = new()
                {Searchterm = Searchterm, Page = Page.HasValue ? Page.Value : 1, PageSize = PageSize.HasValue ? PageSize.Value : 25, };
                MemberResult.Index result = await MemberService.GetIndexAsync(request);
                _members = result.Members!;
            }
        }

        private void OnAdd()
        {
            NavigationManager?.NavigateTo("members/add");
        }

        private void RowClicked(DataGridRowClickEventArgs<MemberDto.Index> p)
        {
            NavigationManager?.NavigateTo($"/members/{p.Item.Id}");
        }
    }
}