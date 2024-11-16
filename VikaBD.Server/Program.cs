
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VikaBD.Server.Context;

namespace VikaBD.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile(
                Path.Combine(AppContext.BaseDirectory, "appsettings.private.json")
                , optional: false
                , reloadOnChange: true);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin();
                        policy.AllowAnyMethod();
                        policy.WithExposedHeaders("Content-Disposition");
                    });
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VikaBirthday",
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddHttpClient();

            builder.Services.Configure<DbConnectOptions>(builder.Configuration.GetSection("NpgSql"));
            builder.Services.AddDbContext<DataContext>(opt
                => opt.UseNpgsql(builder.Configuration["NpgSql:ConnectionString"]));

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseSwagger(opt =>
            //{
            //    if (builder.Environment.IsProduction())
            //    {
            //        opt.PreSerializeFilters.Add((swagger, httpReq) =>
            //        {
            //            //var serverUrl = $"{httpReq.Scheme}://{httpReq.Host}/indastrial-holding-api/";
            //            var serverUrl = $"https://{httpReq.Host}/indastrial-holding-api/";
            //            swagger.Servers = new List<OpenApiServer> {
            //                    new() { Url = serverUrl } };
            //        });
            //    }
            //});
            //app.UseSwaggerUI(opt =>
            //{
            //    if (builder.Environment.IsDevelopment())
            //        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //    else
            //        opt.SwaggerEndpoint("/indastrial-holding-api/swagger/v1/swagger.json", "v1");
            //});


            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            //app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
