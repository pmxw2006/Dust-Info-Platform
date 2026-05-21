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
    /// XiuGaiZhuangTai 的摘要说明
    /// </summary>
    public class XiuGaiZhuangTai : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["ID"] == null)
            {
                context.Response.Redirect("/DengLu/DengLuYe");
                return;
            }
            int userId = (int)context.Session["ID"];
            string json = new StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            string YongHuIDYI = obj?.YongHuIDYI;
            string YongHuIDER = obj?.YongHuIDER;
            string zhuangtai = obj?.ZhuangTai;
            using (YouXiEntities youXi = new YouXiEntities())
            {
                var xiangguan = youXi.GuanZhuYongHu
                    .Where(p => (p.YongHuIDYI == userId && p.YongHuIDER.ToString() == YongHuIDER) || (p.YongHuIDER == userId && p.YongHuIDYI.ToString() == YongHuIDYI)).ToList();
                if (xiangguan.Count == 0 && YongHuIDYI == null && YongHuIDER != null)
                {
                    int targetId = int.Parse(YongHuIDER);
                    GuanZhuYongHu guanZhuYongHu = new GuanZhuYongHu
                    {
                        YongHuIDYI = userId,
                        YongHuIDER = targetId,
                        GuangZhuZhuangTai = 0,
                        HuiGuanZhuangTai = 0,
                        LaiHeiZhuangTai = 0
                    };
                    youXi.GuanZhuYongHu.Add(guanZhuYongHu);
                    youXi.SaveChanges();
                    context.Response.ContentType = "application/json";
                    context.Response.Write("{\"success\": true}");
                }
                else
                {
                    if (YongHuIDYI == null && YongHuIDER != null)
                    {
                        int id = int.Parse(YongHuIDER);
                        int zhuangTai = int.Parse(zhuangtai);
                        GuanZhuYongHu yongHu = youXi.GuanZhuYongHu.Where(p => p.YongHuIDER == id && p.YongHuIDYI == userId).FirstOrDefault();
                        if (zhuangTai == 0)
                        {
                            yongHu.GuangZhuZhuangTai = 1;
                            var xinxi = youXi.XinXi.Where(p => p.ZhuangTai == 0 || p.ZhuangTai == 2).ToList();
                            foreach (var i in xinxi as List<XinXi>)
                            {
                                i.ZhuangTai = 1;
                            }
                        }
                        else if (zhuangTai == 1)
                        {
                            yongHu.GuangZhuZhuangTai = 0;
                        }
                        youXi.SaveChanges();
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"success\": true}");

                    }
                    else if (YongHuIDYI != null && YongHuIDER == null)
                    {
                        int id = int.Parse(YongHuIDYI);
                        int zhuangTai = int.Parse(zhuangtai);
                        GuanZhuYongHu lianxi = youXi.GuanZhuYongHu.Where(p => p.YongHuIDYI == id && p.YongHuIDER == userId).FirstOrDefault();
                        if (zhuangTai == 0)
                        {
                            lianxi.HuiGuanZhuangTai = 1;
                            var xinxi = youXi.XinXi.Where(p => p.ZhuangTai == 0 || p.ZhuangTai == 2).ToList();
                            foreach (var i in xinxi as List<XinXi>)
                            {
                                i.ZhuangTai = 1;
                            }
                        }
                        else if (zhuangTai == 1)
                        {
                            lianxi.HuiGuanZhuangTai = 0;
                            var xinxi = youXi.XinXi.Where(p => p.ZhuangTai == 0).ToList();
                            foreach (var i in xinxi as List<XinXi>)
                            {
                                i.ZhuangTai = 0;
                            }
                        }
                        youXi.SaveChanges();
                        context.Response.ContentType = "application/json";
                        context.Response.Write("{\"success\": true}");
                    }
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