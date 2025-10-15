using JMayer.Example.ASPMVC;
using JMayer.Example.ASPMVC.DataLayers;

var builder = WebApplication.CreateBuilder(args);

#region Setup Database, Data Layers & Logging

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

WorkOrderExampleBuilder exampleBuilder = new();
exampleBuilder.Build();

//Add the data layers. Because the example data needs to be built before registration and the data
//layers are memory based, the data layer objects aren't being built with the middleware.
builder.Services.AddSingleton<IWorkOrderDataLayer, WorkOrderDataLayer>(factory => (WorkOrderDataLayer)exampleBuilder.WorkOrderDataLayer);

#endregion

#region Setup Services

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    //Syncfusion is expecting pascal case.
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

#endregion

var app = builder.Build();

#region Setup App

//Register Syncfusion license
using StreamReader streamReader = new("C:\\GitHub\\Syncfusion License Key.txt");
string licenseKey = streamReader.ReadToEnd();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();

//Used to expose the launching of the web application to xunit using WebApplicationFactory.
public partial class Program { }
