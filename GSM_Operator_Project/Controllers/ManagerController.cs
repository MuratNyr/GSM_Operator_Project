using GSM_Operator_Project.Data;
using Microsoft.AspNetCore.Mvc;

namespace GSM_Operator_Project.Controllers
{
    public class ManagerController : Controller
    {
        ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
