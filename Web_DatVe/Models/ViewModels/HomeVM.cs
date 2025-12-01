using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_DatVe.Models.ViewModels
{
    public class HomeVM
    {
        public List<PHIM> PhimDangChieu { get; set; }
        public List<PHIM> PhimSapChieu { get; set; }
    }
}