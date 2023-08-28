using Client.Extensions;
using Client.Infrastructure;
using Shared.Members;
using Shared.Projects;
using Shared.VirtualMachines;
using System.Net.Http.Json;

namespace Client.Shared.Services
{
    public class ProjectService : IProjectService
    {
        //private readonly HttpClient authenticatedClient;
        private readonly PublicClient publicClient;
        private const string endpoint = "api/project";

        public ProjectService(PublicClient publicClient)
        {
            this.publicClient = publicClient;
        }

        public async Task<ProjectResult.Index> GetIndexAsync(ProjectRequest.Index request)
        {
            var queryParameters = request.GetQueryString();
            var response = await publicClient.Client.GetFromJsonAsync<ProjectResult.Index>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<ProjectResult.Detail> GetDetailAsync(ProjectRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<ProjectResult.Detail>($"{endpoint}/{request.Id}");
            return response; throw new NotImplementedException();
        }

        public async Task<VirtualMachineResult.Index> GetVirtualMachinesFromProjectAsync(ProjectRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<VirtualMachineResult.Index>($"{endpoint}/{request.Id}/vms");
            return response;
        }

        public async Task<ProjectResult.Create> CreateAsync(ProjectRequest.Create request)
        {
            var response = await publicClient.Client.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<ProjectResult.Create>();
        }

        public async Task<ProjectResult.Edit> EditAsync(ProjectRequest.Edit request)
        {
            var response = await publicClient.Client.PutAsJsonAsync($"{endpoint}/{request.Id}", request);
            return await response.Content.ReadFromJsonAsync<ProjectResult.Edit>();
        }

        public async Task DeleteAsync(ProjectRequest.Delete request)
        {
            await publicClient.Client.DeleteAsync($"{endpoint}/{request.Id}");
        }
    }
}
