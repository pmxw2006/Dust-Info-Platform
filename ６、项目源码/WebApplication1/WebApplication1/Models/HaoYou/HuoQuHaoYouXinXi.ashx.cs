using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using WebApplication1.Services;

namespace WebApplication1.Models.HaoYou
{
    /// <summary>
    /// HuoQuHaoYouXinXi 的摘要说明
    /// </summary>·
    public class HuoQuHaoYouXinXi : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["ID"] == null)
            {
                context.Response.Redirect("/DengLu/DengLuYe");
                return;
            }
            int userId = (int)context.Session["ID"];
            using (YouXiEntities youXi =new YouXiEntities())
            {
                    // 无 id 参数：返回当前用户的所有好友关系列表
                   var result = youXi.GuanZhuYongHu
                        .Include(p => p.YongHu)
                        .Include(p => p.YongHu1)
                        .Where(p=>p.GuangZhuZhuangTai==0||p.HuiGuanZhuangTai==0)
                        .Where(p => p.YongHuIDYI == userId || p.YongHuIDER == userId)
                        .Select(p => new
                        {
                            p.YongHuIDYI,
                            p.YongHuIDER,
                            YongHuYIMing = p.YongHu.YongHuMing,
                            YongHuERMing = p.YongHu1.YongHuMing,
                            p.GuangZhuZhuangTai,
                            p.HuiGuanZhuangTai,
                            p.LaiHeiZhuangTai
                        })
                        .ToList();
                
                // 序列化输出
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(result);
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
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