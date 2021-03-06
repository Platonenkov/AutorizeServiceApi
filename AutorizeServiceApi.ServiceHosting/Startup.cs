using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutorizeServiceApi.DAL;
using AutorizeServiceApi.DAL.Data;
using AutorizeServiceApi.Domain.Models;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutorizeServiceApi.ServiceHosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(
            //    o =>
            //    {
            //        o.AddPolicy("CorsPolicy", policy =>
            //        {
            //            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //        });
            //    });

            services.AddCors();

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("AutorizeServiceApi.ServiceHosting")));
            services.AddDatabaseDeveloperPageExceptionFilter();


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
           {
               options.SignIn.RequireConfirmedAccount = false;
               options.Password.RequireDigit = false;
               options.Password.RequireLowercase = false;
               options.Password.RequireUppercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequiredLength = 3;
           }).AddEntityFrameworkStores<ApplicationDBContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
            services.AddIdentityServer(
                    o =>
                    {
                        o.UserInteraction.LoginUrl = "/Identification/login";
                        o.UserInteraction.LogoutUrl = "/Identification/logout";
                    })
               .AddApiAuthorization<ApplicationUser, ApplicationDBContext>(
                    o =>
                    {
                        o.Clients = new ClientCollection(new List<Client>(){new Client()
                        {
                            ClientId = "BlazorTestApp",
                            AllowedGrantTypes = GrantTypes.Code,
                            RequireClientSecret = false,
                            RequireConsent = false,
                            RequirePkce = true,
                            AllowedScopes =
                            {
                                IdentityServerConstants.StandardScopes.Address,
                                IdentityServerConstants.StandardScopes.Email,
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile
                            },
                            RedirectUris = {"https://localhost:5001/authentication/login-callback"},
                            PostLogoutRedirectUris = {"https://localhost:5001/authentication/logout-callback"},
                            AllowedCorsOrigins = { "https://localhost:5001" }
                        }});
                        o.ApiResources = new ApiResourceCollection(new List<ApiResource>() { new ApiResource("API", "ServerApi") });
                        o.ApiScopes = new ApiScopeCollection(new List<ApiScope>() { new ApiScope("API") });
                        o.IdentityResources = new IdentityResourceCollection(new List<IdentityResource>()
                        {
                            new IdentityResources.OpenId(),
                            new IdentityResources.Address(),
                            new IdentityResources.Profile(),
                            new IdentityResources.Email()
                        });
                    })
               .AddDefaultEndpoints();

            services.AddIdentityServerBuilder();
            services.AddAuthentication()
               .AddIdentityServerJwt();

            services.AddTransient<AuthentificationContextInitializer>();


            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutorizeServiceApi.ServiceHosting", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthentificationContextInitializer AuthContext)
        {
            app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            AuthContext.InitializeAsync().Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutorizeServiceApi.ServiceHosting v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
