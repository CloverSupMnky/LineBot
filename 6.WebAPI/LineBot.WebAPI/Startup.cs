using LineBot.Asset.Model.AppSetting;
using LineBot.Entitys;
using LineBot.Entitys.Contexts;
using LineBot.Entitys.Models;
using LineBot.Module.Interface;
using LineBot.Module.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace LineBot.WebAPI
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

            services.AddControllers();

            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            services.Configure<LineBotSetting>(Configuration.GetSection("LineBotSetting"));

            services.AddDbContext<LineBotContext>(options => options.UseNpgsql(Configuration.GetConnectionString("LineBotNpgsql")));

            services.AddScoped<DbContext,LineBotContext>();

            services.AddScoped<IReplyMessageService, ReplyMessageService>();
            services.AddScoped<ILineBotUnitOfWork, LineBotUnitOfWork>();
            services.AddScoped<ITrackableRepository<Person>, TrackableRepository<Person>>();
            services.AddScoped<ITrackableRepository<PersonalLiability>, TrackableRepository<PersonalLiability>>();
            services.AddScoped<ITrackableRepository<RentFixedFee>, TrackableRepository<RentFixedFee>>();
            services.AddScoped<ITrackableRepository<Sysparam>, TrackableRepository<Sysparam>>();
            services.AddScoped<ITrackableRepository<UtilityFee>, TrackableRepository<UtilityFee>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
