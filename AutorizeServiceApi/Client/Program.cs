using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutorizeServiceApi.Domain.Helpers;
using AutorizeServiceApi.Domain.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration.Memory;

namespace AutorizeServiceApi.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<CustomStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton(async p =>
            {
                var httpClient = p.GetRequiredService<HttpClient>();
                return await httpClient.GetJsonAsync<AutorizeServiceApiSettings>("api/AutorizeServiceApiSettings");
            });

            HttpClient client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), Timeout = TimeSpan.FromMinutes(5) };
            var setting = await client.GetJsonAsync<AutorizeServiceApiSettings>("api/AutorizeServiceApiSettings");


            var config = new Dictionary<string, string> { { "AuthorizeServiceAddress", setting.AuthorizeServiceAddress } };
            builder.Configuration.Add(new MemoryConfigurationSource { InitialData = config });
            //var config = new Dictionary<string, string> { { "AuthorizeServiceAddress", "http://localhost:60777" } };
            //builder.Configuration.Add(new MemoryConfigurationSource { InitialData = config });
            await builder.Build().RunAsync();
        }
    }
}
