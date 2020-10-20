using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Inmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmobiliaria
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>//el sitio web valida con cookie
                {
                                options.LoginPath = "/Usuario/Login";
                                options.LogoutPath = "/Usuario/Logout";
                                options.AccessDeniedPath = "/Home/Restringido";
                            });
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
            });
            services.AddTransient<IRepositorio<Propietario>, RepositorioPropietario>();
            services.AddTransient<IRepositorioPropietario, RepositorioPropietario>();
            services.AddTransient<IRepositorioInquilino, RepositorioInquilino>();
            services.AddTransient<IRepositorioInmueble, RepositorioInmueble>();
            services.AddTransient<IRepositorio<Usuario>, RepositorioUsuario>();
            services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
            services.AddTransient<IRepositorioPago, RepositorioPago>();
            services.AddTransient<IRepositorioContrato, RepositorioContrato>();
            services.AddDbContext<DataContext>(
                 options => options.UseSqlServer(
                     Configuration["ConnectionStrings:DefaultConnection"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Uso de archivos estáticos (*.html, *.css, *.js, etc.)
            app.UseStaticFiles();

            app.UseRouting();

            // Permitir cookies
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });

            // Habilitar autenticación
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("login", "login/{**accion}", new { controller = "Usuario", action = "Login" });
            });
        }
    }
}
