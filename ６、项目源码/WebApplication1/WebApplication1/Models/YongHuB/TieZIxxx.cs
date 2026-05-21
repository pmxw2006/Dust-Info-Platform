using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.YongHuB
{
    /// <summary>
    /// 查询的帖子数据模型
    /// </summary>
    public class TieZIxxx
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public int TieZiID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string YongHuMing { get; set; }
        /// <summary>
        /// 帖子名
        /// </summary>
        public string TieZiMing { get; set; }
        /// <summary>
        /// 帖子类型
        /// </summary>
        public string TieZiLeiXing { get; set; }
        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTime ShangJiaShiJian { get; set; }
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string NeiRong { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Lujing { get; set; }
    }
}