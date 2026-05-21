using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.Web.Script.Serialization;
using System.Collections;

namespace WebApplication1.Models.YongHuB
{
    /// <summary>
    /// TieZiHandler 的摘要说明
    /// </summary>
    public class TieZiHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            int page = 1, pageSize = 10;
            int.TryParse(context.Request["page"], out page);
            int.TryParse(context.Request["pageSize"], out pageSize);
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            using (YouXiEntities db = new YouXiEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                // 一次性获取所有帖子及其第一张图片（EF 会生成一个 LEFT JOIN 子查询，避免 N+1）
                var allPosts = (from t in db.TieZi
                                join y in db.YongHu on t.YongHuID equals y.YongHuID
                                where t.ZhuangTai == 1
                                select new
                                {
                                    t.TieZiID,
                                    t.TieZiMing,
                                    t.YongHuID,
                                    y.YongHuMing,
                                    t.FaBuShiJian,
                                    FirstImage = db.ZhaoPian
                                        .Where(z => z.TieZiID == t.TieZiID)
                                        .Select(z => z.Lujing)
                                        .FirstOrDefault()
                                }).ToList();

                // 内存排序：按发布时间降序（最新在前）
                var sorted = allPosts
                    .OrderByDescending(p => p.FaBuShiJian)
                    .ToList();

                int totalCount = sorted.Count;
                // 分页
                var paged = sorted
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(item => new
                    {
                        t = new
                        {
                            item.TieZiID,
                            item.TieZiMing,
                            item.YongHuID,
                            item.YongHuMing,
                            item.FaBuShiJian
                        },
                        Lujing = item.FirstImage ?? "YongHu.jpeg"
                    }).ToList();

                var responseData = new
                {
                    total = totalCount,
                    page = page,
                    pageSize = pageSize,
                    list = paged
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                context.Response.Write(js.Serialize(responseData));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}