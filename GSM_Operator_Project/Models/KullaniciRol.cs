using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class KullaniciRol
    {
        [Key]
        public int Id { get; set; }
        public int KullaniciID { get; set; }
        public int RolID { get; set; }

        public Musteri Kullanici { get; set; }
        public Rol Rol { get; set; }
    }
}
