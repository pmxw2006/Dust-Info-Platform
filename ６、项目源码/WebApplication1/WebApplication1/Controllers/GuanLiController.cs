using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Models.GeRenZhongXin;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class GuanLiController : Controller
    {
        /// <summary>
        /// GET：显示管理端界面首页判断是否登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            // 未登录则跳转登录页
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            return View(new XiuGaiMiMaViewModel());
        }

        /// <summary>
        /// GET：显示用户封禁管理页面
        /// 需要管理员登录后才能访问，未登录则跳转到登录页
        /// </summary>
        [HttpGet]
        public ActionResult FengJin()
        {
            // 检查 Session 中是否存在登录用户标识
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            // 返回封禁管理视图（页面使用 Ajax 加载数据）
            return View();
        }

        /// <summary>
        /// GET：获取用户列表（JSON 接口，用于表格和分页）
        /// 支持按用户名、邮箱、用户ID进行组合搜索
        /// </summary>
        /// <param name="yongHuMing">搜索 - 用户名（模糊匹配）</param>
        /// <param name="youXiang">搜索 - 邮箱（模糊匹配）</param>
        /// <param name="yongHuID">搜索 - 用户ID（精确匹配）</param>
        /// <param name="page">当前页码，默认第1页</param>
        /// <param name="pageSize">每页显示数量，默认5条</param>
        /// <returns>JSON 对象，包含用户列表、总记录数、总页数等信息</returns>
        [HttpGet]
        public JsonResult LieBiao(string yongHuMing, string youXiang, int? yongHuID, int page = 1, int pageSize = 5)
        {
            using (var db = new YouXiEntities())
            {
                // 构建查询，可叠加条件
                var query = db.YongHu.AsQueryable();

                // 按用户名模糊搜索
                if (!string.IsNullOrWhiteSpace(yongHuMing))
                    query = query.Where(u => u.YongHuMing.Contains(yongHuMing));

                // 按邮箱模糊搜索
                if (!string.IsNullOrWhiteSpace(youXiang))
                    query = query.Where(u => u.YouXiang.Contains(youXiang));

                // 按用户ID精确搜索
                if (yongHuID.HasValue)
                    query = query.Where(u => u.YongHuID == yongHuID.Value);

                // 计算总记录数
                int total = query.Count();

                // 计算总页数（向上取整）
                int totalPages = (int)Math.Ceiling((double)total / pageSize);
                if (totalPages < 1) totalPages = 1; // 保证分页始终存在，无数据时显示第1页

                // 分页查询：按ID倒序，跳过前面的记录，取当前页数据
                var list = query.OrderByDescending(u => u.YongHuID)
                                .Where(u => u.QuanXian == "0")// 只显示普通用户，管理员不显示在列表中
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(u => new {
                                    u.YongHuID,       // 用户ID
                                    u.YongHuMing,     // 用户名
                                    u.YouXiang,       // 邮箱
                                    u.ZhuangTai,      // 账号状态（1正常，0封禁）
                                    u.QuanXian        // 权限
                                })
                                .ToList();

                // 返回 JSON 数据
                return Json(new { list, total, page, pageSize, totalPages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// POST：封禁用户
        /// 将指定用户的状态设置为封禁，并写入封禁日志
        /// </summary>
        /// <param name="yongHuID">要封禁的用户ID</param>
        /// <param name="yuanYin">封禁原因（由操作人员填写）</param>
        /// <returns>JSON 对象，包含操作结果和提示信息</returns>
        [HttpPost]
        public JsonResult FengJinYongHu(int yongHuID, string yuanYin)
        {
            // 检查登录状态
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            // 从 Session 中获取当前操作人（管理员用户名）
            string caoZuoRen = Session["ID"].ToString();

            // 调用服务层执行封禁操作
            bool ok = YongHuYanZhengFuWu.FengJinYongHu(yongHuID, yuanYin, caoZuoRen);

            // 返回结果给前端
            return Json(new { success = ok, message = ok ? "封禁成功" : "封禁失败，用户可能已处于封禁状态" });
        }

        /// <summary>
        /// POST：解封用户
        /// 将指定用户的状态恢复为正常，并更新封禁日志的解封时间
        /// </summary>
        /// <param name="yongHuID">要解封的用户ID</param>
        /// <returns>JSON 对象，包含操作结果和提示信息</returns>
        [HttpPost]
        public JsonResult JieFengYongHu(int yongHuID)
        {
            // 检查登录状态
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            // 调用服务层执行解封操作
            bool ok = YongHuYanZhengFuWu.JieFengYongHu(yongHuID);

            // 返回结果给前端
            return Json(new { success = ok, message = ok ? "解封成功" : "解封失败，用户可能未被封禁" });
        }
        /// <summary>
        /// GET：帖子封禁管理页面
        /// </summary>
        [HttpGet]
        public ActionResult TieZiFengJin()
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");
            return View();
        }

        /// <summary>
        /// 获取帖子列表（JSON分页）
        /// </summary>
        [HttpGet]
        public JsonResult TieZiLieBiao(string tieZiMing, string tieZiID, int page = 1, int pageSize = 5)
        {
            using (var db = new YouXiEntities())
            {
                var query = db.TieZi.Join(db.YongHu,
                    t => t.YongHuID,
                    y => y.YongHuID,
                    (t, y) => new { t, y.YongHuMing });

                if (!string.IsNullOrWhiteSpace(tieZiID) && int.TryParse(tieZiID, out int id))
                    query = query.Where(x => x.t.TieZiID == id);
                if (!string.IsNullOrWhiteSpace(tieZiMing))
                    query = query.Where(x => x.t.TieZiMing.Contains(tieZiMing));

                int total = query.Count();
                int totalPages = (int)Math.Ceiling((double)total / pageSize);
                if (totalPages < 1) totalPages = 1;

                var list = query.OrderByDescending(x => x.t.TieZiID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new
                    {
                        x.t.TieZiID,
                        x.t.TieZiMing,
                        x.YongHuMing,
                        FaBuShiJian = x.t.FaBuShiJian.ToString(),
                        x.t.ZhuangTai
                    })
                    .ToList();

                return Json(new { list, total, page, pageSize, totalPages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 封禁帖子（下架并记录日志）
        /// </summary>
        [HttpPost]
        public JsonResult FengJinTieZi(int tieZiID, string yuanYin)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            string caoZuoRen = Session["ID"].ToString();
            using (var db = new YouXiEntities())
            {
                var tieZi = db.TieZi.FirstOrDefault(t => t.TieZiID == tieZiID);
                if (tieZi == null || tieZi.ZhuangTai == 0)
                    return Json(new { success = false, message = "帖子不存在或已封禁" });

                tieZi.ZhuangTai = 0;
                db.TieZiXiaJiaRiZhi.Add(new TieZiXiaJiaRiZhi
                {
                    TieZiID = tieZiID,
                    YuanYin = yuanYin ?? "管理操作",
                    XiaJiaShiJian = DateTime.Now,
                    CaoZuoRen = caoZuoRen
                });
                db.SaveChanges();
                return Json(new { success = true, message = "封禁成功" });
            }
        }

        /// <summary>
        /// 解封帖子（恢复上架并更新日志）
        /// </summary>
        [HttpPost]
        public JsonResult JieFengTieZi(int tieZiID)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            using (var db = new YouXiEntities())
            {
                var tieZi = db.TieZi.FirstOrDefault(t => t.TieZiID == tieZiID);
                if (tieZi == null || tieZi.ZhuangTai != 0)
                    return Json(new { success = false, message = "帖子未被封禁" });

                tieZi.ZhuangTai = 1;
                // 更新最近的未恢复日志
                var log = db.TieZiXiaJiaRiZhi
                    .Where(l => l.TieZiID == tieZiID && l.HuiFuShiJian == null)
                    .OrderByDescending(l => l.XiaJiaShiJian)
                    .FirstOrDefault();
                if (log != null) log.HuiFuShiJian = DateTime.Now;
                db.SaveChanges();
                return Json(new { success = true, message = "解封成功" });
            }
        }
        /// <summary>
        /// GET：反馈管理页面
        /// </summary>
        [HttpGet]
        public ActionResult GuanLiFanKui()
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");
            return View();
        }

        /// <summary>
        /// 获取反馈列表（状态0优先，时间倒序，分页10条）
        /// </summary>
        [HttpGet]
        public JsonResult HuoQuFanKuiLieBiao(int page = 1, int pageSize = 10)
        {
            using (var db = new YouXiEntities())
            {
                var query = db.FanKui
                    .OrderBy(f => f.ZhuangTai)        // 0 在前
                    .ThenByDescending(f => f.FanKuiShiJian);

                int total = query.Count();
                int totalPages = (int)Math.Ceiling((double)total / pageSize);
                if (totalPages < 1) totalPages = 1;

                var list = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(f => new
                    {
                        f.FanKuiID,
                        f.YongHuID,
                        f.FanKuiNeiRong,
                        f.FanKuiShiJian,
                        f.ZhuangTai
                    })
                    .ToList();

                return Json(new { list, total, page, totalPages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// AI 整理反馈：管理员输入指令，AI 分析所有未处理反馈，将无效的标记为已处理
        /// </summary>
        [HttpPost]
        public async Task<JsonResult> AiZhengLiFanKui(string adminMessage)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            if (string.IsNullOrWhiteSpace(adminMessage))
                return Json(new { success = false, message = "请输入指令" });

            try
            {
                // 1. 获取所有未处理反馈
                List<FanKui> unprocessedFeedbacks;
                using (var db = new YouXiEntities())
                {
                    unprocessedFeedbacks = db.FanKui.Where(f => f.ZhuangTai == 0).ToList();
                }

                if (!unprocessedFeedbacks.Any())
                    return Json(new { success = true, message = "没有未处理的反馈", numUpdated = 0 });

                // 2. 构造 AI 提示词
                var sb = new StringBuilder();
                sb.AppendLine("以下是用户反馈列表，每条反馈有ID、用户ID和内容。请根据管理员的要求，分析每条反馈是否'无效'（例如广告、无意义内容、重复等）。返回一个JSON数组，包含需要标记为已处理的反馈ID。格式：[123, 456]");
                sb.AppendLine("管理员要求：" + adminMessage);
                foreach (var f in unprocessedFeedbacks)
                {
                    sb.AppendLine($"ID:{f.FanKuiID} | 用户{f.YongHuID}: {f.FanKuiNeiRong}");
                }

                // 3. 调用 Ollama
                var reply = await QueryOllama(sb.ToString());

                // 4. 解析 AI 返回的 ID 列表
                List<int> idsToUpdate = new List<int>();
                try
                {
                    idsToUpdate = JsonConvert.DeserializeObject<List<int>>(reply);
                }
                catch
                {
                    // 可能 AI 返回了其他格式，尝试提取数字
                    foreach (var match in System.Text.RegularExpressions.Regex.Matches(reply, @"\d+"))
                    {
                        if (int.TryParse(match.ToString(), out int id))
                            idsToUpdate.Add(id);
                    }
                }

                // 5. 更新数据库
                int numUpdated = 0;
                if (idsToUpdate.Any())
                {
                    using (var db = new YouXiEntities())
                    {
                        var feedbacks = db.FanKui.Where(f => idsToUpdate.Contains(f.FanKuiID) && f.ZhuangTai == 0);
                        foreach (var f in feedbacks)
                        {
                            f.ZhuangTai = 1;
                            numUpdated++;
                        }
                        db.SaveChanges();
                    }
                }

                return Json(new { success = true, message = $"整理完成，已将 {numUpdated} 条无效反馈标记为已处理。" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "AI 服务异常：" + ex.Message });
            }
        }

        /// <summary>
        /// 手动标记反馈为已处理
        /// </summary>
        [HttpPost]
        public JsonResult ChuLiFanKui(int fanKuiID)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            using (var db = new YouXiEntities())
            {
                var fanKui = db.FanKui.FirstOrDefault(f => f.FanKuiID == fanKuiID);
                if (fanKui == null)
                    return Json(new { success = false, message = "反馈不存在" });

                fanKui.ZhuangTai = 1;
                db.SaveChanges();
                return Json(new { success = true, message = "已标记为已处理" });
            }
        }

        // 调用 Ollama 的通用方法
        private async Task<string> QueryOllama(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                var requestBody = new
                {
                    model = "deepseek-r1:8B",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    stream = false
                };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:11434/api/chat", content);
                var responseJson = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseJson);
                return result?.message?.content?.ToString() ?? "";
            }
        }

    }
}