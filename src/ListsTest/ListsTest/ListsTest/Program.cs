using ListsTest;
using ListsTest.Components;

var databaseManager = new DatabaseManager();
databaseManager.Start(DatabaseType.SQLite, @"c:\Work\Data\ListsTest.sqlite", string.Empty, string.Empty, string.Empty);
DemoHub.DatabaseManager = databaseManager;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ListsTest.Client._Imports).Assembly);

app.MapHub<DemoHub>("/demoHub");

app.Run();
