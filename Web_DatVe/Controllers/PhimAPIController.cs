using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Web_DatVe.Models;

namespace Web_DatVe.Controllers
{
    public class PhimAPIController : ApiController
    {
        public QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();

        [HttpGet]
        [Route("api/ping")]
        public IHttpActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpGet]
        [Route("api/phim/getall")]
        public IHttpActionResult GetAll()
        {
            return Ok(db.PHIMs.ToList());
        }

        [HttpGet]
        [Route("api/phim/get/{id:int}")]
        public IHttpActionResult GetPhim(int id)
        {
            var phim = db.PHIMs.Find(id);
            if (phim == null) return NotFound();
            return Ok(phim);
        }

        [HttpPost]
        [Route("api/phim/create")]
        public IHttpActionResult PostPhim(PHIM phim)
        {
            db.PHIMs.Add(phim);
            db.SaveChanges();
            return Ok(phim);
        }

        [HttpPut]
        [Route("api/phim/update/{id:int}")]
        public IHttpActionResult PutPhim(int id, PHIM phim)
        {
            if (id != phim.MaPhim) return BadRequest("ID mismatch");
            db.Entry(phim).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok(phim);
        }

        [HttpDelete]
        [Route("api/phim/delete/{id:int}")]
        public IHttpActionResult DeletePhim(int id)
        {
            try
            {
                var phim = db.PHIMs.Find(id);
                if (phim == null) return NotFound();


                var suatChieuList = db.SUATCHIEUx.Where(sc => sc.MaPhim == id).ToList();

                var maSuatChieuList = suatChieuList.Select(sc => sc.MaSuat).ToList();

                var veList = db.VEs.Where(v => maSuatChieuList.Contains(v.MaSuat)).ToList();
                db.VEs.RemoveRange(veList);

                db.SUATCHIEUx.RemoveRange(suatChieuList);

                db.PHIMs.Remove(phim);

                // Lưu tất cả thay đổi
                db.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Tra ve loi chi tiet de de debug
                return Content(HttpStatusCode.InternalServerError, "Xóa thất bại. Vui lòng kiểm tra các ràng buộc dữ liệu liên quan khác.");
            }
        }

    }
}