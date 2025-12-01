using System;
using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using System.Data.Entity;

namespace Web_DatVe.Controllers
{
    public class NguoiDungController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        /* ------------------- LOGIN ------------------- */

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string matkhau)
        {
            var user = db.NGUOIDUNGs.SingleOrDefault(x => x.Email == email && x.MatKhau == matkhau);

            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }

            Session["MaND"] = user.MaND;
            Session["HoTen"] = user.HoTen;
            Session["VaiTro"] = user.VaiTro;

            // Nếu là Admin hoặc QuanTri thì redirect đến trang Admin
            if (user.VaiTro == "Admin" || user.VaiTro == "QuanTri")
            {
                return RedirectToAction("Index", "Admin");
            }

            // Người dùng thường thì về trang chủ
            return RedirectToAction("Index", "Home");
        }

        /* ------------------- REGISTER ------------------- */

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(NGUOIDUNG nd)
        {
            if (!ModelState.IsValid)
                return View(nd);

            nd.VaiTro = "KhachHang";
            nd.NgayTao = DateTime.Now;

            db.NGUOIDUNGs.Add(nd);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        /* ------------------- PROFILE ------------------- */

        public new ActionResult Profile()
        {
            if (Session["MaND"] == null)
                return RedirectToAction("Login");

            int mand = (int)Session["MaND"];
            var user = db.NGUOIDUNGs.Find(mand);

            return View(user);
        }

        [HttpPost]
        public new ActionResult Profile(NGUOIDUNG nd)
        {
            var user = db.NGUOIDUNGs.Find(nd.MaND);

            user.HoTen = nd.HoTen;
            user.Email = nd.Email;
            user.SoDienThoai = nd.SoDienThoai;
            user.DiaChi = nd.DiaChi;
            user.NgaySinh = nd.NgaySinh;
            user.GioiTinh = nd.GioiTinh;

            db.SaveChanges();

            Session["HoTen"] = user.HoTen;
            ViewBag.Success = "Cập nhật thông tin thành công!";

            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
