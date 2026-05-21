using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class HuoQuTieZi
    {
        /// <summary>
        /// 根据用户ID获取帖子信息
        /// </summary>
        /// <param name="YongHuID"></param>
        /// <returns></returns>
        public static List<TieZi> TieZiXinXi(int YongHuID)
        {
            using (YouXiEntities YouXi=new YouXiEntities())
            {
                var list = YouXi.TieZi.Where(p => p.YongHuID == YongHuID).ToList();
                return list;
            }
        }
        /// <summary>
        /// 根据帖子ID获取照片信息
        /// </summary>
        /// <param name="TieZiID"></param>
        /// <returns></returns>
        public static ZhaoPian ZhaoPianXinxiONE(int TieZiID)
        {
            using (YouXiEntities youXi=new YouXiEntities())
            {
                var zhaopian = youXi.ZhaoPian.Where(p => p.TieZiID == TieZiID).FirstOrDefault();
                return zhaopian;
            }
        }
        /// <summary>
        /// 根据帖子ID获取照片信息
        /// </summary>
        /// <param name="TieZiID"></param>
        /// <returns></returns>
        public static List<ZhaoPian> ZhaoPianXinxi(int TieZiID)
        {
            using (YouXiEntities youXi = new YouXiEntities())
            {
                var zhaopian = youXi.ZhaoPian.Where(p => p.TieZiID == TieZiID).ToList();
                return zhaopian;
            }
        }
        /// <summary>
        /// 根据帖子ID获取帖子详情信息
        /// </summary>
        /// <param name="TieZiID"></param>
        /// <returns></returns>
        public static List<TieziXiangXi> NeiRong(int TieZiID)
        {
            using (YouXiEntities youXi = new YouXiEntities())
            {
                var neirong = youXi.TieziXiangXi.Where(p => p.TieZiID == TieZiID).ToList();
                return neirong;
            }
        }
    }
}