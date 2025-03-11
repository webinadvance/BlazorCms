using System.Text.Json.Serialization;
using BlazorApp2.Components;
using BlazorApp2.Data;
using BlazorApp2.Data.Services;
using Microsoft.EntityFrameworkCore;
using _Imports = BlazorApp2.Client._Imports;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Add API controllers with appropriate serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=bookingApp.db";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

// Register services
builder.Services.AddScoped<BookingService>();
var app = builder.Build();

// Initialize the database
await DbInitializer.InitializeAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

// Map controllers BEFORE Razor components
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorComponents<App>()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(_Imports).Assembly);
});
await app.RunAsync();