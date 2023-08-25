using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSM_Operator_Project.Data;
using GSM_Operator_Project.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GSM_Operator_Project.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            var userName = Request.Cookies["UserName"];
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }
            return RedirectToAction("Index", "Musteri");
        }

        [HttpPost]
        public IActionResult SignUp(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                var musteriVarmi = _context.Musteriler.FirstOrDefault(m => m.TC == musteri.TC);
                if (musteriVarmi != null)
                {
                    ModelState.AddModelError("TC", "Bu TC kimlik numarasıyla zaten bir kayıt bulunmaktadır.");
                    return View("SignUp", musteri);
                }

                var passwordHasher = new PasswordHasher<Musteri>();
                var hashedPassword = passwordHasher.HashPassword(musteri, musteri.SifreHash);

                var yeniMusteri = new Musteri
                {
                    Ad = musteri.Ad,
                    Soyad = musteri.Soyad,
                    TC = musteri.TC,
                    GSMno = musteri.GSMno,
                    Email = musteri.Email,
                    SifreHash = hashedPassword
                };

                _context.Musteriler.Add(yeniMusteri);
                _context.SaveChanges();

                var normalUserRole = _context.Roller.FirstOrDefault(r => r.RolAd == "User");
                var kullaniciRolu = new KullaniciRol
                {
                    KullaniciID = yeniMusteri.MusteriID,
                    RolID = normalUserRole.RolID,
                    Kullanici = musteri,
                    Rol = normalUserRole
                };
                _context.KullaniciRoller.Add(kullaniciRolu);
                _context.SaveChanges();

                return RedirectToAction("Login", "Account");
            }

            return View("SignUp", musteri);
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("UserTC");
            Response.Cookies.Delete("UserRole");

            return RedirectToAction("Index", "Musteri");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Musteri musteri)
        {
            var user = await _context.Musteriler.FirstOrDefaultAsync(x => x.TC == musteri.TC);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<Musteri>();
                var result = passwordHasher.VerifyHashedPassword(user, user.SifreHash, musteri.SifreHash);

                var kullaniciRoller = await _context.KullaniciRoller
                .Where(kr => kr.KullaniciID == user.MusteriID)
                .Join(_context.Roller, kr => kr.RolID, r => r.RolID, (kr, r) => r.RolAd)
                .ToListAsync();

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, musteri.TC)
                    };

                    var userIdentity = new ClaimsIdentity(claims, "Admin");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);

                    Response.Cookies.Append("UserTC", musteri.TC);
                    Response.Cookies.Append("UserName", user.Ad);
                    Response.Cookies.Append("UserRole", kullaniciRoller[0]);

                    return RedirectToAction("Index", "Musteri");

                }
            }
            return View();
        }
    }
}
