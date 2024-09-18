using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTransfer.BLL.Dtos;
using MoneyTransfer.BLL.Interfaces;
using MoneyTransfer.Data.Entites;
using System.Text.Json;

namespace MoneyTransfer.Presentation.Controllers
{
    public class TransactionInformationController : Controller
    {
        private readonly ITransactionInformationService _transactionInformation;

        public TransactionInformationController(ITransactionInformationService transactionInformation)
        {
            _transactionInformation = transactionInformation;
        }

        [Authorize]
        public IActionResult Details(TransactionInformation information)
        {
            TransactionInformationDto dto = new();
            var senderDetails = JsonSerializer.Deserialize<SenderDetails>(information.SenderDetails!);
            var receiverDetails = JsonSerializer.Deserialize<ReceiverDetails>(information.ReceiverDetails!);
            var paymentDetails = JsonSerializer.Deserialize<PaymentDetails>(information.PaymentDetails!);

            dto.SenderDetails = senderDetails;
            dto.ReceiverDetails = receiverDetails;
            dto.PaymentDetails = paymentDetails;

            return View(dto);
        }

        [Authorize]
        public async Task<IActionResult> TransactionReport()
        {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            var result = await _transactionInformation.TransactionReport(userId, DateTime.Now, DateTime.Now);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> TransactionReport(DateTime? fromDate, DateTime? toDate)
        {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            var result = await _transactionInformation.TransactionReport(userId, fromDate, toDate);
            return View(result);
        }
    }
}
