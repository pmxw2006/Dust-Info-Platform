using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Services;
using WebApplication1.Models.GeRenZhongXin;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GuanGeRenZhongXinController : Controller
    {
        [HttpGet]
        // GET: GuanGeRenZhongXin
        public ActionResult GeRenXinXi()
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            int yongHuID = int.Parse(Session["ID"].ToString());
            var model = YongHuYanZhengFuWu.HuoQuGeRenXinXi(yongHuID);
            if (model == null)
                return RedirectToAction("DengLuYe", "DengLu");

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GeRenXinXi(GeRenXinXiViewModel model)
        {
            int currentYongHuID = int.Parse(Session["ID"].ToString());
            bool success = YongHuYanZhengFuWu.BaoCunGeRenXinXi(currentYongHuID, model);

            if (success)
            {
                TempData["SuccessMessage"] = "个人信息保存成功";
            }
            else
            {
                TempData["ErrorMessage"] = "保存失败";
            }

            return RedirectToAction("GeRenXinXi");
        }
        /// <summary>
        /// GET：修改密码页面
        /// </summary>
        [HttpGet]
        public ActionResult XiuGaiMiMa()
        {
            // 未登录则跳转登录页
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            return View(new XiuGaiMiMaViewModel());
        }

        /// <summary>
        /// POST：处理修改密码请求
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XiuGaiMiMa(XiuGaiMiMaViewModel model)
        {
            // 登录检查
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            // 跳过入场动画（表单回发时）
            TempData["SkipAnimation"] = true;

            // 服务端模型验证（非空、长度、一致性）
            if (!ModelState.IsValid)
                return View(model);

            int yongHuID = int.Parse(Session["ID"].ToString());

            // 1. 验证当前密码是否正确
            if (!YongHuYanZhengFuWu.YanZhengDangQianMiMa(yongHuID, model.DangQianMiMa))
            {
                TempData["ErrorMessage"] = "当前密码不正确";
                return View(model);
            }

            // 2. 新密码不能与当前密码相同
            if (model.DangQianMiMa == model.XinMiMa)
            {
                TempData["ErrorMessage"] = "新密码不能与当前密码相同";
                return View(model);
            }

            // 3. 执行密码修改
            bool success = YongHuYanZhengFuWu.DXiuGaiMiMa(yongHuID, model.XinMiMa.Trim());
            if (success)
            {
                // 清除登录状态（但保留当前请求的 Session，以便显示成功消息）
                Session.Clear();
                CookieFuWu.QingChuCookie();
                TempData["SuccessMessage"] = "密码修改成功，即将跳转到登录页...";
                // 不重定向，返回当前视图，让前端处理延迟跳转
                return View(new XiuGaiMiMaViewModel());
            }
            else
            {
                TempData["ErrorMessage"] = "密码修改失败，请稍后重试";
                return View(model);
            }
        }
        public ActionResult Tiezinerong()
        {
            if (Request["id"] != null)
            {
                int id = int.Parse(Request["id"]);
                using (YouXiEntities youXi = new YouXiEntities())
                {
                    var Tiezi = youXi.TieZi.Where(p => p.TieZiID == id).ToList();
                    int yonghu = 0;
                    foreach (var i in Tiezi as List<TieZi>)
                    {
                        yonghu = i.YongHuID;
                    }
                    var yonghuming = youXi.YongHu.Where(p => p.YongHuID == yonghu).ToList();
                    foreach (var i in yonghuming as List<YongHu>)
                    {
                        ViewBag.yonghuming = i.YongHuMing;
                    }
                    ViewBag.tiezi = Tiezi;
                    ViewBag.zhaopian = youXi.ZhaoPian.Where(p => p.TieZiID == id).ToList();
                    ViewBag.neirong = youXi.TieziXiangXi.Where(p => p.TieZiID == id).ToList();
                }

            }
            return View();
        }
    }
}
