using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace WebApplication1.Models.YongHuB
{
    /// <summary>
    /// FaSongZiPingLun 的摘要说明
    /// </summary>
    public class FaSongZiPingLun : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["ID"] == null)
            {
                context.Response.Redirect("/DengLu/DengLuYe");
                return;
            }
            int id = Convert.ToInt32(context.Request["id"]);
            object userId = context.Session["ID"];
            string json = new StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            using (YouXiEntities youxi = new YouXiEntities())
            {
                ZiPinglun zpl = new ZiPinglun
                {
                    TieZiID = id,
                    YongHuID = Convert.ToInt32(userId),
                    PingLuNeiRong = obj.pinglun,
                    ZhuangTai = 0,
                    PingLuShiJian = DateTime.Now,
                    ZhuPingLuID = obj.zhuPingLunID
                };
                youxi.ZiPinglun.Add(zpl);
                youxi.SaveChanges();
                var zipinlun = youxi.ZiPinglun.Where(a => a.TieZiID == zpl.TieZiID)
                                                .Where(p => p.YongHuID == zpl.YongHuID)
                                                .Where(p => p.PingLuNeiRong == zpl.PingLuNeiRong)
                                                .Where(p => p.ZhuangTai == zpl.ZhuangTai)
                                                .Where(p => p.ZhuPingLuID == zpl.ZhuPingLuID)
                                                .ToList();
                context.Response.ContentType = "application/json";
                if (zipinlun.Count > 0)
                {
                    context.Response.Write("{\"success\": true, \"message\": \"评论成功\"}");
                }
                else
                {
                    context.Response.Write("{\"success\": false, \"message\": \"评论失败\"}");
                }
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