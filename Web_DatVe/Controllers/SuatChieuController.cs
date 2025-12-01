using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using System.Data.Entity;
namespace Web_DatVe.Controllers
{
    public class SuatChieuController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        public ActionResult ChonSuat(int id)
        {
            var suat = db.SUATCHIEUx
                .Where(s => s.MaPhim == id)
                .OrderBy(s => s.ThoiGianBatDau)
                .ToList();

            return View(suat);
        }
    }
}
