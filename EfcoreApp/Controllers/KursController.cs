using EfcoreApp.Data;
using EfcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EfcoreApp.Controllers
{
    public class KursController:Controller
    {
        private readonly DataContext _context;
        public  KursController(DataContext context) 
        {
            _context = context;

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad") ;
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.Kurslar.Include(k => k.Ogretmen).ToListAsync();
            return View(kurslar);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursViewModel model)
        {
            if (ModelState.IsValid)
            {
          _context.Kurslar.Add(new Kurs(){ KursId = model.KursId,Baslık=model.Baslık,OgretmenId=model.OgretmenId});
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(model);
           
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kurs = await _context.Kurslar
                                             .Include(t => t.KursKayitlari)
                                             .ThenInclude(t => t.Ogrenci)
                                             .Select(k => new KursViewModel { KursId = k.KursId,Baslık = k.Baslık, OgretmenId=k.OgretmenId,KursKayitlari=k.KursKayitlari})
                                             .FirstOrDefaultAsync(t => t.KursId==id); //sadece ıd ye göre arama yapılır
            //var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(i => i.OgrenciId == id); 
            if (kurs == null)
            {
                return NotFound();
            }

            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(kurs);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel model)

        {if(id != model.KursId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                 _context.Update(new Kurs() {KursId = model.KursId,Baslık = model.Baslık, OgretmenId = model.OgretmenId });
                await _context.SaveChangesAsync();

                }
                catch (DbUpdateException)
                {
                    if(!_context.Kurslar.Any(kurs => kurs.KursId == model.KursId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction("Index");
               
            }
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null){
                return NotFound();
            }
            var kurs = await _context.Kurslar.FindAsync(id);
            if (kurs == null) 
            {
                return NotFound();
            }

            return View(kurs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var kurs = await _context.Kurslar.FindAsync(id);
            if(kurs == null)
            {
                return NotFound();
            }
            _context.Remove(kurs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




    }
}
