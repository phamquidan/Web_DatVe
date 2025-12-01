using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_DatVe.Models;
using Web_DatVe.Models.ViewModels;
using System.Data.Entity;
namespace Web_DatVe.Controllers
{
    public class GheController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new  QL_DatVeXemPhimEntities1();

        public ActionResult ChonGhe(int id) // id = MaSuat
        {
            var suat = db.SUATCHIEUx.Find(id);
            var phong = db.PHONGs.Find(suat.MaPhong);

            var ghe = db.GHEs
                .Where(g => g.MaPhong == phong.MaPhong)
                .OrderBy(g => g.DayGhe)
                .ThenBy(g => g.SoGhe)
                .ToList();

            var gheDaDat = db.VEs.Where(v => v.MaSuat == id)
                .Select(v => v.MaGhe)
                .ToList();

            SeatMapVM vm = new SeatMapVM
            {
                Suat = suat,
                Phong = phong,
                Ghe = ghe,
                GheDaDat = gheDaDat
            };

            // View chính hiện tại ở Views/Ghe/Index.cshtml
            return View("Index", vm);
        }

        [HttpPost]
        public ActionResult DatGhe(int id, string seatIds)
        {
            // Lưu id vào biến local để dùng trong catch block
            int maSuat = id;

            if (Session["MaND"] == null) return RedirectToAction("Login", "NguoiDung");

            // Kiểm tra và parse an toàn seatIds
            if (string.IsNullOrWhiteSpace(seatIds))
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một ghế.";
                return RedirectToAction("ChonGhe", new { id = maSuat });
            }

            var seats = new List<int>();
            foreach (var item in seatIds.Split(','))
            {
                var trimmed = item.Trim();
                if (int.TryParse(trimmed, out int seatId))
                {
                    seats.Add(seatId);
                }
            }

            if (seats.Count == 0)
            {
                TempData["Error"] = "Không có ghế hợp lệ được chọn.";
                return RedirectToAction("ChonGhe", new { id = maSuat });
            }

            int mand = (int)Session["MaND"];
            var suat = db.SUATCHIEUx.Find(maSuat);
            if (suat == null)
            {
                TempData["Error"] = "Suất chiếu không tồn tại.";
                return RedirectToAction("Index", "Phim");
            }

            // Lấy danh sách ghế đã có trong giỏ hàng của user này (cùng suất chiếu)
            var gheTrongGioHang = db.GIOHANGs
                .Where(gh => gh.MaND == mand && gh.MaSuat == maSuat)
                .Select(gh => gh.MaGhe)
                .ToList();

            var gheMoi = new List<int>();
            var gheTrung = new List<int>();

            foreach (var ghe in seats)
            {
                // Kiểm tra xem ghế đã có trong giỏ hàng chưa
                if (gheTrongGioHang.Contains(ghe))
                {
                    gheTrung.Add(ghe);
                }
                else
                {
                    // Kiểm tra xem ghế đã được đặt (trong bảng VE) chưa
                    var daDat = db.VEs.Any(v => v.MaSuat == maSuat && v.MaGhe == ghe);
                    if (!daDat)
                    {
                        gheMoi.Add(ghe);
                        db.GIOHANGs.Add(new GIOHANG
                        {
                            MaND = mand,
                            MaGhe = ghe,
                            MaSuat = maSuat,
                            Gia = suat.GiaVe,
                            ThoiGianThem = DateTime.Now
                        });
                    }
                }
            }

            if (gheMoi.Count == 0)
            {
                if (gheTrung.Count > 0)
                {
                    TempData["Error"] = "Một số ghế đã có trong giỏ hàng của bạn.";
                }
                else
                {
                    TempData["Error"] = "Không có ghế nào được thêm vào giỏ hàng (có thể đã được đặt).";
                }
                return RedirectToAction("ChonGhe", new { id = maSuat });
            }

            try
            {
                db.SaveChanges();
                string message = "Đã thêm " + gheMoi.Count + " ghế vào giỏ hàng.";
                if (gheTrung.Count > 0)
                {
                    message += " (" + gheTrung.Count + " ghế đã có trong giỏ hàng)";
                }
                TempData["Success"] = message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi thêm vào giỏ hàng: " + ex.Message;
                return RedirectToAction("ChonGhe", new { id = maSuat });
            }

            return RedirectToAction("Index", "GioHang");
        }
    }
}
