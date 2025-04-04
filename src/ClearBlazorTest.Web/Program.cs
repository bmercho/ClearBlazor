using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClearBlazorTest.Web;
using ClearBlazorTest;

public class Program
{
    public static async Task Main(string[] args)
    {
        ClientData.LoadTestData();
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<ClearBlazorTest.Web.App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        await builder.Build().RunAsync();
    }
}