using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DengLu;
using WebApplication1.Models.RenYuanGuanLi;
using WebApplication1.Services;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// 登录控制器：处理登录页面的显示、表单提交、自动登录、登出等操作
    /// </summary>
    public class DengLuController : Controller
    {
        /// <summary>
        /// GET：显示登录页面，并尝试自动登录
        /// </summary>
        [HttpGet]
        public ActionResult DengLuYe()
        {
            // 尝试自动登录
            var autoLoginResult = ZiDongDengLuFuWu.ChangShiZiDongDengLu();
            if (autoLoginResult != null)
            {
                return autoLoginResult;
            }
            // 正常显示登录页
            var model = new DengLuViewModel
            {
                YongHuMing = "",
                JiZhuMiMa = false
            };
            return View(model);
        }

        /// <summary>
        /// POST：处理登录表单提交
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DengLuYe(DengLuViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int ID = 0;
            try
            {
                TempData["SkipAnimation"] = true;
                ID = int.Parse(model.YongHuMing.Trim());
            }
            catch (Exception)
            {
                ID = 0;
            }

            // 调用验证服务
            var user = YongHuYanZhengFuWu.YanZhengDengLu(
                ID,
                model.MiMa.Trim()
            );

            if (user == null)
            {
                // 进一步判断具体失败原因
                using (var db = new YouXiEntities())
                {
                    try
                    {
                        var tempUser = db.YongHu.FirstOrDefault(u => u.YongHuID == ID);
                        if (tempUser == null)
                            TempData["ErrorMessage"] = "账号或密码错误";
                        else if (tempUser.ZhuangTai == 0)
                            TempData["ErrorMessage"] = "您的账号已被封禁";
                        else
                            TempData["ErrorMessage"] = "账号或密码错误";
                    }
                    catch (Exception)
                    {
                        TempData["ErrorMessage"] = "服务器繁忙请稍后再试";
                    }
                }
                return View(model);
            }

            // 登录成功
            if (model.JiZhuMiMa)
            {
                CookieFuWu.SheZhiJiZhuMiMaCookie(user.YongHuID.ToString());
            }

            Session["ID"] = user.YongHuID;
            Session["QuanXian"] = user.QuanXian;
            string url = null;
            try
            {
                url = QuanXianTiaoZhuanFuWu.HuoQuTiaoZhuanUrl(int.Parse(user.QuanXian));
            }
            catch (Exception)
            {
                url = null;
            }
            if (url != null)
            {
                return Redirect(url);
            }
            else
            {
                Session.Clear();
                CookieFuWu.QingChuCookie();
                TempData["ErrorMessage"] = "您的权限信息有误，请联系管理员";
                return RedirectToAction("DengLuYe");
            }
        }

        /// <summary>
        /// GET：忘记密码页面（支持自动登录）
        /// </summary>
        [HttpGet]
        public ActionResult WangJiMiMa()
        {
            // 尝试自动登录
            var autoLoginResult = ZiDongDengLuFuWu.ChangShiZiDongDengLu();
            if (autoLoginResult != null)
            {
                return autoLoginResult;
            }

            return View(new WangJiMiMaViewModel());
        }

        /// <summary>
        /// POST：处理忘记密码请求（验证身份并修改密码）
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WangJiMiMa(WangJiMiMaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "账户与邮箱不匹配，请检查后重试";
                TempData["SkipAnimation"] = true;
                return View(model);
            }

            int yongHuMing = 0;
            string youXiang = "";
            string xinMiMa = "";
            try
            {
                yongHuMing = int.Parse(model.YongHuMing.Trim());
                youXiang = model.YouXiang.Trim();
                xinMiMa = model.XinMiMa.Trim();
            }
            catch (Exception)
            {
                // 转换失败已在 ModelState 中体现，此处忽略
            }

            // 验证账户和邮箱是否匹配
            bool yanZhengTongGuo = YongHuYanZhengFuWu.YanZhengYongHuShenFen(yongHuMing, youXiang);
            if (!yanZhengTongGuo)
            {
                TempData["ErrorMessage"] = "账户与邮箱不匹配，请检查后重试";
                TempData["SkipAnimation"] = true;
                return View(model);
            }

            // 验证通过，修改密码
            YongHuYanZhengFuWu.XiuGaiMiMa(yongHuMing, xinMiMa);

            TempData["SuccessMessage"] = "密码修改成功，2秒后跳转到登录页...";
            TempData["SkipAnimation"] = true;
            return View(new WangJiMiMaViewModel()); // 返回清空表单的视图，显示成功消息
        }

        /// <summary>
        /// 添加账户页面（支持自动登录）
        /// </summary>
        [HttpGet]
        public ActionResult TianJiaZhangHu()
        {
            // 尝试自动登录
            var autoLoginResult = ZiDongDengLuFuWu.ChangShiZiDongDengLu();
            if (autoLoginResult != null)
            {
                return autoLoginResult;
            }

            return View(new TianJiaZhangHuViewModel());
        }

        /// <summary>
        /// 处理添加账户表单提交
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TianJiaZhangHu(TianJiaZhangHuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "添加失败";
                TempData["SkipAnimation"] = true;
                return View(model);
            }

            int newUserId = 0;
            try
            {
                newUserId = YongHuYanZhengFuWu.TianJiaYongHu(model);
            }
            catch (Exception)
            {
                newUserId = 0;
            }

            if (newUserId > 0)
            {
                TempData["SuccessMessage"] = $"账户创建成功！您的账号ID是：{newUserId},十秒后将会跳转至登录页";
                TempData["SkipAnimation"] = true;   // 跳过入场动画
                return View(new TianJiaZhangHuViewModel()); // 返回清空表单，并显示成功消息
            }
            else
            {
                TempData["ErrorMessage"] = "添加失败";
                TempData["SkipAnimation"] = true;
                return View(model);
            }
        }

        /// <summary>
        /// 默认页
        /// </summary>
        public ActionResult Index()
        {
            return RedirectToAction("DengLuYe");
        }

        /// <summary>
        /// 登出操作
        /// </summary>
        public ActionResult DengChu()
        {
            Session.Clear();
            CookieFuWu.QingChuCookie();
            return RedirectToAction("DengLuYe");
        }
    }
}