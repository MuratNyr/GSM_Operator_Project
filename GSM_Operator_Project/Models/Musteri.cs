using GSM_Operator_Project.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GSM_Operator_Project.Models
{
    public class Musteri
    {
        public int MusteriID { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "TC alanı zorunludur.")]
        [TCValidation(ErrorMessage = "Geçersiz TC kimlik numarası.")]
        public string TC { get; set; }

        [Required(ErrorMessage = "GSMno alanı zorunludur.")]
        [StringLength(15, ErrorMessage = "Numara alanı en fazla 15 karakter olmalıdır.")]
        //[RegularExpression(@"\+90\d{3}\)\d{3}\s\d{2}\s\d{2}", ErrorMessage = "Geçerli bir telefon numarası formatı giriniz.")]
        public string GSMno { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required]
        public string SifreHash { get; set; }
    }
}
