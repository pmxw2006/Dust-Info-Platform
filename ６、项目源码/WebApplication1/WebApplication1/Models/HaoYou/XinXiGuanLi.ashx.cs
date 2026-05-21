using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace WebApplication1.Models.HaoYou
{
    public class XinXiGuanLi : IHttpHandler, IRequiresSessionState
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

                int userId = (int)context.Session["ID"];

                // 安全获取参数
                string Yid = obj?.YongHuID?.ToString();
                string neirong = obj?.NeiRong?.ToString();
                string caozuo = obj?.CaoZuo?.ToString();

                using (YouXiEntities youXi = new YouXiEntities())
                {
                    // 查询消息列表（无操作指令）
                    if (neirong == null && caozuo == null )
                    {
                        int id = 0;
                        if (!string.IsNullOrEmpty(Yid))
                            int.TryParse(Yid, out id);
                        if (id == 0)
                        {
                            WriteJson(context, "{\"error\":\"缺少有效的YongHuID\"}");
                            return;
                        }

                        var xixi = youXi.XinXi
                            .Include(p => p.YongHu)
                            .Include(p => p.YongHu1)
                            .Where(p => p.ZhuangTai == 0||(p.ZhuangTai==2 && p.FaSongZheID == userId && p.JieShouZheID==id))   // 只查询未删除的消息
                            .Where(p => (p.FaSongZheID == id && p.JieShouZheID == userId) ||
                                        (p.FaSongZheID == userId && p.JieShouZheID == id))
                            .Select(p => new
                            {
                                p.XinXiID,
                                p.FaSongZheID,
                                fasongming = p.YongHu.YongHuMing,
                                p.JieShouZheID,
                                jieshouming = p.YongHu1.YongHuMing,
                                p.XinXiNeiRong,
                                p.ChuangJianShiJian,
                                p.ZhuangTai
                            }).ToList();   // 立即执行查询

                        WriteJson(context, new JavaScriptSerializer().Serialize(xixi));
                    }
                    else
                    {
                            int toUserId = 0;
                            if (!string.IsNullOrEmpty(Yid))
                                int.TryParse(Yid, out toUserId);
                            if (toUserId == 0 || string.IsNullOrEmpty(neirong))
                            {
                                WriteJson(context, "{\"success\":false,\"message\":\"参数错误\"}");
                                return;
                            }

                        GuanZhuYongHu guanxi =youXi.GuanZhuYongHu.Where(p => (p.YongHuIDYI == toUserId && p.YongHuIDER == userId) ||
                                        (p.YongHuIDYI == userId && p.YongHuIDER == toUserId)).FirstOrDefault();
                        if (guanxi.YongHuIDYI==userId)
                        {
                            if (guanxi.GuangZhuZhuangTai==1)
                            {
                                XinXi xx = new XinXi
                                {
                                    FaSongZheID = userId,
                                    JieShouZheID = toUserId,
                                    XinXiNeiRong = neirong,
                                    ChuangJianShiJian = DateTime.Now,
                                    ZhuangTai = 2
                                };
                                youXi.XinXi.Add(xx);
                                WriteJson(context, "{\"success\":true}");
                            }
                            else
                            {
                                if (guanxi.HuiGuanZhuangTai == 1)
                                {
                                    var xixi = youXi.XinXi
                                        .Where(p => p.ZhuangTai == 0)   // 只查询未删除的消息
                                .Where(p => (p.FaSongZheID == toUserId && p.JieShouZheID == userId) ||
                                            (p.FaSongZheID == userId && p.JieShouZheID == toUserId)).ToList();
                                    if (xixi.Count >= 1)
                                    {
                                        XinXi xx = new XinXi
                                        {
                                            FaSongZheID = userId,
                                            JieShouZheID = toUserId,
                                            XinXiNeiRong = neirong,
                                            ChuangJianShiJian = DateTime.Now,
                                            ZhuangTai = 2
                                        };
                                        youXi.XinXi.Add(xx);
                                        WriteJson(context, "{\"success\":true}");
                                    }
                                    else
                                    {
                                        XinXi xx = new XinXi
                                        {
                                            FaSongZheID = userId,
                                            JieShouZheID = toUserId,
                                            XinXiNeiRong = neirong,
                                            ChuangJianShiJian = DateTime.Now,
                                            ZhuangTai = 0
                                        };
                                        youXi.XinXi.Add(xx);
                                        WriteJson(context, "{\"success\":true}");
                                    }
                                }
                                else if (guanxi.HuiGuanZhuangTai == 0)
                                {
                                    XinXi xx = new XinXi
                                    {
                                        FaSongZheID = userId,
                                        JieShouZheID = toUserId,
                                        XinXiNeiRong = neirong,
                                        ChuangJianShiJian = DateTime.Now,
                                        ZhuangTai = 0
                                    };
                                    youXi.XinXi.Add(xx);
                                    WriteJson(context, "{\"success\":true}");
                                }
                            }
                            
                        }
                        else if (guanxi.YongHuIDER==userId)
                        {
                            if (guanxi.HuiGuanZhuangTai==1)
                            {
                                XinXi xx = new XinXi
                                {
                                    FaSongZheID = userId,
                                    JieShouZheID = toUserId,
                                    XinXiNeiRong = neirong,
                                    ChuangJianShiJian = DateTime.Now,
                                    ZhuangTai = 2
                                };
                                youXi.XinXi.Add(xx);
                                WriteJson(context, "{\"success\":true}");
                            }
                            else
                            {
                                if (guanxi.GuangZhuZhuangTai == 1)
                                {
                                    var xixi = youXi.XinXi
                                        .Where(p => p.ZhuangTai == 0)   // 只查询未删除的消息
                                .Where(p => (p.FaSongZheID == toUserId && p.JieShouZheID == userId) ||
                                            (p.FaSongZheID == userId && p.JieShouZheID == toUserId)).ToList();
                                    if (xixi.Count >= 1)
                                    {
                                        XinXi xx = new XinXi
                                        {
                                            FaSongZheID = userId,
                                            JieShouZheID = toUserId,
                                            XinXiNeiRong = neirong,
                                            ChuangJianShiJian = DateTime.Now,
                                            ZhuangTai = 2
                                        };
                                        youXi.XinXi.Add(xx);
                                        WriteJson(context, "{\"success\":false}");
                                    }
                                    else
                                    {
                                        XinXi xx = new XinXi
                                        {
                                            FaSongZheID = userId,
                                            JieShouZheID = toUserId,
                                            XinXiNeiRong = neirong,
                                            ChuangJianShiJian = DateTime.Now,
                                            ZhuangTai = 0
                                        };
                                        youXi.XinXi.Add(xx);
                                        WriteJson(context, "{\"success\":true}");
                                    }
                                }
                                else if (guanxi.GuangZhuZhuangTai == 0)
                                {
                                    XinXi xx = new XinXi
                                    {
                                        FaSongZheID = userId,
                                        JieShouZheID = toUserId,
                                        XinXiNeiRong = neirong,
                                        ChuangJianShiJian = DateTime.Now,
                                        ZhuangTai = 0
                                    };
                                    youXi.XinXi.Add(xx);
                                    WriteJson(context, "{\"success\":true}");
                                }
                            }
                            
                        }
                        youXi.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                WriteJson(context, "{\"error\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        private void WriteJson(HttpContext context, string json)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        public bool IsReusable => false;
    }
}