using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class Tarife
    {
        public int TarifeID { get; set; }

        [Required]
        public string TarifeAdi { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal TarifeUcreti { get; set; }

        public int TarifeDakika { get; set; }

        public int TarifeInternetMb { get; set; }

        public int TarifeSMS { get; set; }
    }
}
