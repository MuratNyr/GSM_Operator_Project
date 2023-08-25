using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class Tahsilat
    {
        public int TahsilatID { get; set; }

        [Required]
        public int FaturaID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TahsilTarihi { get; set; }

        [Required]
        public int TarifeID { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal TarifeUcreti { get; set; }
    }
}
