using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Services
{
    /// <summary>
    /// Cookie 相关服务：负责读取、写入、清除、验证“记住密码”Cookie
    /// </summary>
    public static class CookieFuWu
    {
        // Cookie 的名称，与前端保持一致
        public const string CookieMingCheng = "ID";

        // 用于 Cookie 加密的固定盐值（与登录时使用的盐值无关，仅用于 Cookie 哈希计算）
        public const string CookieYanZhi = "GuDinZiChan_Cookie_Salt_2024";

        /// <summary>
        /// 获取当前请求中的 Cookie 值
        /// </summary>
        /// <returns>Cookie 值字符串，若不存在则返回 null</returns>
        public static string HuoQuCookieZhi()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieMingCheng];
            return cookie?.Value;  // 使用 null 条件运算符，避免空引用异常
        }

        /// <summary>
        /// 设置“记住密码”Cookie，有效期为一个月
        /// Cookie 值存储的是用户ID的哈希（使用固定盐值），防止用户名明文暴露
        /// </summary>
        /// <param name="yongHuID">登录成功的用户名</param>
        public static void SheZhiJiZhuMiMaCookie(string yongHuID)
        {
            // 计算用户名 + 固定盐值的哈希作为 Cookie 值
            string haXiZhi = MiMaFuWu.JiSuanHaXi(yongHuID, CookieYanZhi);

            // 创建 HttpCookie 对象
            HttpCookie cookie = new HttpCookie(CookieMingCheng, haXiZhi)
            {
                Expires = DateTime.Now.AddMonths(1),   // 有效期一个月
                HttpOnly = true                        // 防止 JavaScript 读取，提升安全性
            };

            // 将 Cookie 添加到响应中，发送给浏览器
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 清除 Cookie（登出时或 Cookie 无效时调用）
        /// 通过设置过期时间为过去的时间来删除客户端 Cookie
        /// </summary>
        public static void QingChuCookie()
        {
            if (HttpContext.Current.Request.Cookies[CookieMingCheng] != null)
            {
                HttpCookie expired = new HttpCookie(CookieMingCheng)
                {
                    Expires = DateTime.Now.AddDays(-1)  // 设置为昨天，浏览器会自动删除
                };
                HttpContext.Current.Response.Cookies.Add(expired);
            }
        }

        /// <summary>
        /// 验证 Cookie 是否有效，如果有效则返回对应的用户ID
        /// 用于自动登录功能
        /// </summary>
        /// <returns>验证成功返回用户ID，否则返回 null</returns>
        public static int YanZhengCookieBingHuoQuYongHuID()
        {
            // 1. 获取 Cookie 值
            string cookieZhi = HuoQuCookieZhi();
            if (string.IsNullOrEmpty(cookieZhi))
                return 0;

            // 2. 连接数据库，遍历所有状态正常的用户，比对哈希值
            using (var db = new Models.YouXiEntities())
            {
                // 查询所有状态为 1（正常）的用户
                var users = db.YongHu.Where(u => u.ZhuangTai == 1).ToList();

                foreach (var user in users)
                {
                    // 对每个用户名计算期望的 Cookie 哈希值
                    string expected = MiMaFuWu.JiSuanHaXi(user.YongHuID.ToString(), CookieYanZhi);

                    // 如果匹配，说明该 Cookie 对应该用户
                    if (expected == cookieZhi)
                    {
                        return user.YongHuID;
                    }
                }
            }

            // 遍历结束未找到匹配用户，返回 null
            return 0;
        }
    }
}