using Microservices.Shared.Services;
using Microservices.Web.Handler;
using Microservices.Web.Helpers;
using Microservices.Web.Models;
using Microservices.Web.Services;
using Microservices.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web
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
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));  //bunu apsettingden elde edip classda consturctrnde elde etmek icin IOptions olarak

            services.AddHttpContextAccessor(); //bunu signin de contrustr olarak kkllndigimzdan eklerz

            services.AddAccessTokenManagement();  //bu iclientcredentialrtoken de olusturulanı gecmeye yarar

            services.AddSingleton<PhotoHelper>();

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();  //ocntructorlarda gecen yapı burda tnmlanmalı ki onu grnce nesnesini uretsin

            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();

            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            services.AddHttpClient<IIdentityService, IdentityService>(); //bunu signin de contrustr olarak kkllndigimzdan eklerz orada new ile uretmek yerine uygulama dnsun

            //serviceapi settingsden classndan json dosyasını mapliyor oradan alıyoruz
            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");   //gatewaybaseuri ve sonrasını adım adım ekle nerey vurcaz http://localhost:5000/services/catalog demek bu satır
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();   //AddHttpMessageHandler<ClientCredentialTokenHandler>()   //normalde burada delege yok delegeyi buraya ekliyoruzki bu url vururken gitsin tokeni alıp tokenle birlikte vursun diye user greekyirmeyen yapı
            //ustte eskiden .AddHttpMessageHandler<ClientCredentialTokenHandler>(); yoktu bu ekleme ile catalog da gidip kendi token alacak

            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            //user servicede contructor olarak di oarak kullanıldıgı icin onu grunce buradan drek olustursun 
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
            //userservide bi istek baslatıldıgında git resource classını calıstır demek

            
            //altta addcookie dedigi icin mvc tarfnda cookie bazlı kmlk dogrulama yetklndrme var ama servis traflarnda microsservics addjwt dendiginzden token bazlı kmlk dogrulama
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                opts.LoginPath = "/Auth/SignIn";
                opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                opts.SlidingExpiration = true;
                opts.Cookie.Name = "udemywebcookie";
            });

            services.AddControllersWithViews();
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
                //burasi cnalida calsicak exception olayinida cozeriz bu sayede canlida sayfa direk gozukecek hata almıcaz ama developmentta hatayı alacagız 1 saat gecirirsek tokeni ama dev de zten bunu alalım ama canlıda almayalım olay bu olmalı
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
