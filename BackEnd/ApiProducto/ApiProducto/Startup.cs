using ApiProducto.Data;
using ApiProducto.Helpers;
using ApiProducto.ProductoMapper;
using ApiProducto.Repository;
using ApiProducto.Repository.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace ApiProducto
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
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Conexion")));
			services.AddAutoMapper(typeof(ProductosMappers));
			services.AddScoped<ICategoriaRepository, CategoriaRepository>();
			services.AddScoped<IProductoRepository, ProductoRepository>();
			services.AddControllers();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("ApiProducto", new Microsoft.OpenApi.Models.OpenApiInfo()
				{
					Title = "API Productos",
					Version = "1.0",
					Description = "Backend Productos",
					Contact = new Microsoft.OpenApi.Models.OpenApiContact()
					{
						Email = "jtatianasd@gmail.com",
						Name = "Tatiana Salamanca",

					}
				});
				var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
				options.IncludeXmlComments(rutaApiComentarios);
			});
			services.AddCors();
			//services.AddCors(options=>options.AddPolicy("AllowWebApp",builder=>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
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
				app.UseExceptionHandler(builder =>
				{
					builder.Run(async context =>
					{
						context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						var error = context.Features.Get<IExceptionHandlerFeature>();

						if (error != null)
						{
							context.Response.AddApplicationError(error.Error.Message);
							await context.Response.WriteAsync(error.Error.Message);
						}

					});
				});
			}
			//app.UseCors();
			app.UseCors(options =>
			{
				options.WithOrigins("*");
				options.AllowAnyMethod();
				options.AllowAnyHeader();
			});
			app.UseHttpsRedirection();
			app.UseSwagger();
			
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/ApiProducto/swagger.json", "API Productos");
				options.RoutePrefix = "";
			});
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
