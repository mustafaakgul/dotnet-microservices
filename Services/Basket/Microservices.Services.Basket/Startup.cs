using Microservices.Services.Basket.Services;
using Microservices.Services.Basket.Settings;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Basket
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
            //kullanıcı gerektiren token icin ayar jtw den kesnlkle sub keyword bekliyoruz artık
            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");  //kendi claimslerde map leme yapıyor bunu iptal etmek cnku sub oalrak msela ersmek istiyecegz kendi ismini degisityor, herseyi map le ama sub key ini mapleme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_basket";
                options.RequireHttpsMetadata = false;
            });

            services.AddHttpContextAccessor(); //shared altındakine eriseblmek iicin
            services.AddScoped<ISharedIdentityService, SharedIdentityService>(); //interface grunce git nesne ornegi al class indan
            services.AddScoped<IBasketService, BasketService>();

            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));  //redissetting class karıslasnca app settingden git ayarları al demek appsetting icindeki node dan al

            //singleton olarak ekledk cnku kalkınca 1 kere yapsın o kdar
            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                var redis = new RedisService(redisSettings.Host, redisSettings.Port);

                redis.Connect();  //metodu yapılanlar baglantı

                return redis;
            });

            //services.AddControllers();
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //authenticate olmus kuullnıcı
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.Basket", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.Basket v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
