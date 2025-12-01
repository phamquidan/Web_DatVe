using System.Linq;
using System.Web.Mvc;
using Web_DatVe.Models;
using System.Data.Entity;
namespace Web_DatVe.Controllers
{
    public class GioHangController : Controller
    {
        QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        public ActionResult Index()
        {
            if (Session["MaND"] == null) return RedirectToAction("Login", "NguoiDung");

            int mand = (int)Session["MaND"];

            var list = db.GIOHANGs
                .Where(x => x.MaND == mand)
                .ToList();

            return View(list);
        }

        public ActionResult Xoa(int id)
        {
            var item = db.GIOHANGs.Find(id);
            if (item != null)
            {
                db.GIOHANGs.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
