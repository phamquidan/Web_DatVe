using System;
using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using System.Data.Entity;

namespace Web_DatVe.Controllers
{
    public class AdminController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        // Kiểm tra quyền admin trước mỗi action
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["MaND"] == null || Session["VaiTro"] == null)
            {
                filterContext.Result = RedirectToAction("Login", "NguoiDung");
                return;
            }

            string vaiTro = Session["VaiTro"].ToString();
            if (vaiTro != "Admin" && vaiTro != "QuanTri")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang quản trị!";
                filterContext.Result = RedirectToAction("Index", "Home");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        /* ------------------- DASHBOARD ------------------- */

        public ActionResult Index()
        {
            ViewBag.SoPhim = db.PHIMs.Count();
            ViewBag.SoRap = db.RAPs.Count();
            ViewBag.SoNguoiDung = db.NGUOIDUNGs.Count();
            ViewBag.SoDonHang = db.DONHANGs.Count();
            return View();
        }

        /* ------------------- QUẢN LÝ PHIM ------------------- */

        public ActionResult Phim()
        {
            return View(db.PHIMs.ToList());
        }

        public ActionResult ThemPhim() => View();

        [HttpPost]
        public ActionResult ThemPhim(PHIM p)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PHIMs.Add(p);
                    db.SaveChanges();
                    TempData["Success"] = "Đã thêm phim thành công!";
                    return RedirectToAction("Phim");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi thêm phim: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
            }
            return View(p);
        }

        public ActionResult SuaPhim(int id)
        {
            return View(db.PHIMs.Find(id));
        }

        [HttpPost]
        public ActionResult SuaPhim(PHIM p)
        {
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Phim");
        }

        public ActionResult XoaPhim(int id)
        {
            var p = db.PHIMs.Find(id);
            db.PHIMs.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Phim");
        }

        /* ------------------- QUẢN LÝ RẠP ------------------- */

        public ActionResult Rap() => View(db.RAPs.ToList());

        public ActionResult ThemRap() => View();

        [HttpPost]
        public ActionResult ThemRap(RAP r)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.RAPs.Add(r);
                    db.SaveChanges();
                    TempData["Success"] = "Đã thêm rạp thành công!";
                    return RedirectToAction("Rap");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi thêm rạp: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
            }
            return View(r);
        }

        public ActionResult SuaRap(int id)
        {
            return View(db.RAPs.Find(id));
        }

        [HttpPost]
        public ActionResult SuaRap(RAP r)
        {
            db.Entry(r).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Rap");
        }

        public ActionResult XoaRap(int id)
        {
            db.RAPs.Remove(db.RAPs.Find(id));
            db.SaveChanges();
            return RedirectToAction("Rap");
        }

        /* ------------------- QUẢN LÝ PHÒNG ------------------- */

        public ActionResult Phong()
        {
            ViewBag.Rap = db.RAPs.ToList();
            return View(db.PHONGs.ToList());
        }

        public ActionResult ThemPhong()
        {
            ViewBag.Rap = db.RAPs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ThemPhong(PHONG p)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PHONGs.Add(p);
                    db.SaveChanges();
                    TempData["Success"] = "Đã thêm phòng thành công!";
                    return RedirectToAction("Phong");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi thêm phòng: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
            }
            ViewBag.Rap = db.RAPs.ToList();
            return View(p);
        }

        public ActionResult SuaPhong(int id)
        {
            ViewBag.Rap = db.RAPs.ToList();
            return View(db.PHONGs.Find(id));
        }

        [HttpPost]
        public ActionResult SuaPhong(PHONG p)
        {
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Phong");
        }

        public ActionResult XoaPhong(int id)
        {
            db.PHONGs.Remove(db.PHONGs.Find(id));
            db.SaveChanges();
            return RedirectToAction("Phong");
        }

        /* ------------------- GHẾ ------------------- */
        public ActionResult Ghe()
        {
            ViewBag.Phong = db.PHONGs.ToList();
            return View(db.GHEs.ToList());
        }

        public ActionResult ThemGhe()
        {
            ViewBag.Phong = db.PHONGs.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ThemGhe(GHE g)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.GHEs.Add(g);
                    db.SaveChanges();
                    TempData["Success"] = "Đã thêm ghế thành công!";
                    return RedirectToAction("Ghe");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi thêm ghế: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
            }
            ViewBag.Phong = db.PHONGs.ToList();
            return View(g);
        }

        public ActionResult SuaGhe(int id)
        {
            ViewBag.Phong = db.PHONGs.ToList();
            return View(db.GHEs.Find(id));
        }

        [HttpPost]
        public ActionResult SuaGhe(GHE g)
        {
            db.Entry(g).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Ghe");
        }

        public ActionResult XoaGhe(int id)
        {
            var g = db.GHEs.Find(id);
            if (g != null)
            {
                db.GHEs.Remove(g);
                db.SaveChanges();
            }
            return RedirectToAction("Ghe");
        }

        /* ------------------- SUẤT CHIẾU ------------------- */
        public ActionResult SuatChieu()
        {
            var suatChieu = db.SUATCHIEUx
                .Include(s => s.PHIM)
                .Include(s => s.PHONG)
                .OrderByDescending(s => s.ThoiGianBatDau)
                .ToList();
            return View(suatChieu);
        }

        /* ------------------- NGƯỜI DÙNG ------------------- */
        public ActionResult NguoiDung()
        {
            return View(db.NGUOIDUNGs.OrderByDescending(n => n.NgayTao).ToList());
        }

        public ActionResult SuaNguoiDung(int id)
        {
            return View(db.NGUOIDUNGs.Find(id));
        }

        [HttpPost]
        public ActionResult SuaNguoiDung(NGUOIDUNG nd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(nd).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = "Đã cập nhật thông tin người dùng thành công!";
                    return RedirectToAction("NguoiDung");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin nhập vào.";
            }
            return View(nd);
        }

        public ActionResult XoaNguoiDung(int id)
        {
            var nd = db.NGUOIDUNGs.Find(id);
            if (nd != null)
            {
                // Không cho xóa admin
                if (nd.VaiTro == "Admin" || nd.VaiTro == "QuanTri")
                {
                    TempData["Error"] = "Không thể xóa tài khoản quản trị!";
                    return RedirectToAction("NguoiDung");
                }

                try
                {
                    db.NGUOIDUNGs.Remove(nd);
                    db.SaveChanges();
                    TempData["Success"] = "Đã xóa người dùng thành công!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
                }
            }
            return RedirectToAction("NguoiDung");
        }

        /* ------------------- ĐƠN HÀNG ------------------- */
        public ActionResult DonHang(string trangThai, string tuNgay, string denNgay)
        {
            var query = db.DONHANGs.Include(d => d.NGUOIDUNG).AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(d => d.TrangThai == trangThai);
            }

            if (!string.IsNullOrEmpty(tuNgay))
            {
                if (DateTime.TryParse(tuNgay, out DateTime tu))
                {
                    query = query.Where(d => d.NgayDat.HasValue && d.NgayDat.Value >= tu);
                }
            }

            if (!string.IsNullOrEmpty(denNgay))
            {
                if (DateTime.TryParse(denNgay, out DateTime den))
                {
                    query = query.Where(d => d.NgayDat.HasValue && d.NgayDat.Value <= den.AddDays(1));
                }
            }

            var donHang = query.OrderByDescending(d => d.NgayDat).ToList();
            return View(donHang);
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = db.DONHANGs
                .Include(d => d.NGUOIDUNG)
                .Include(d => d.VEs)
                .FirstOrDefault(d => d.MaDH == id);

            if (donHang == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("DonHang");
            }

            // Load related data for each ticket
            foreach (var ve in donHang.VEs)
            {
                if (ve.MaSuat > 0)
                {
                    var suat = db.SUATCHIEUx
                        .Include(s => s.PHIM)
                        .Include(s => s.PHONG)
                        .FirstOrDefault(s => s.MaSuat == ve.MaSuat);
                    if (suat != null)
                    {
                        ve.SUATCHIEU = suat;
                        if (suat.PHONG != null)
                        {
                            var rap = db.RAPs.FirstOrDefault(r => r.MaRap == suat.PHONG.MaRap);
                            if (rap != null)
                            {
                                suat.PHONG.RAP = rap;
                            }
                        }
                    }
                }
                if (ve.MaGhe > 0)
                {
                    ve.GHE = db.GHEs.FirstOrDefault(g => g.MaGhe == ve.MaGhe);
                }
            }

            return View(donHang);
        }

        [HttpPost]
        public ActionResult CapNhatTrangThai(int id, string trangThai)
        {
            var donHang = db.DONHANGs.Find(id);
            if (donHang != null)
            {
                try
                {
                    donHang.TrangThai = trangThai;
                    db.SaveChanges();
                    TempData["Success"] = "Đã cập nhật trạng thái đơn hàng thành công!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
                }
            }
            return RedirectToAction("ChiTietDonHang", new { id = id });
        }
    }
}
