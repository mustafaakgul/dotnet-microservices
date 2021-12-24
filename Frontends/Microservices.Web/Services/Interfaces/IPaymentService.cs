using Microservices.Web.Models.FakePayments;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
namespace Microservices.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}
