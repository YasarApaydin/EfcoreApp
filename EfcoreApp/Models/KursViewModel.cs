
using EfcoreApp.Data;
using System.ComponentModel.DataAnnotations;

namespace EfcoreApp.Models
{
    public class KursViewModel
    {
        
        public int KursId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Kurs Başlığı")]
        public string? Baslık { get; set; }
        public int OgretmenId { get; set; }
        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();


    }
}
