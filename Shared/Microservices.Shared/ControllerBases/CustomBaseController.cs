using Microservices.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)  //ObjectResult  donelimki o herseyi alablir cnku icine basarız dinamik e lazımsa
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
