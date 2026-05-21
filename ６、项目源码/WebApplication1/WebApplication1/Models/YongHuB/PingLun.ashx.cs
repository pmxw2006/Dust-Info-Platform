using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebApplication1.Models.YongHuB
{
    /// <summary>
    /// PingLun 的摘要说明 - 获取某帖子下的所有评论（主评论 + 子评论 + 回复）
    /// </summary>
    public class PingLun : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // 1. 获取帖子ID，简单校验
                if (!int.TryParse(context.Request.QueryString["id"], out int tieZiId))
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("缺少或无效的帖子ID参数");
                    return;
                }

                using (YouXiEntities youxi = new YouXiEntities())
                {
                    // 关闭延迟加载和代理创建，防止序列化循环引用
                    youxi.Configuration.LazyLoadingEnabled = false;
                    youxi.Configuration.ProxyCreationEnabled = false;

                    // 2. 加载所有需要的数据：主评论 -> 子评论 -> 回复，以及对应的用户
                    var mainComments = youxi.ZhuPinglun
                        .Include(p => p.YongHu)                                   // 主评论用户
                        .Include(p => p.ZiPinglun.Select(zi => zi.YongHu))       // 子评论用户
                        .Where(z => z.TieZiID == tieZiId && z.ZhuangTai == 0)     // 只取有效主评论
                        .Select(z => new
                        {
                            // 主评论基本信息
                            z.YongHuID,
                            z.ZhuPingLunID,
                            z.PingLuNeiRong,
                            z.PingLuShiJian,
                            YongHuMing = z.YongHu.YongHuMing,

                            // 子评论（包含其回复）
                            ZiPinglunList = z.ZiPinglun
                                .Where(zi => zi.ZhuangTai == 0)                    // 有效子评论
                                .Select(zi => new
                                {
                                    zi.YongHuID,
                                    zi.ZiPingLunID,
                                    zi.PingLuNeiRong,
                                    zi.PingLuShiJian,
                                    YongHuMing = zi.YongHu.YongHuMing,
                                }).ToList()
                        })
                        .ToList();

                    // 3. 将数据映射为最终要序列化的结构（日期格式化）
                    var result = mainComments.Select(mc => new
                    {
                        mc.YongHuID,
                        mc.ZhuPingLunID,
                        mc.PingLuNeiRong,
                        PingLuShiJian = mc.PingLuShiJian.ToShortDateString(),
                        mc.YongHuMing,
                        ZiPinglunList = mc.ZiPinglunList.Select(zi => new
                        {
                            zi.ZiPingLunID,
                            zi.YongHuID,
                            zi.PingLuNeiRong,
                            PingLuShiJian = zi.PingLuShiJian.ToShortDateString(),
                            zi.YongHuMing,
                        }).ToList()
                    }).ToList();

                    // 4. 序列化为 JSON 并输出
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(result);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(json);
                }
            }
            catch (Exception ex)
            {
                // 生产环境建议记录日志，此处简单返回错误信息
                context.Response.StatusCode = 500;
                context.Response.Write("服务器内部错误：" + ex.Message);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}