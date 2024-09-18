using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyTransfer.BLL.Dtos;
using MoneyTransfer.BLL.Interfaces;
using MoneyTransfer.BLL.Models;
using MoneyTransfer.Data;
using MoneyTransfer.Data.Entites;
using System.Text.Json;

namespace MoneyTransfer.BLL.Services
{
    public class TransactionInformationService : ITransactionInformationService
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionInformationService> _logger;

        public TransactionInformationService(AppDbContext context, IMapper mapper, ILogger<TransactionInformationService> logger, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task<ResponseModel<TransactionInformation>> CreateTransactionInformation(TransactionInformationDto dto)
        {
            if (dto != null)
            {
                var senderDetails = JsonSerializer.Serialize(dto.SenderDetails);
                var receiverDetails = JsonSerializer.Serialize(dto.ReceiverDetails);
                var paymentDetails = JsonSerializer.Serialize(dto.PaymentDetails);
                var data = new TransactionInformation()
                {
                    Id = dto.Id,
                    SenderDetails = senderDetails,
                    ReceiverDetails = receiverDetails,
                    PaymentDetails = paymentDetails,
                    UserId = dto.UserId!,
                    TransactionDate = dto.TransactionDate
                };

                await _context.Transaction_Information.AddAsync(data);
                await _context.SaveChangesAsync();
                _logger.LogInformation("data saved successfully");
                return new ResponseModel<TransactionInformation>() { Data = data, Message = "Transaction successfull", Success = true };
            }
            _logger.LogInformation("data save failed");
            return new ResponseModel<TransactionInformation>() { Data = null, Message = "Parameter is null", Success = false };
        }

        public async Task<List<TransactionInformationDto>> TransactionReport(string? userId, DateTime? fromDate, DateTime? toDate)
        {
            List<TransactionInformationDto> list = new();

            var query = _context.Transaction_Information.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(p => p.UserId == userId);
            }

            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(p => p.TransactionDate >= Convert.ToDateTime(fromDate) && Convert.ToDateTime(toDate) <= p.TransactionDate);
            }

            var transactionData = await query.ToListAsync();

            if (transactionData.Count > 0)
            {
                foreach (var item in transactionData)
                {
                    var senderDetails = item.SenderDetails != null
                    ? JsonSerializer.Deserialize<SenderDetails>(item.SenderDetails)
                    : null;

                    var receiverDetails = item.ReceiverDetails != null
                    ? JsonSerializer.Deserialize<ReceiverDetails>(item.ReceiverDetails)
                    : null;

                    var paymentDetails = item.PaymentDetails != null
                    ? JsonSerializer.Deserialize<PaymentDetails>(item.PaymentDetails)
                    : null;

                    list.Add(new TransactionInformationDto()
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        SenderDetails = senderDetails,
                        ReceiverDetails = receiverDetails,
                        PaymentDetails = paymentDetails,
                        TransactionDate = item.TransactionDate
                    });
                }
                return list;
            }
            return list;
        }
    }
}
