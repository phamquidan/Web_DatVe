using System;
using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using System.Data.Entity;
namespace Web_DatVe.Controllers
{
    public class DonHangController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        public ActionResult Index()
        {
            if (Session["MaND"] == null)
                return RedirectToAction("Login", "NguoiDung");

            int mand = (int)Session["MaND"];

            var list = db.DONHANGs
                .Where(x => x.MaND == mand)
                .OrderByDescending(x => x.NgayDat)
                .ToList();

            return View(list);
        }

        public ActionResult Checkout()
        {
            if (Session["MaND"] == null) return RedirectToAction("Login", "NguoiDung");

            int mand = (int)Session["MaND"];
            var cart = db.GIOHANGs.Where(x => x.MaND == mand).ToList();

            if (!cart.Any())
                return RedirectToAction("Index", "GioHang");

            decimal tong = cart.Sum(x => x.Gia);

            ViewBag.TongTien = tong;
            ViewBag.Cart = cart;

            return View();
        }

        [HttpPost]
        public ActionResult ThanhToan(string phuongthuc)
        {
            if (Session["MaND"] == null) return RedirectToAction("Login", "NguoiDung");

            int mand = (int)Session["MaND"];
            var cart = db.GIOHANGs.Where(x => x.MaND == mand).ToList();
            if (!cart.Any()) return RedirectToAction("Index");

            decimal tong = cart.Sum(x => x.Gia);

            var dh = new DONHANG
            {
                MaND = mand,
                NgayDat = DateTime.Now,
                TongTien = tong,
                TrangThai = "Đã thanh toán",
                PhuongThucThanhToan = phuongthuc
            };

            db.DONHANGs.Add(dh);
            db.SaveChanges();

            foreach (var g in cart)
            {
                VE v = new VE
                {
                    MaDH = dh.MaDH,
                    MaGhe = g.MaGhe,
                    MaSuat = g.MaSuat,
                    Gia = g.Gia
                };
                db.VEs.Add(v);
            }

            db.GIOHANGs.RemoveRange(cart);
            db.SaveChanges();

            return RedirectToAction("ChiTiet", new { id = dh.MaDH });
        }

        public ActionResult ChiTiet(int id)
        {
            var dh = db.DONHANGs.Find(id);
            if (dh == null) return HttpNotFound();

            var ve = db.VEs.Where(v => v.MaDH == id).ToList();

            ViewBag.Ve = ve;

            return View(dh);
        }
    }
}
