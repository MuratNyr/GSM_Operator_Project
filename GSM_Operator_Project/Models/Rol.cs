using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class Rol
    {
        [Key]
        public int RolID { get; set; }

        [Required]
        [MaxLength(50)]
        public string RolAd { get; set; }
    }
}
