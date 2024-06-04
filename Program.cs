

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myte.Models;
using myte.Services;

var builder = WebApplication.CreateBuilder(args);


//1� Adicionar o servi�o de string de conex�o com o servidor db
builder.Services.AddDbContext<AppDbContext>((options) =>
options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
);
//2� Contexto de autentica��o
builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();
//Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddScoped<WbsService>();
builder.Services.AddScoped<RegistroHorasService>();

/***** Primeiro bloco unifica os servi�os para a aplica��o funcionar *****/



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


//3� Adiciona o m�todo que auxilia na aplica��o dos processos de autentica��o de usu�rios para �rea restrita
//UseAuthentication(); � um m�todo
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
