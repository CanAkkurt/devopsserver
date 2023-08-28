using Bogus;
using Domain;
using Shared.Projects;
using Shared.VirtualMachines;

namespace Services.Projects;

public class BogusProjectService : IProjectService
{
    private readonly List<ProjectDto.Detail> _projects = new();

    public BogusProjectService()
    {
        var names = new[] { "Project 1", "Project 2", "Project 3", "Project 4", "Project 5" };
        var customerNames = new[] { "Jonas Verschueren", "Sean Van den Branden" };


        var projectFaker = new Faker<ProjectDto.Detail>("nl_BE")
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Name, f => f.PickRandom(names))
                .RuleFor(x => x.CustomerName, f => f.PickRandom(customerNames));


        _projects = projectFaker.Generate(5);
    }


    public async Task<ProjectResult.Index> GetIndexAsync(ProjectRequest.Index request)
    {
        await Task.Delay(200);

        List<ProjectDto.Index> projects = new();
        foreach (ProjectDto.Detail vm in _projects)
        {
            ProjectDto.Index v = new()
            {
                Id = vm.Id,
                Name = vm.Name,
            };
            projects.Add(v);

        }

        var result = new ProjectResult.Index
        {
            Projects = projects,
            TotalAmount = _projects.Count()
        };

        return result;
    }

    public async Task<ProjectDto.Detail> GetDetailAsync(int id)
    {
        await Task.Delay(200);
        return _projects.First(project => project.Id == id);
    }

    public async Task<int> CreateAsync(ProjectDto.Create model)
    {
        await Task.Delay(200);
        //Project project = new(model.Name!, model.CustomerName!);
        return 0;
    }


    public async Task EditAsync(int id, ProjectDto.Update model)
    {
        await Task.Delay(200);
    }


    public async Task DeleteAsync(int id)
    {
        await Task.Delay(200);
        var project = _projects.SingleOrDefault(x => x.Id == id);
        if (project != null)
        {
            _projects.Remove(project);
        }
    }

    public Task<ProjectResult.Detail> GetDetailAsync(ProjectRequest.Detail request)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectResult.Create> CreateAsync(ProjectRequest.Create request)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectResult.Edit> EditAsync(ProjectRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(ProjectRequest.Delete request)
    {
        throw new NotImplementedException();
    }

    public Task<VirtualMachineResult.Index> GetVirtualMachinesFromProjectAsync(ProjectRequest.Detail request)
    {
        throw new NotImplementedException();
    }
}
