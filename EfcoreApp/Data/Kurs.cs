﻿namespace EfcoreApp.Data
{
    public class Kurs
    {
     
        public int KursId { get; set; }
        public string? Baslık { get; set; }
        public int  OgretmenId { get; set; }
        public Ogretmen Ogretmen { get; set; } = null!;

        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();
    }
}