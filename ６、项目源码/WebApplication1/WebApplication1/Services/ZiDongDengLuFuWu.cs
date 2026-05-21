using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    /// <summary>
    /// 自动登录服务：尝试通过 Cookie 进行自动登录，成功则返回跳转结果，失败返回 null
    /// </summary>
    public static class ZiDongDengLuFuWu
    {
        /// <summary>
        /// 尝试自动登录，若成功则返回对应的 RedirectResult，否则返回 null
        /// </summary>
        public static ActionResult ChangShiZiDongDengLu()
        {
            // 1. 验证 Cookie 并获取用户ID
            int YongHuID = CookieFuWu.YanZhengCookieBingHuoQuYongHuID();
            if (YongHuID==0)
                return null;

            // 2. 根据用户ID获取用户实体
            var user = YongHuYanZhengFuWu.HuoQuYongHuByMingCheng(YongHuID);
            if (user == null)
            {
                HttpContext.Current.Session.Clear();
                CookieFuWu.QingChuCookie();
                return null;
            }

            // 3. 检查账号是否被封禁
            if (user.ZhuangTai == 0)
            {
                HttpContext.Current.Session.Clear();
                CookieFuWu.QingChuCookie();
                // 注意：在调用方根据返回值是否为 null 决定是否显示错误信息，
                // 这里无法直接设置 TempData，因此由调用方自行处理错误提示。
                return null;
            }

            // 4. 登录成功，设置 Session
            HttpContext.Current.Session["ID"] = user.YongHuID;
            HttpContext.Current.Session["QuanXian"] = user.QuanXian;

            // 5. 获取跳转 URL
            string url = QuanXianTiaoZhuanFuWu.HuoQuTiaoZhuanUrl(int.Parse(user.QuanXian));
            if (url != null)
            {
                return new RedirectResult(url);
            }
            else
            {
                // 权限信息有误，清理登录状态
                HttpContext.Current.Session.Clear();
                CookieFuWu.QingChuCookie();
                return null;
            }
        }
    }
}