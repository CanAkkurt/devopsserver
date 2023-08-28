using Client.Extensions;
using Client.Infrastructure;
using Shared.Customers;
using Shared.Tickets;
using Shared.VirtualMachines;
using System.Net.Http.Json;

namespace Client.Shared.Services
{
    public class VirtualMachineService : IVirtualMachineService
    {
        //private readonly HttpClient authenticatedClient;
        private readonly PublicClient publicClient;
        private const string endpoint = "api/virtualmachine";

        public VirtualMachineService(PublicClient publicClient)
        {
            this.publicClient = publicClient;
        }

        public async Task<VirtualMachineResult.Index> GetIndexAsync(VirtualMachineRequest.Index request)
        {
            var queryParameters = request.GetQueryString();
            var response = await publicClient.Client.GetFromJsonAsync<VirtualMachineResult.Index>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<VirtualMachineResult.Detail> GetDetailAsync(VirtualMachineRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<VirtualMachineResult.Detail>($"{endpoint}/{request.Id}");
            return response;
        }

        public async Task<TicketResult.Index> GetTicketsFromVirtualMachineAsync(VirtualMachineRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<TicketResult.Index>($"{endpoint}/{request.Id}/tickets");
            return response;
        }
        public async Task<CustomerResult.Detail> GetCustomerFromVirtualMachineAsync(VirtualMachineRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<CustomerResult.Detail>($"{endpoint}/{request.Id}/customer");
            return response;
        }

        public async Task<VirtualMachineResult.Create> CreateAsync(VirtualMachineRequest.Create request)
        {
            var response = await publicClient.Client.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<VirtualMachineResult.Create>();
        }

        public async Task<VirtualMachineResult.Edit> EditAsync(VirtualMachineRequest.Edit request)
        {
            var response = await publicClient.Client.PutAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<VirtualMachineResult.Edit>();
        }

        public async Task DeleteAsync(VirtualMachineRequest.Delete request)
        {
            await publicClient.Client.DeleteAsync($"{endpoint}/{request.Id}");
        }

        public async Task<VirtualMachineResult.Usage> GetUsageAsync()
        {
            var response = await publicClient.Client.GetFromJsonAsync<VirtualMachineResult.Usage>($"{endpoint}/analytics");
            return response;
        }
    }
}
