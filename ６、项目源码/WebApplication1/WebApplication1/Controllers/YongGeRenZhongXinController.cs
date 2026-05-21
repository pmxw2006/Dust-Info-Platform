using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.GeRenZhongXin;
using WebApplication1.Models.YongHuB;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class YongGeRenZhongXinController : Controller
    {
        [HttpGet]
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

        [HttpGet]
        public ActionResult GeReXiangXi()
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            int yongHuID = int.Parse(Session["ID"].ToString());
            using (var db = new YouXiEntities())
            {
                var yonghu = db.YongHu.FirstOrDefault(y => y.YongHuID == yongHuID);
                if (yonghu == null)
                    return RedirectToAction("DengLuYe", "DengLu");
                var model = new GeRenXiangXi
                {
                    YongHuID = yonghu.YongHuID,
                    YongHuMing = yonghu.YongHuMing
                };
                return View(model);
            }
        }
        public static int YID;
        public static bool aa = false;

        public ActionResult QiTaYongHu(int page = 1, int pageSize = 10)  // ← 加了两个参数
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            if (Request["id"] == null)
            {
                Response.Redirect("/YongHu/Index");
            }
           var danqianID = MiMaFuWu .JieMi(Request["id"].ToString());
            if (YID == danqianID)
            {
                aa = true;
            }
            else
            {
                aa = false;
            }
            if (!aa)
            {
                YID = danqianID;
                aa = true;
            } 

            YongHu yh = YongHuXinXi.YongHuXinxi(YID);
            ViewBag.Yid = int.Parse(Session["ID"].ToString());
            ViewBag.YongHu = yh;
            ViewBag.YongHuID = Request["id"];  // ← 加：供前端分页链接使用


            if (yh.ZhuangTai == 0)
            {
                ViewBag.TieZi = null;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 0;
                return View();
            }
            else
            {
                var tiezi = HuoQuTieZi.TieZiXinXi(YID);
                List<YongHuJiBenXinxi> tz = new List<YongHuJiBenXinxi>();
                foreach (var i in tiezi as List<TieZi>)
                {
                    YongHuJiBenXinxi yhjbxx = new YongHuJiBenXinxi();
                    yhjbxx.TieZiID = i.TieZiID;
                    yhjbxx.TieZiMing = i.TieZiMing;
                    yhjbxx.FaBuShiJian = i.FaBuShiJian;
                    // 建议保留判空，避免无图时崩溃
                    var photo = HuoQuTieZi.ZhaoPianXinxiONE(i.TieZiID);
                    yhjbxx.Lujing = photo?.Lujing ?? "YongHu.jpeg";
                    tz.Add(yhjbxx);
                }

                // ===== 新增分页处理（不破坏原有逻辑） =====
                //总数据条数
                // ========== 新增翻页边界修正（只改此处） ==========
                int totalCount = tz.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                if (totalPages == 0) totalPages = 1;   // 无数据时总页数为1，避免除零或空页

                // 修正当前页码：不能小于1，也不能大于总页数
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                // 根据修正后的page获取数据（如果totalCount为0，Skip/Take不影响结果）
                var pagedList = tz
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                // ================================================

                ViewBag.TieZi = pagedList;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
            }

            return View();
        }
        //帖子的删除
        public ActionResult TieZiDel(int id)
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");
            using (YouXiEntities db = new YouXiEntities())
            {
                TieZi tieZi = db.TieZi.FirstOrDefault(t => t.TieZiID == id);
                if (tieZi != null)
                {
                    tieZi.ZhuangTai = 2; // 修改帖子的状态，标记为已删除
                    db.SaveChanges();

                }
                return RedirectToAction("GeReXiangXi", "YongGeRenZhongXin", new { id = tieZi.TieZiID });
            }

        }
        [HttpGet]
        public ActionResult TieZiEdit(int? id)
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");
            if (id == null)
                return RedirectToAction("Index", "YongHu");
            using (YouXiEntities db = new YouXiEntities())
            {
                int userId = int.Parse(Session["ID"].ToString());
                // 增加归属验证：只能编辑自己的帖子
                TieZi tieZi = db.TieZi.FirstOrDefault(t => t.TieZiID == id && t.YongHuID == userId);
                if (tieZi == null)
                    return RedirectToAction("Index", "YongHu");  // 无权限或不存在

                ViewData["TieZiID"] = tieZi.TieZiID;
                ViewData["TieZiMing"] = tieZi.TieZiMing;

                TieziXiangXi tieziXiangXi = db.TieziXiangXi.FirstOrDefault(t => t.TieZiID == id);
                ViewData["XiangXIID"] = tieziXiangXi?.XiangXIID ?? 0;
                ViewData["NeiRong"] = tieziXiangXi?.NeiRong ?? "";

                // 获取现有图片列表，生成前端需要的 JSON
                var photos = db.ZhaoPian
                .Where(z => z.TieZiID == id)
                .ToList()   // ← 必须先执行查询，拿到内存中的列表
                .Select(z => new {
                    id = z.ZhaoPianID,
                    url = Url.Content("~/Img/" + z.Lujing)
                }).ToList();
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ViewBag.ExistingImagesJson = serializer.Serialize(photos);
            }
            return View();
        }
        [HttpPost]
        public ActionResult TieZiEdit(int TieZiID, int? XiangXIID, string TieZiMing, string NeiRong, string keepImageIds, HttpPostedFileBase[] files)
        {
            // 登录校验
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            // 后端字段验证
            if (string.IsNullOrWhiteSpace(TieZiMing) || string.IsNullOrWhiteSpace(NeiRong))
                return Json(new { success = false, message = "标题和内容不能为空" });

            try
            {
                int userId = int.Parse(Session["ID"].ToString());
                using (YouXiEntities db = new YouXiEntities())
                {
                    // 确认帖子存在且属于当前用户
                    var tieZi = db.TieZi.FirstOrDefault(t => t.TieZiID == TieZiID && t.YongHuID == userId);
                    if (tieZi == null)
                        return Json(new { success = false, message = "帖子不存在或无权编辑" });

                    // 更新标题
                    tieZi.TieZiMing = TieZiMing.Trim();

                    // 更新内容
                    var xiangXi = db.TieziXiangXi.FirstOrDefault(x => x.TieZiID == TieZiID);
                    if (xiangXi != null)
                        xiangXi.NeiRong = NeiRong.Trim();
                    else
                        db.TieziXiangXi.Add(new TieziXiangXi { TieZiID = TieZiID, NeiRong = NeiRong.Trim() });

                    // --- 图片处理 ---
                    // 1. 解析需保留的图片ID
                    List<int> keepIds = new List<int>();
                    if (!string.IsNullOrWhiteSpace(keepImageIds))
                    {
                        keepIds = keepImageIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(int.Parse).ToList();
                    }

                    // 2. 删除未被保留的已有图片（同时删除物理文件）
                    var existingPhotos = db.ZhaoPian.Where(z => z.TieZiID == TieZiID).ToList();
                    foreach (var photo in existingPhotos)
                    {
                        if (!keepIds.Contains(photo.ZhaoPianID))
                        {
                            string physicalPath = Server.MapPath("~/Img/" + photo.Lujing);
                            if (System.IO.File.Exists(physicalPath))
                                System.IO.File.Delete(physicalPath);
                            db.ZhaoPian.Remove(photo);
                        }
                    }

                    // 3. 添加新上传的图片
                    if (files != null && files.Length > 0)
                    {
                        // 校验总张数
                        int totalAfterAdd = keepIds.Count + files.Length;
                        if (totalAfterAdd > 9)
                            return Json(new { success = false, message = "图片总数不能超过9张" });

                        string folder = Server.MapPath("~/Img");
                        foreach (var file in files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                string newName = Guid.NewGuid().ToString("N").Substring(0, 16) + Path.GetExtension(file.FileName);
                                string savePath = Path.Combine(folder, newName);
                                file.SaveAs(savePath);

                                db.ZhaoPian.Add(new ZhaoPian
                                {
                                    TieZiID = TieZiID,
                                    Lujing = newName
                                });
                            }
                        }
                    }

                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "修改失败：" + ex.Message });
            }
        }
    }
}