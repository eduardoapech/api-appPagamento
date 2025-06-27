using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PagamentosApp.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;


namespace PagamentosApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Aqui muda para PostgreSQL com variável de ambiente
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql("Host=localhost;Port=5432;Database=controledepagamentosdb;Username=postgres;Password=123456"));


            services.AddCors(opt =>
                opt.AddPolicy("AllowAll", b =>
                    b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors("AllowAll");

            // 🔽 Aqui você expõe a pasta Uploads para acesso externo
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadsPath),
                RequestPath = "/uploads"
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
