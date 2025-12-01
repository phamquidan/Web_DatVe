using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_DatVe.Models.ViewModels
{
    public class SeatMapVM
    {
        public SUATCHIEU Suat { get; set; }
        public PHONG Phong { get; set; }
        public List<GHE> Ghe { get; set; }
        public List<int> GheDaDat { get; set; }
    }
}