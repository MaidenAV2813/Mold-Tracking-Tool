using Microsoft.AspNetCore.Authentication.Cookies;
using Tracking_Tool_System.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ApiService>();

// Razor Pages
builder.Services.AddRazorPages();

// 🔐 Auth + Session
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";   // ajustado a controller
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/Denied";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<ApiService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// 🔥 ORDEN CRÍTICO
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();