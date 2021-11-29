using Microservices.Services.Catalog.Services;
using Microservices.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];  //bu apple token tagtmakta grevli bu
                options.Audience = "resource_catalog";  //tokenin icindeki resource olmalu  aud parmetrelere bakacak bu varmı
                options.RequireHttpsMetadata = false;  //default https bekler bunu pasif yapalım
            });
            //farklı uyelik sistemleri girerese bayiler ve normal uselar iin schema kullanılır

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();

            services.AddAutoMapper(typeof(Startup));

            //eskiden services.AddControllers(); boyleydi ancak methodlara tokensiz istemedigimzden butun controllerlara uygulansın diye evrimlestirecegiz
            //asagıdaki sayesnden tum contorllerlar authorize ile islendi
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());
            });

            //Datalar Configuration uzernden gelecek getsection method ile
            //bunu startp gectigimzden snra herhangi bir ifade ile artık alttaki satiri kulanabliriz
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;  //databasesetingsden degerii al ve return ile don
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.Catalog", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.Catalog v1"));
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
