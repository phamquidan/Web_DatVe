using System;
using System.Linq;
using System.Web.Http;
using Web_DatVe.Models;

namespace Web_DatVe.Controllers
{
    [RoutePrefix("api/ThongKe")]
    public class MyAPIController : ApiController
    {
        private QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();


        // Số Phim
        [HttpGet]
        [Route("SoPhim")]
        public IHttpActionResult GetSoPhim()
        {
            try
            {
                int soPhim = db.PHIMs.Count();
                return Ok(new { SoPhim = soPhim });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Số Rạp
        [HttpGet]
        [Route("SoRap")]
        public IHttpActionResult GetSoRap()
        {
            try
            {
                int soRap = db.RAPs.Count();
                return Ok(new { SoRap = soRap });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Số Người Dùng
        [HttpGet]
        [Route("SoNguoiDung")]
        public IHttpActionResult GetSoNguoiDung()
        {
            try
            {
                int soNguoiDung = db.NGUOIDUNGs.Count();
                return Ok(new { SoNguoiDung = soNguoiDung });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Số Đơn Hàng
        [HttpGet]
        [Route("SoDonHang")]
        public IHttpActionResult GetSoDonHang()
        {
            try
            {
                int soDonHang = db.DONHANGs.Count();
                return Ok(new { SoDonHang = soDonHang });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



    }
}
