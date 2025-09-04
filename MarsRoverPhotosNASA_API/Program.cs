using MarsRoverPhotosNASA_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//register the API service
builder.Services.AddHttpClient<MarsRoverService>();
//before the builder.build we implement the memory caching
builder.Services.AddMemoryCache();      //ensure builder utilizes memory cache
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseResponseCaching();       //including caching for faster response times
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MarsRover}/{action=Index}/{id?}");

app.Run();
