using DirectumRX_beta.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<DbServices>();
builder.Services.AddScoped<LocalFileServices>();
builder.Services.AddScoped<LocalDbServices>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.UseDeveloperExceptionPage();
app.UseAuthorization();
app.UseResponseCaching();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Files}/{action=Public}/id={id}&&pass={password}");

app.Run();
