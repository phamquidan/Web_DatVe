using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_DatVe.Models.ViewModels
{
    public class MovieDetailVM
    {
        public PHIM Phim { get; set; }
        public List<SUATCHIEU> SuatChieu { get; set; }
    }
}