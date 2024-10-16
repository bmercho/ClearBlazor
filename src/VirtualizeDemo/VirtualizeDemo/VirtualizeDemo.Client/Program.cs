using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VirtualizeDemo;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        var baseAddress = builder.HostEnvironment.BaseAddress;

        await new SignalRClient().Initialise(baseAddress);

        await builder.Build().RunAsync();
    }
}