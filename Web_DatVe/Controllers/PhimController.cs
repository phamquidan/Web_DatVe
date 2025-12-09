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

        public ActionResult Index(string search, string theloai, int? nam, string quocgia, int page = 1, int pageSize = 12)
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

            // Phân trang
            var pagedResult = GetPaged(ds.OrderByDescending(x => x.NgayKhoiChieu), page, pageSize);
            
            // Giữ lại các tham số filter trong ViewBag để hiển thị lại trong form
            ViewBag.Search = search;
            ViewBag.TheLoai = theloai;
            ViewBag.Nam = nam;
            ViewBag.QuocGia = quocgia;

            return View(pagedResult);
        }

        private PagingModel<PHIM> GetPaged(IQueryable<PHIM> query, int pageNumber, int pageSize)
        {
            var result = new PagingModel<PHIM>();

            result.CurrentPage = pageNumber;
            result.PageSize = pageSize;
            result.RowCount = query.Count();
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (pageNumber - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public ActionResult ChiTiet(int id)
        {
            var phim = db.PHIMs.Find(id);
            if (phim == null) return HttpNotFound();

            var suat = db.SUATCHIEUx
                .Where(s => s.MaPhim == id)
                .OrderBy(s => s.ThoiGianBatDau)
                .ToList();

            // Lấy danh sách phim liên quan
            // Ưu tiên cùng thể loại, khác phim hiện tại; nếu không có thì lấy các phim mới nhất khác bất kỳ
            var query = db.PHIMs.Where(p => p.MaPhim != id);

            if (!string.IsNullOrWhiteSpace(phim.TheLoai))
            {
                string tl = phim.TheLoai.Trim();
                query = query.Where(p =>
                    p.TheLoai != null &&
                    (p.TheLoai.Contains(tl) || tl.Contains(p.TheLoai)));
            }

            var phimLienQuan = query
                .OrderByDescending(p => p.NgayKhoiChieu)
                .Take(4)
                .ToList();

            // Nếu sau khi lọc thể loại không có phim nào, lấy 4 phim bất kỳ (trừ phim hiện tại)
            if (phimLienQuan.Count == 0)
            {
                phimLienQuan = db.PHIMs
                    .Where(p => p.MaPhim != id)
                    .OrderByDescending(p => p.NgayKhoiChieu)
                    .Take(4)
                    .ToList();
            }

            var vm = new MovieDetailVM
            {
                Phim = phim,
                SuatChieu = suat,
                PhimLienQuan = phimLienQuan
            };

            return View(vm);
        }
    }
}
