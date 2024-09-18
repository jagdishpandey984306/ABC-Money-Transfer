using MoneyTransfer.BLL.Dtos;
using MoneyTransfer.BLL.Models;
using MoneyTransfer.Data.Entites;

namespace MoneyTransfer.BLL.Interfaces
{
    public interface ITransactionInformationService
    {
        Task<ResponseModel<TransactionInformation>> CreateTransactionInformation(TransactionInformationDto dto);
        Task<List<TransactionInformationDto>> TransactionReport(string? userId, DateTime? fromDate, DateTime? toDate);
    }
}
