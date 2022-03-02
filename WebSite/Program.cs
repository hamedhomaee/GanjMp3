using WebSite.Data;
using Microsoft.EntityFrameworkCore;
using WebSite.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("CustomCookieAuthentication").AddCookie("CustomCookieAuthentication", options => 
    {
        options.Cookie.Name = "CustomCookieAuthentication";
        options.Events.OnRedirectToLogin = async (context) => 
        {
            await Task.FromResult(0);
            context.HttpContext.Response.Redirect("/?auth=failed");
        };
    });

builder.Services.AddTransient<IPasswordHasher, SimplePasswordHasher>();

builder.Services.AddDataProtection();

builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

builder.Services.AddTransient<IMp3Editor, Mp3Editor>();

builder.Services.AddScoped<IUserSignIn, FirstUserSignIn>();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<SiteDbContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:SQLite"]));

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();