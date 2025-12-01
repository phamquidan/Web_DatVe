using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_DatVe.Models.ViewModels
{
    public class RapDetailVM
    {
        public RAP Rap { get; set; }
        public List<PHONG> Phong { get; set; }
        public List<SUATCHIEU> SuatChieu { get; set; }
    }
}