using IdentityMVCcore.Models.Domain;
using IdentityMVCcore.Repository;
using IdentityMVCcore.Repository.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add DataBase
var connectionstring = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<ApplicationDBContext>(options=>options.UseSqlServer(connectionstring));

//add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDBContext>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/UserAuthentication/Login");

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=UserAuthentication}/{action=Login}/{id?}");

app.Run();
