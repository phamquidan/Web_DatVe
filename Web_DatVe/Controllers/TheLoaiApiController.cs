using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_DatVe.Models;

namespace Web_DatVe.Controllers
{
    public class TheLoaiApiController : ApiController
    {
        private QL_DatVeXemPhimEntities1 db = new QL_DatVeXemPhimEntities1();


        [HttpGet]
        [Route("api/theloai/list")]
        public IHttpActionResult GetTheLoai()
        {
            try
            {
                var tenPhimList = db.PHIMs
                    .Select(p => p.TheLoai)
                    .ToList();

                return Ok(tenPhimList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
