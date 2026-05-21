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
    /// fasongzhupinglun 的摘要说明
    /// </summary>
    public class fasongzhupinglun : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["ID"] == null)
            {
                context.Response.Redirect("/DengLu/DengLuYe");
                return;
            }
            int id= Convert.ToInt32(context.Request["id"]);
            object userId = context.Session["ID"];
            string json = new StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            using (YouXiEntities youxi=new YouXiEntities())
            {
                ZhuPinglun zpl = new ZhuPinglun();
                zpl.TieZiID= id;
                zpl.YongHuID = Convert.ToInt32(userId);
                zpl.PingLuNeiRong = obj.pinglun;
                zpl.PingLuShiJian = DateTime.Now;
                zpl.ZhuangTai = 0;
                youxi.ZhuPinglun.Add(zpl);
                youxi.SaveChanges();
                var zhupinlun = youxi.ZhuPinglun.Where(a => a.TieZiID == zpl.TieZiID)
                                                .Where(p=>p.YongHuID== zpl.YongHuID)
                                                .Where(p=>p.PingLuNeiRong== zpl.PingLuNeiRong)
                                                .Where(p=>p.ZhuangTai== zpl.ZhuangTai)
                                                .ToList();
                context.Response.ContentType = "application/json";
                if (zhupinlun.Count>0)
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