using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace WebApplication1.Models.HaoYou
{
    public class HuoQuDangQianYongHu : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Session["ID"] == null)
                {
                    context.Response.Redirect("/DengLu/DengLuYe");
                    return;
                }
                // 读取请求体
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                var obj = JsonConvert.DeserializeObject<dynamic>(json);

                // 获取当前登录用户ID（不做额外验证）
                int userId = (int)context.Session["ID"];

                // 安全获取目标用户ID，避免 null 或无法转换的异常
                int targetId = 0;
                if (obj?.YongHuID != null)
                    int.TryParse(obj.YongHuID.ToString(), out targetId);

                if (targetId == 0)
                {
                    WriteJsonError(context, "缺少或无效的YongHuID参数");
                    return;
                }

                using (YouXiEntities youXi = new YouXiEntities())
                {
                    var result = youXi.GuanZhuYongHu
                        .Include(p => p.YongHu)
                        .Include(p => p.YongHu1)
                        .Where(p => (p.YongHuIDYI == targetId && p.YongHuIDER == userId) ||
                                    (p.YongHuIDYI == userId && p.YongHuIDER == targetId))
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
                        .FirstOrDefault();   // 可能为 null（无关注关系）

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string jsonResult = js.Serialize(result);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jsonResult);
                }
            }
            catch (Exception ex)
            {
                // 返回 JSON 格式错误，避免返回 HTML 导致前端解析失败
                WriteJsonError(context, "服务器内部错误: " + ex.Message.Replace("\"", "\\\""));
            }
        }

        private void WriteJsonError(HttpContext context, string errorMessage)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"error\":\"" + errorMessage + "\"}");
        }

        public bool IsReusable => false;
    }
}