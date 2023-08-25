using Microsoft.AspNetCore.Mvc;
using GSM_Operator_Project.Data;
using GSM_Operator_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace GSMOperatörleriProjesi.Controllers
{
    public class MusteriController : Controller
    {
        private ApplicationDbContext _context;

        public MusteriController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var indexViewModel = new IndexViewModel
            {
                Musteri = _context.Musteriler.ToList(),
                Tarife = _context.Tarifeler.ToList()
            };

            return View(indexViewModel);
        }


        public IActionResult Detay(int id)
        {
            var musteri = _context.Musteriler.FirstOrDefault(m => m.MusteriID == id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }

        [HttpGet]
        public IActionResult Tarife(int id)
        {
            var musteriTarife = _context.Tarifeler.FirstOrDefault(m => m.TarifeID == id);
            if (musteriTarife == null)
            {
                return NotFound();
            }
            return View(musteriTarife);
        }

        [HttpPost]
        public IActionResult Tarife(int tarifeId,string tarifeName)
        {
            var userTC = Request.Cookies["UserTC"];
            var musteri = _context.Musteriler.FirstOrDefault(m => m.TC == userTC);
            if (musteri != null)
            {
                MusteriTarife musteriTarife = new MusteriTarife
                {
                    MusteriID = musteri.MusteriID,
                    TarifeID = tarifeId,
                    BaslangicTarihi = DateTime.Now,
                    BitisTarihi = DateTime.Now.AddYears(1)
                };
                _context.MusteriTarifeleri.Add(musteriTarife);
                _context.SaveChanges();
                return RedirectToAction("Ekle", "Musteri");
            }

            return RedirectToAction("Index", "Musteri");
        }

        public IActionResult Ekle()
        {
            var userName = Request.Cookies["UserName"];
            var userRole = Request.Cookies["UserRole"];
            if (userRole == "Admin" && !string.IsNullOrEmpty(userName))
            {
                return View();
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult Ekle(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                _context.Musteriler.Add(musteri);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musteri);
        }

        public IActionResult Duzenle(int id)
        {
            var musteri = _context.Musteriler.FirstOrDefault(m => m.MusteriID == id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }

        [HttpPost]
        public IActionResult Duzenle(int id, Musteri musteri)
        {
            if (id != musteri.MusteriID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(musteri);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musteri);
        }

        public IActionResult Sil(int id)
        {
            var musteri = _context.Musteriler.FirstOrDefault(m => m.MusteriID == id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }

        [HttpPost]
        public IActionResult Sil(int id, bool confirm)
        {
            var musteri = _context.Musteriler.FirstOrDefault(m => m.MusteriID == id);
            if (musteri == null)
            {
                return NotFound();
            }

            if (confirm)
            {
                _context.Musteriler.Remove(musteri);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Sil", new { id = id });
        }
    }
}
