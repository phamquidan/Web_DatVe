using System;
using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using Web_DatVe.Models.ViewModels;
using System.Data.Entity;
namespace Web_DatVe.Controllers
{
    public class HomeController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        public ActionResult Index()
        {
            var dangChieu = db.PHIMs
                .Where(p => p.NgayKhoiChieu <= DateTime.Now)
                .OrderByDescending(p => p.NgayKhoiChieu)
                .Take(8)
                .ToList();

            var sapChieu = db.PHIMs
                .Where(p => p.NgayKhoiChieu > DateTime.Now)
                .OrderBy(p => p.NgayKhoiChieu)
                .Take(8)
                .ToList();

            HomeVM vm = new HomeVM
            {
                PhimDangChieu = dangChieu,
                PhimSapChieu = sapChieu
            };

            return View(vm);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
