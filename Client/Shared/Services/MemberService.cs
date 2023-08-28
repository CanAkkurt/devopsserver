using Client.Extensions;
using Client.Infrastructure;
using Shared.Customers;
using Shared.Members;
using Shared.VirtualMachines;
using System.Net.Http.Json;

namespace Client.Shared.Services
{
    public class MemberService : IMemberService
    {
        //private readonly HttpClient authenticatedClient;
        private readonly PublicClient publicClient;
        private const string endpoint = "api/member";

        public MemberService(PublicClient publicClient)
        {
            this.publicClient = publicClient;
        }

        public async Task<MemberResult.Index> GetIndexAsync(MemberRequest.Index request)
        {
            var queryParameters = request.GetQueryString();
            var response = await publicClient.Client.GetFromJsonAsync<MemberResult.Index>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<MemberResult.Detail> GetDetailAsync(MemberRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<MemberResult.Detail>($"{endpoint}/{request.Id}");
            return response;
        }

        public async Task<VirtualMachineResult.Index> GetVirtualMachinesFromMemberAsync(MemberRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<VirtualMachineResult.Index>($"{endpoint}/{request.Id}/vms");
            return response;
        }

        public async Task<MemberResult.Create> CreateAsync(MemberRequest.Create request)
        {
            var response = await publicClient.Client.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<MemberResult.Create>();
        }

        public async Task<MemberResult.Edit> EditAsync(MemberRequest.Edit request)
        {
            var response = await publicClient.Client.PutAsJsonAsync($"{endpoint}/{request.Id}", request);
            //var response = await publicClient.Client.PutAsJsonAsync(endpoint, request);
            
            return await response.Content.ReadFromJsonAsync<MemberResult.Edit>();
        }

        public async Task DeleteAsync(MemberRequest.Delete request)
        {
            await publicClient.Client.DeleteAsync($"{endpoint}/{request.Id}");
        }
    }
}
