using EfcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EfcoreApp.Controllers
{
    public class KursKayitController: Controller
    {
        private readonly DataContext _context;
        public  KursKayitController(DataContext context)
        {
            _context = context ;

        }

      

  public async Task<IActionResult> Index()
        {
            var kurskayitlari = await _context.KursKayitlari.Include(m => m.Ogrenci).Include(m => m.Kurs).ToListAsync();
            return View(kurskayitlari);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Ogrenciler = new SelectList(await _context.Ogrenciler.ToListAsync(),"OgrenciId", "AdSoyad");
            ViewBag.Kurslar = new SelectList(await _context.Kurslar.ToListAsync(), "KursId", "Baslık");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursKayit model) 
        {
            model.KayitTarihi = DateTime.Now;
            _context.KursKayitlari.Add(model);
           await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
