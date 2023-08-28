using Client.Extensions;
using Client.Infrastructure;
using Shared.Customers;
using Shared.Tickets;
using System.Net.Http.Json;

namespace Client.Shared.Services
{
    public class TicketService : ITicketService
    {
        //private readonly HttpClient authenticatedClient;
        private readonly PublicClient publicClient;
        private const string endpoint = "api/ticket";

        public TicketService(PublicClient publicClient)
        {
            this.publicClient = publicClient;
        }

        public async Task<TicketResult.Index> GetIndexAsync(TicketRequest.Index request)
        {
            var queryParameters = request.GetQueryString();
            var response = await publicClient.Client.GetFromJsonAsync<TicketResult.Index>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<TicketResult.Detail> GetDetailAsync(TicketRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<TicketResult.Detail>($"{endpoint}/{request.Id}");
            return response;
        }

        public async Task<TicketResult.Create> CreateAsync(TicketRequest.Create request)
        {
            var response = await publicClient.Client.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<TicketResult.Create>();
        }

        public async Task<TicketResult.Edit> EditAsync(TicketRequest.Edit request)
        {
            var response = await publicClient.Client.PutAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<TicketResult.Edit>();
        }

        public async Task DeleteAsync(TicketRequest.Delete request)
        {
            await publicClient.Client.DeleteAsync($"{endpoint}/{request.Id}");
        }
    }
}
