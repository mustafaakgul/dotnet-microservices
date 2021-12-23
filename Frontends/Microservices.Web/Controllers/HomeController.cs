using Microservices.Web.Exceptions;
using Microservices.Web.Models;
using Microservices.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        /*public IActionResult Index()
        {
            return View();
        }*/

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourseAsync());
        }

        
         public async Task<IActionResult> Detail(string id)
        {
            return View(await _catalogService.GetByCourseId(id));
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();  //hata oldgunda hatayi yakalamak

            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)  //unexcepition tokenden dolayı olan hatayı ele almak icin olursa logine ynlendr cıkısa ynlendryrsn oda logine  bu senaryo ancak uygulamaya 60 gun grmesse olur
            {
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            }//uygulamayı developmenta srekli acıp kapıyoruz refresh tokenleri ucuruyoruz ama normalde canlida server ayaga kalkınca db de refreshleri tutarız ucmaz

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

        public IActionResult Privacy()
        {
            return View();
        }
        /*
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
