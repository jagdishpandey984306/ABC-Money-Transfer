using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTransfer.Data.Entites
{
    [Table("Transaction_Information")]
    public class TransactionInformation
    {
        public Guid Id { get; set; }
        [Required]
        public string? SenderDetails { get; set; }
        [Required]
        public string? ReceiverDetails { get; set; }
        [Required]
        public string? PaymentDetails { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser? AppUser { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
