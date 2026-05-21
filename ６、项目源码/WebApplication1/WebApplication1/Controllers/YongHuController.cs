using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.GeRenZhongXin;
using WebApplication1.Models.YongHuB;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class YongHuController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            // 未登录则跳转登录页
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            return View(new XiuGaiMiMaViewModel());
        }
        /// <summary>
        /// 显示用户的帖子列表，并实现分页功能
        /// </summary>
        /// <param name="page">页数</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int page = 1)
        {
            using (YouXiEntities db = new YouXiEntities())
            {
                // 每页显示的数据条数
                int pageSize = 10;

                // 查询当前页的帖子数据（按 ShangJiaShiJian 降序）
                string sql = "select YongHuMing,TieZI.TieZiID,TieZiMing,FaBuShiJian,NeiRong ,Lujing from TieZi join TieziXiangXi on TieZi.TieZiID=TieziXiangXi.TieZiID join YongHu on YongHu.YongHuID= TieZi.YongHuID join ZhaoPian on TieZi.TieZiID=ZhaoPian.TieZiID";
                var data = db.Database.SqlQuery<Models.YongHuB.TieZIxxx>(sql).ToList();

                // 获取帖子总条数（用于分页计算）
                int totalCount = db.TieZi.Count();

                // 计算总页数（向上取整）
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // 将分页信息存入 ViewBag，供视图使用
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                // 将当前页的数据传递给视图
                return View(data);
            }
        }

        [HttpGet]
        public ActionResult TianJia()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("DengLuYe", "DengLu");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult TianJia(HttpPostedFileBase[] files, string TieZiMing, string NeiRong)
        {
            // 验证必填字段
            if (string.IsNullOrWhiteSpace(TieZiMing) || string.IsNullOrWhiteSpace(NeiRong))
            {
                return Json(new { success = false, message = "标题和内容不能为空" });
            }

            try
            {
                int userId = int.Parse(Session["ID"].ToString());
                DateTime now = DateTime.Now;

                // 检查是否重复（同一用户相同标题视为重复）
                using (YouXiEntities db = new YouXiEntities())
                {
                    bool exists = db.TieZi.Any(t => t.YongHuID == userId && t.TieZiMing == TieZiMing.Trim());
                    if (exists)
                    {
                        return Json(new { success = false, message = "您已发布过同名帖子，请修改标题" });
                    }

                    // 保存帖子主表
                    TieZi tiezi = new TieZi
                    {
                        YongHuID = userId,
                        TieZiMing = TieZiMing.Trim(),
                        FaBuShiJian = now,
                        ZhuangTai = 1
                    };
                    db.TieZi.Add(tiezi);
                    db.SaveChanges();   // 获取 TieZi数据库生成的 TieZiID

                    // 保存帖子详情
                    TieziXiangXi xiangxi = new TieziXiangXi
                    {
                        TieZiID = tiezi.TieZiID,
                        NeiRong = NeiRong.Trim()
                    };
                    db.TieziXiangXi.Add(xiangxi);

                    // 处理图片文件
                    if (files != null && files.Length > 0)
                    {
                        string folderPhysical = Server.MapPath("~/Img");
                        foreach (var file in files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                string newName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16) +
                                                 Path.GetExtension(file.FileName);
                                string filePath = Path.Combine(folderPhysical, newName);
                                file.SaveAs(filePath);

                                ZhaoPian zp = new ZhaoPian
                                {
                                    TieZiID = tiezi.TieZiID,
                                    Lujing = newName
                                };
                                db.ZhaoPian.Add(zp);
                            }
                        }
                        
                    }
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "发布失败：" + ex.Message });
            }
        }
        [HttpGet]
        public ActionResult TieZiNeiRong()
        {
            // 未登录则跳转登录页
            if (Session["ID"] == null)
            {
                return RedirectToAction("DengLuYe", "DengLu");
            }
            else
            {
                string a = Request["id"];
                var Yid = int.Parse(Session["ID"].ToString());
                if (a != null)
                {
                    int id = int.Parse(a);
                    using (YouXiEntities youXi = new YouXiEntities())
                    {
                        var Tiezi = youXi.TieZi.Where(p => p.TieZiID == id).ToList();
                        int yonghu = 0;
                        foreach (var i in Tiezi as List<TieZi>)
                        {
                            yonghu = i.YongHuID;//获取帖子所属用户ID
                        }
                        var yonghuming = youXi.YongHu.Where(p => p.YongHuID == yonghu).ToList();//根据用户ID获取用户表
                        foreach (var i in yonghuming as List<YongHu>)
                        {
                            ViewBag.yonghuming = i.YongHuMing;
                            ViewBag.YongHuID = i.YongHuID;
                            ViewBag.YongHuUID = MiMaFuWu.JiSuanHaXi(i.YongHuID.ToString(), i.YanZhi);//获取用户名，并进行哈希加密
                        }
                        ViewBag.tiezi = Tiezi;//获取帖子
                        ViewBag.zhaopian = youXi.ZhaoPian.Where(p => p.TieZiID == id).ToList();//根据帖子id获取照片
                        ViewBag.neirong = youXi.TieziXiangXi.Where(p => p.TieZiID == id).ToList();//根据帖子id获取内容
                        DianZan dz = youXi.DianZan.Where(p => p.TieZiID == id).FirstOrDefault(p => p.YongHuID == Yid);
                        ViewBag.Yid = Yid;
                        ViewBag.yesorno = dz?.ZhuangTai ?? 0;//获取该用户是否点赞
                        ViewBag.dianzanshu = youXi.DianZan.Where(p => p.TieZiID == id).Count(p => p.ZhuangTai == 1);//获取点赞数
                    }
                }
                else
                {
                    Response.Redirect("YongHu.Index");//url重写
                }
            }
            return View(new XiuGaiMiMaViewModel());
        }
        [HttpGet]
        public ActionResult xiugai(int id)
        {
            var Yid = int.Parse(Session["ID"].ToString());
            using (YouXiEntities youxi = new YouXiEntities())
            {
                DianZan dz = youxi.DianZan.Where(p => p.TieZiID == id).FirstOrDefault(p => p.YongHuID == Yid);//获取用户对帖子的状态
                if (dz == null)
                {
                    DianZan DZ = new DianZan();
                    DZ.YongHuID = Yid;
                    DZ.TieZiID = id;
                    DZ.ZhuangTai = 1;
                    youxi.DianZan.Add(DZ);
                    youxi.SaveChanges();
                }
                else
                {
                    dz.ZhuangTai = (dz.ZhuangTai == 0 ? 1 : 0);
                    youxi.SaveChanges();
                }
                Response.Redirect($"/YongHu/TieZiNeiRong?id={id}");
            }
            return View();
        }
    }
}