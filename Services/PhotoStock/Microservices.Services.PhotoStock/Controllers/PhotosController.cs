using Microservices.Services.PhotoStock.DTOs;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController  // : ControllerBase
    {
        //cancelationtoken olayı su foto yukleme sresinde endpoint islemi snlandırırsa foto yuklemede sonlansın, 1 dakikalık bir foto yukluyor tarayıcıyı kapatırsa islemi kapat foto yklenmeye devam etmesn demek
        //cnku islem asnekron sadece asenkron islemi hata fırlatarak sonlandırabliriz yoksa arka planda thread ile deva meder
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)  
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);  //ilgili scope bitince bellekten dusecek

                var returnPath = photo.FileName; //"photos/" + photo.FileName
              
                PhotoDto photoDto = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400)); //foto null geliyor ise
        }

        //burada asenkron metod yapılmadı cunku Delete metodunun asenrkonu yok sadece senkronu var
        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))  //path varsa yani foto varsa orada
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));  //body bos 204 basarı donecegiz
        }
    }
}
