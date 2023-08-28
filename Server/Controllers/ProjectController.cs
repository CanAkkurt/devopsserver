using Microsoft.AspNetCore.Mvc;
using Shared.Projects;
using Shared.VirtualMachines;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            this._projectService = projectService;
        }

        [SwaggerOperation("Returns a list of projects.")]
        [HttpGet]
        public async Task<ProjectResult.Index> GetIndex([FromQuery] ProjectRequest.Index request)
        {
            return await _projectService.GetIndexAsync(request);
        }

        [SwaggerOperation("Returns a specific project.")]
        [HttpGet("{id}")]
        public async Task<ProjectResult.Detail> GetDetail([FromRoute] ProjectRequest.Detail request)
        {
            return await _projectService.GetDetailAsync(request);
        }
        [SwaggerOperation("Returns the VMs of a specific project.")]
        [HttpGet("{id}/vms")]
        public async Task<VirtualMachineResult.Index> GetVirtualMachinesFromMember([FromRoute] ProjectRequest.Detail request)
        {
            return await _projectService.GetVirtualMachinesFromProjectAsync(request);
        }

        [SwaggerOperation("Creates a new project.")]
        [HttpPost]
        public async Task<ProjectResult.Create> Create([FromBody] ProjectRequest.Create request)
        {
            return await _projectService.CreateAsync(request);
        }

        [SwaggerOperation("Edits an existing project.")]
        [HttpPut("{id}")]
        public async Task<ProjectResult.Edit> Edit([FromBody] ProjectRequest.Edit request)
        {
            return await _projectService.EditAsync(request);
        }

        [SwaggerOperation("Deletes an existing project.")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] ProjectRequest.Delete request)
        {
            await _projectService.DeleteAsync(request);
            return NoContent();
        }
    }
}
