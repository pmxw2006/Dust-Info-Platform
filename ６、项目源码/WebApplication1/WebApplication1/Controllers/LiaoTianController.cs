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
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class LiaoTianController : Controller
    {
        // 聊天室视图（默认房间1）
        [HttpGet]
        public ActionResult LiaoTianShi(int id = 1)
        {
            if (Session["ID"] == null)
                return RedirectToAction("DengLuYe", "DengLu");

            ViewBag.RoomId = id;
            ViewBag.CurrentUserId = Session["ID"];  
            return View();
        }

        [HttpGet]
        public JsonResult HuoQuXiaoXi(int roomId, int take = 50)
        {
            if (Session["ID"] == null)
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            using (var db = new YouXiEntities())
            {
                var query = Enumerable.Empty<dynamic>().AsQueryable();

                if (roomId == 1)
                    query = db.LiaoTianShi1.OrderByDescending(m => m.FaSongShiJian).Take(take)
                        .Select(m => new {
                            m.XiaoXiID,
                            m.YongHuID,
                            m.NeiRong,
                            FaSongShiJian = m.FaSongShiJian.ToString(),
                            YongHuMing = db.YongHu.Where(y => y.YongHuID == m.YongHuID).Select(y => y.YongHuMing).FirstOrDefault()
                        });
                else if (roomId == 2)
                    query = db.LiaoTianShi2.OrderByDescending(m => m.FaSongShiJian).Take(take)
                        .Select(m => new {
                            m.XiaoXiID,
                            m.YongHuID,
                            m.NeiRong,
                            FaSongShiJian = m.FaSongShiJian.ToString(),
                            YongHuMing = db.YongHu.Where(y => y.YongHuID == m.YongHuID).Select(y => y.YongHuMing).FirstOrDefault()
                        });
                else if (roomId == 3)
                    query = db.LiaoTianShi3.OrderByDescending(m => m.FaSongShiJian).Take(take)
                        .Select(m => new {
                            m.XiaoXiID,
                            m.YongHuID,
                            m.NeiRong,
                            FaSongShiJian = m.FaSongShiJian.ToString(),
                            YongHuMing = db.YongHu.Where(y => y.YongHuID == m.YongHuID).Select(y => y.YongHuMing).FirstOrDefault()
                        });
                else
                    return Json(new { success = false, message = "房间号无效" }, JsonRequestBehavior.AllowGet);

                var messages = query.ToList();
                messages.Reverse();
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
        }

        // 发送消息
        [HttpPost]
        public JsonResult FaSongXiaoXi(int roomId, string neiRong)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            if (string.IsNullOrWhiteSpace(neiRong))
                return Json(new { success = false, message = "内容不能为空" });

            int yongHuID = Convert.ToInt32(Session["ID"]);

            using (var db = new YouXiEntities())
            {
                if (roomId == 1)
                    db.LiaoTianShi1.Add(new LiaoTianShi1 { YongHuID = yongHuID, NeiRong = neiRong.Trim(), FaSongShiJian = DateTime.Now });
                else if (roomId == 2)
                    db.LiaoTianShi2.Add(new LiaoTianShi2 { YongHuID = yongHuID, NeiRong = neiRong.Trim(), FaSongShiJian = DateTime.Now });
                else if (roomId == 3)
                    db.LiaoTianShi3.Add(new LiaoTianShi3 { YongHuID = yongHuID, NeiRong = neiRong.Trim(), FaSongShiJian = DateTime.Now });
                else
                    return Json(new { success = false, message = "房间号无效" });

                db.SaveChanges();
                return Json(new { success = true });
            }
        }
        // 新增：与 AI 对话（不存储）
        [HttpPost]
        public async Task<JsonResult> AiChat(string message)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, reply = "未登录" });

            if (string.IsNullOrWhiteSpace(message))
                return Json(new { success = false, reply = "消息不能为空" });

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var requestBody = new
                    {
                        model = "deepseek-r1:8B",
                        messages = new[]
                        {
                            new { role = "user", content = message }
                        },
                        stream = false
                    };
                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:11434/api/chat", content);
                    var responseJson = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseJson);
                    string reply = result?.message?.content?.ToString() ?? "AI 无应答";
                    return Json(new { success = true, reply = reply });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, reply = "AI 服务异常：" + ex.Message });
            }
        }
        /// <summary>
        /// POST：提交用户反馈
        /// </summary>
        [HttpPost]
        public JsonResult TiJiaoFanKui(string neiRong)
        {
            if (Session["ID"] == null)
                return Json(new { success = false, message = "未登录" });

            if (string.IsNullOrWhiteSpace(neiRong))
                return Json(new { success = false, message = "内容不能为空" });

            int yongHuID = Convert.ToInt32(Session["ID"]);

            using (var db = new YouXiEntities())
            {
                db.FanKui.Add(new FanKui
                {
                    YongHuID = yongHuID,
                    FanKuiNeiRong = neiRong.Trim(),
                    FanKuiShiJian = DateTime.Now,
                    ZhuangTai = 0      // 0 未处理，1 已处理
                });
                db.SaveChanges();
                return Json(new { success = true, message = "感谢您的反馈！" });
            }
        }
    }
}