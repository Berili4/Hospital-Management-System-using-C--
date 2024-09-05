using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    public class Islem
    {
        public int IslemId { get; set; }
        public int HastaId { get; set; }
        public int RandevuId { get; set; }
        public int DoktorId { get; set; }
        public int TıbbiRaporId { get; set; }
    }
}