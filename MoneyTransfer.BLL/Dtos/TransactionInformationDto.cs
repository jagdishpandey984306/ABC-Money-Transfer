using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MoneyTransfer.BLL.Dtos
{
    public class TransactionInformationDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public SenderDetails? SenderDetails { get; set; }

        public ReceiverDetails? ReceiverDetails { get; set; }

        public PaymentDetails? PaymentDetails { get; set; }

        public string? UserId { get; set; }
        public DateTime? TransactionDate { get; set; } = DateTime.Now;

        public List<SelectListItem>? countryList { get; set; }
    }

    public class SenderDetails
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; } = "";
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }


    }

    public class ReceiverDetails
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Please at least one country")]
        public string? Country { get; set; }
    }

    public class PaymentDetails
    {
        [Required(ErrorMessage = "Bank Name is required")]
        public string? Bank_Name { get; set; }
        [Required(ErrorMessage = "Account Number is required")]
        public string? Account_Number { get; set; }
        public string? Transfer_Currency_Type { get; set; }
        public float? Transfer_Amount { get; set; }
        public float? Exchange_Rate { get; set; }
        public string? Payout_Currency_Type { get; set; }
        public float? Payout_Amount { get; set; }
        public int? Unit { get; set; }
    }
}
