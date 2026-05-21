using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class YongHuXinXi
    {
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="YongHuID"></param>
        /// <returns></returns>
        public static YongHu YongHuXinxi(int YongHuID)
        {
            using (YouXiEntities youXi=new YouXiEntities())
            {
                var yonghu = youXi.YongHu.Where(p => p.YongHuID == YongHuID).First();
                return yonghu;
            }
        }
    }
}