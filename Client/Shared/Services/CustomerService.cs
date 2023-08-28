using Client.Extensions;
using Client.Infrastructure;
using Shared.Customers;
using Shared.Projects;
using System.Net.Http.Json;

namespace Client.Shared.Services
{
    public class CustomerService : ICustomerService
    {
        //Change methods to authenticatedClient if needs to be authenticated
        //private readonly HttpClient authenticatedClient;
        private readonly PublicClient publicClient;
        private const string endpoint = "api/customer";

        public CustomerService(PublicClient publicClient)
        {
            this.publicClient = publicClient;
        }

        public async Task<CustomerResult.Index> GetIndexAsync(CustomerRequest.Index request)
        {
            var queryParameters = request.GetQueryString();
            var response = await publicClient.Client.GetFromJsonAsync<CustomerResult.Index>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<CustomerResult.Detail> GetDetailAsync(CustomerRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<CustomerResult.Detail>($"{endpoint}/{request.Id}");
            return response;
        }

        public async Task<ProjectResult.Index> GetProjectsFromCustomerAsync(CustomerRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<ProjectResult.Index>($"{endpoint}/{request.Id}/projects");
            return response;
        }

        public async Task<CustomerResult.Create> CreateAsync(CustomerRequest.Create request)
        {
            var response = await publicClient.Client.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<CustomerResult.Create>();
        }

        public async Task<CustomerResult.Edit> EditAsync(CustomerRequest.Edit request)
        {
            var response = await publicClient.Client.PutAsJsonAsync($"{endpoint}/{request.Id}", request);
            return await response.Content.ReadFromJsonAsync<CustomerResult.Edit>();
        }

        public async Task DeleteAsync(CustomerRequest.Delete request)
        {
            await publicClient.Client.DeleteAsync($"{endpoint}/{request.Id}");
        }
    }
}
