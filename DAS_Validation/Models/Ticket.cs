using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAS_Validation.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string UserID { get; set; }
        [Required, MaxLength(50)]
        public string Barcode { get; set; }
        [Required]

        public double PrizeAmount { get; set; }
        [Required]
        public double TaxAmount { get; set; }
        [Required, MaxLength(50)]
        public string ValidationResult { get; set; }
        [Required]
        public DateTime ValidationDate { get; set; }
    }
}
