using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Shared.Customers;
using Shared.VirtualMachines;
using Shared.Members;
using MudBlazor.Services;
using Shared.Projects;
using Shared.Tickets;
using Client.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Client.Shared.Services;
using Client.Infrastructure;
using Append.Blazor.Sidepanel;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Sidepanel
builder.Services.AddSidepanel();

//MudBlazor
builder.Services.AddMudServices();

//Basis
builder.Services.AddHttpClient<PublicClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

//auth







builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

//Add Bogus/Fake DATA to the project scope
builder.Services.AddScoped<IVirtualMachineService, VirtualMachineService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITicketService, TicketService>();


//For authorization
// builder.Services.AddSingleton<FakeAuthenticationProvider>();
// builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<FakeAuthenticationProvider>());


await builder.Build().RunAsync();
