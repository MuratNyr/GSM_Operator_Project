using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class MusteriTarife
    {
        public int MusteriTarifeID { get; set; }

        [Required]
        public int MusteriID { get; set; }

        [Required]
        public int TarifeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BaslangicTarihi { get; set; }

        [DataType(DataType.Date)]
        public DateTime BitisTarihi { get; set; }

        //public Musteri Musteri { get; set; }
        //public Tarife Tarife { get; set; }
    }
}
