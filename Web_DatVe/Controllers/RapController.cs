using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using Web_DatVe.Models.ViewModels;
using System.Data.Entity;
namespace Web_DatVe.Controllers
{
    public class RapController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        public ActionResult Index()
        {
            return View(db.RAPs.ToList());
        }

        public ActionResult ChiTiet(int id)
        {
            var rap = db.RAPs.Find(id);
            if (rap == null) return HttpNotFound();

            var phong = db.PHONGs.Where(p => p.MaRap == id).ToList();
            var phongIds = phong.Select(p => p.MaPhong).ToList();

            var suat = new System.Collections.Generic.List<SUATCHIEU>();
            if (phongIds != null && phongIds.Count > 0)
            {
                suat = db.SUATCHIEUx
                    .Where(s => phongIds.Contains(s.MaPhong))
                    .OrderBy(s => s.ThoiGianBatDau)
                    .ToList();
            }

            RapDetailVM vm = new RapDetailVM
            {
                Rap = rap,
                Phong = phong,
                SuatChieu = suat
            };

            return View(vm);
        }
    }
}
