using Microsoft.AspNetCore.Components;
using global::Shared.Projects;
using MudBlazor;

namespace Client.Pages.Projects;

public partial class List
{
    [Parameter, SupplyParameterFromQuery] public string? Searchterm { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? Page { get; set; }
    [Parameter, SupplyParameterFromQuery] public int? PageSize { get; set; }

    private IEnumerable<ProjectDto.Index> _projects = new List<ProjectDto.Index>();
    [Inject] public IProjectService ProjectService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private string _searchString = "";
    private List<string> _events = new();

    private Func<ProjectDto.Index, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;
        if (x.Name != null && x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (x.CustomerName != null && x.CustomerName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (ProjectService != null)
        {
            ProjectRequest.Index request = new()
            { 
                Searchterm = Searchterm, 
                Page = Page.HasValue ? Page.Value : 1, 
                PageSize = PageSize.HasValue ? PageSize.Value : int.MaxValue
            };

            ProjectResult.Index result = await ProjectService.GetIndexAsync(request);
            _projects = result.Projects!;
        }
    }

    private void OnAdd()
    {
        NavigationManager?.NavigateTo("projects/add");
    }

    private void RowClicked(DataGridRowClickEventArgs<ProjectDto.Index> p)
    {
        NavigationManager?.NavigateTo($"/projects/{p.Item.Id}");
    }
}