using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Razor.Tokenizer;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace WebApplication1.Models.YongHuB
{
    /// <summary>
    /// ShanChuPingLun 的摘要说明
    /// </summary>
    public class ShanChuPingLun : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["ID"] == null)
            {
                context.Response.Redirect("/DengLu/DengLuYe");
                return;
            }
            string json = new StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            bool success = false;
            if (obj?.ZhuPingLunID != null)
            {
                // 尝试获取 ZhuPingLunID
                if (int.TryParse(obj.ZhuPingLunID.ToString(), out int zhuPingLunID))
                {
                    success = DeleteZhuPingLun(zhuPingLunID, context);
                    context.Response.Write("{\"success\": " + success.ToString().ToLower() + "}");
                }
            }
            else if (obj?.ZiPingLunID != null)
            {
                if (int.TryParse(obj.ZiPingLunID.ToString(), out int ziPingLunID))
                {
                    success = DeleteZiPingLun(ziPingLunID, context);
                    context.Response.Write("{\"success\": " + success.ToString().ToLower() + "}");
                }
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "缺少评论ID参数" }));
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private bool DeleteZhuPingLun(int zhuPingLunID, HttpContext context)
        {
            // 实现删除主评论的逻辑（包括可能级联删除子评论）
            // 示例：调用数据库帮助类
            // 注意：一般需要验证当前登录用户是否有权限删除（评论作者或管理员）
            try
            {
                using (YouXiEntities youxi = new YouXiEntities())
                {
                    ZhuPinglun zpl= youxi.ZhuPinglun.FirstOrDefault(p => p.ZhuPingLunID == zhuPingLunID);
                    zpl.ZhuangTai = 1;
                    youxi.SaveChanges();
                    var ZPingLun=youxi.ZhuPinglun.Where(p => p.ZhuPingLunID == zhuPingLunID).FirstOrDefault(p=>p.ZhuangTai==1);
                    return ZPingLun!=null;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool DeleteZiPingLun(int ziPingLunID, HttpContext context)
        {
            // 实现删除子评论的逻辑
            try
            {
                using (YouXiEntities youxi = new YouXiEntities())
                {
                    ZiPinglun zpl = youxi.ZiPinglun.FirstOrDefault(p => p.ZiPingLunID == ziPingLunID);
                    zpl.ZhuangTai = 1;
                    youxi.SaveChanges();
                    var ZPingLun = youxi.ZiPinglun.Where(p => p.ZiPingLunID == ziPingLunID).FirstOrDefault(p => p.ZhuangTai == 1);
                    return ZPingLun != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}