using Core.Attributes;
using Core.Common;
using Core.Responses;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Masterdata.Application.Features.V1.Commands.Topic;
using Masterdata.Application.Features.V1.Queries.Question;
using Masterdata.Application.Features.V1.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Masterdata.API
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
            //Global filter
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Masterdata.API", Version = "v1" });
            });

            services.AddDbContext<IOT_SOCKETContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            var origins = Configuration.GetValue<string>("AllowedOrigins").Split(";");
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials() // Cho phép sử dụng các credentials từ origin cụ thể
                        .WithExposedHeaders("Access-Control-Allow-Origin"); // Chỉ định header được tiết lộ
                });
            });

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddMediatR(typeof(CreateTopicCommandHandler).Assembly);

            // SignalR
            services.AddSignalR();

            // Inject UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IQuestionQuery, QuestionQuery>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Masterdata.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChathubService>("/chathub");

            });
        }
    }
}
