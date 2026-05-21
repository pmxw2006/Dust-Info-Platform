using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Newtonsoft.Json;
using WebApplication1.Services;

namespace WebApplication1.Models.HaoYou
{
    /// <summary>
    /// HaoYouHuoQu 的摘要说明
    /// </summary>
    public class HaoYouHuoQu : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            // 读取请求体
            if (context.Session["ID"]==null)
            {
                context.Response.Redirect("~/YongHu/Index");
                return;
            }
            int userId = (int)context.Session["ID"];
            string json = new StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            string yongHuId = obj.YongHuID;
            int targetId = int.Parse(yongHuId);
            using (YouXiEntities youXi=new YouXiEntities())
            {
                var lianxin = youXi.GuanZhuYongHu
                                   .Where(p => (p.YongHuIDYI == targetId && p.YongHuIDER == userId) ||
                                   (p.YongHuIDYI == userId && p.YongHuIDER == targetId))
                                    .Select(p => new
                                    {
                                        p.GuanZhuYongHuID,          // 假设有关键字段
                                        p.YongHuIDYI,
                                        p.YongHuIDER,
                                        p.GuangZhuZhuangTai,
                                        p.HuiGuanZhuangTai
                                    })
                                    .ToList();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonResult = js.Serialize(lianxin);
                context.Response.ContentType = "application/json";
                    context.Response.Write(jsonResult);
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