using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models
{
    public class CombinedModels
    {
        public Rapor Rapor { get; set; }
        public Doktor Doktor { get; set; }
        public Hasta Hasta { get; set; }
        public Yönetici Yönetici { get; set; }
    }
}