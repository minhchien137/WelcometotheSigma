using WelcometotheSigma.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddScoped<IVipScheduleService, VipScheduleService>();
builder.Services.AddSingleton<IDisplayConfigService, DisplayConfigService>();
builder.Services.AddSingleton<IVipDisplayStateService, VipDisplayStateService>();
builder.Services.AddHostedService<VipDisplayBackgroundService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
