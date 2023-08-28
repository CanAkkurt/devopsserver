using Persistence.Data;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.VirtualMachines;
using Shared.VirtualMachines;
using Services.Customers;
using Shared.Customers;
using Shared.Projects;
using Shared.Tickets;
using Shared.Members;
using Services.Members;
using Services.Tickets;
using Services.Projects;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MySql.Data.EntityFrameworkCore;


namespace Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<VirtualMachineDto.Create.Validator>();
                config.ImplicitlyValidateChildProperties = true;
            });
            
            
            
            services.AddDbContext<DataContext>(options =>
            {
                
                options.UseMySql(Configuration.GetConnectionString("MysqlData"), new MySqlServerVersion(new Version()));
            });
                    

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.DeclaringType is null ? $"{type.Name}" : $"{type.DeclaringType?.Name}.{type.Name}");
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vic API", Version = "v1" });
            });



            //auth

            services.AddAuthentication(options =>
              {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {   //to do get from apssettings
                options.Authority = Configuration["Auth0:Authority"];
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });
            //
            
            services.AddAuth0AuthenticationClient(config =>
            {  
                config.Domain = Configuration["Auth0:Authority"];
                config.ClientId = Configuration["Auth0:ClientId"];
                config.ClientSecret = Configuration["Auth0:ClientSecret"];
            });
            services.AddAuth0ManagementClient().AddManagementAccessToken();
            // 👆


            services.AddRazorPages();

            services.AddScoped<IVirtualMachineService, VirtualMachineService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<DataInitializer>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataInitializer dataInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vic API"));

                dataInitializer.InitializeData();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            // auth
            app.UseAuthentication();
            app.UseAuthorization();
            //

            app.UseCors(policyName => policyName.WithOrigins("https://localhost:5001")
                .AllowAnyMethod()
                .WithHeaders(HeaderNames.ContentType));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
