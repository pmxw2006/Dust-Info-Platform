using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.YongHuB
{
    public class YongHuJiBenXinxi
    {
        private int _TieZiID;
        private string _TieZiMing;
        private string _Lujing;
        private DateTime _FaBuShiJian;

        public int TieZiID { get => _TieZiID; set => _TieZiID = value; }
        public string TieZiMing { get => _TieZiMing; set => _TieZiMing = value; }
        public string Lujing { get => _Lujing; set => _Lujing = value; }
        public DateTime FaBuShiJian { get => _FaBuShiJian; set => _FaBuShiJian = value; }
    }
}