using AutoHub.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var CS = builder.Configuration.GetConnectionString(builder.Environment.IsDevelopment() ? "Local" : "OnLine");
builder.Services.AddDbContext<MainDbContext>(options => options.UseSqlServer(CS));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var db = serviceScope?.ServiceProvider.GetService<MainDbContext>();
if (db != null)
{
    db.Database.Migrate();
    DbSeeder.SeedAdminUser(db);
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
