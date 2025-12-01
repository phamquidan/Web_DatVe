using System;
using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using Web_DatVe.Models.ViewModels;
using System.Data.Entity;

namespace Web_DatVe.Controllers
{
    public class PhimController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        public ActionResult Index(string search, string theloai, int? nam, string quocgia)
        {
            var ds = db.PHIMs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                ds = ds.Where(x => x.TenPhim.Contains(search));

            if (!string.IsNullOrEmpty(theloai))
                ds = ds.Where(x => x.TheLoai.Contains(theloai));

            if (nam.HasValue)
                ds = ds.Where(x => x.NgayKhoiChieu.HasValue &&
                                   x.NgayKhoiChieu.Value.Year == nam.Value);

            if (!string.IsNullOrEmpty(quocgia))
                ds = ds.Where(x => x.QuocGia == quocgia);

            // ViewBag filter lists
            ViewBag.TheLoaiList = db.PHIMs
                .Select(x => x.TheLoai)
                .Distinct()
                .ToList();

            ViewBag.NamList = db.PHIMs
                .Where(x => x.NgayKhoiChieu.HasValue)
                .Select(x => x.NgayKhoiChieu.Value.Year)
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();

            ViewBag.QuocGiaList = db.PHIMs
                .Select(x => x.QuocGia)
                .Where(x => x != null && x != "")
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return View(ds.ToList());
        }

        public ActionResult ChiTiet(int id)
        {
            var phim = db.PHIMs.Find(id);
            if (phim == null) return HttpNotFound();

            var suat = db.SUATCHIEUx
                .Where(s => s.MaPhim == id)
                .OrderBy(s => s.ThoiGianBatDau)
                .ToList();

            var vm = new MovieDetailVM
            {
                Phim = phim,
                SuatChieu = suat
            };

            return View(vm);
        }
    }
}
