using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ListsTest;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        var baseAddress = builder.HostEnvironment.BaseAddress;

        await new SignalRClient().Initialise(baseAddress);

        ClientData.LoadTestData();

        await builder.Build().RunAsync();
    }
}