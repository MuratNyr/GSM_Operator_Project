using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class Fatura
    {
        public int FaturaID { get; set; }

        [Required]
        public int MusteriTarifeID { get; set; }

        [Required]
        public string Donem { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Ucret { get; set; }

        public bool Odendi { get; set; }
    }
}
