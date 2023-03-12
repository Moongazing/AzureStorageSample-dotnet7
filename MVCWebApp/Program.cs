using TAO.AzureStorage;
using TAO.AzureStorage.Services.Abstract;
using TAO.AzureStorage.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


ConnectionStrings.AzureStorageConnectionsString = builder.Configuration.GetSection("AzureConnectionsStrings")["StorageCloudConnectionString"];

builder.Services.AddScoped(typeof(INoSqlStorage<>), typeof(TableStorage<>)); 


builder.Services.AddSingleton<IBlobStorage,BlobStorage>();
var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
