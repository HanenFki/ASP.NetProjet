using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projet2.Models;
using projet2.Models.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceDBConnection")));

builder.Services.AddScoped<ICategorieRepository<Categorie>, SqlCategorieRepository>();
builder.Services.AddScoped<IOrderRepository<Order>, SqlOrderRepository>();
builder.Services.AddScoped<IProductRepository<Produit>, SqlProductRepository>();
builder.Services.AddScoped<ICartRepository, SqlCartRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
	// Default Password settings.
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
});
builder.Services.AddSession(options =>
{
	options.Cookie.IsEssential = true; // make the session cookie essential
});

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
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
