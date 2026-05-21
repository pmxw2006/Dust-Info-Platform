using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.GeRenZhongXin;
using WebApplication1.Models.RenYuanGuanLi;

namespace WebApplication1.Services
{
    /// <summary>
    /// 用户验证服务类，负责处理用户登录验证、获取用户信息、修改密码等与用户身份相关的操作。
    /// </summary>
    public static class YongHuYanZhengFuWu
    {
        /// <summary>
        /// 验证用户登录凭证
        /// 验证顺序：1. 用户是否存在 → 2. 账号是否被封禁 → 3. 密码是否正确
        /// </summary>
        /// <param name="yongHuID">用户名</param>
        /// <param name="mingWenMiMa">明文密码</param>
        /// <returns>成功返回用户实体，失败返回 null</returns>
        public static YongHu YanZhengDengLu(int yongHuID, string mingWenMiMa)
        {
            using (var db = new YouXiEntities())
            {
                try
                {
                    // 1. 根据用户名查找用户
                    var user = db.YongHu.FirstOrDefault(u => u.YongHuID == yongHuID);
                    if (user == null)
                        return null;  // 用户不存在

                    // 2. 检查账号是否被封禁（ZhuangTai == 0 表示封禁）
                    if (user.ZhuangTai == 0)
                        return null;  // 账号已被封禁

                    // 3. 验证密码
                    string inputHash = MiMaFuWu.JiSuanHaXi(mingWenMiMa, user.YanZhi);
                    if (inputHash != user.MiMaHaXi)
                        return null;  // 密码错误
                    return user;  // 验证通过

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据用户ID获取用户实体
        /// </summary>
        public static YongHu HuoQuYongHuByMingCheng(int YongHuID)
        {
            using (var db = new YouXiEntities())
            {
                return db.YongHu.FirstOrDefault(u => u.YongHuID == YongHuID);
            }
        }

        /// <summary>
        /// 验证用户身份（账户 + 邮箱是否匹配）
        /// </summary>
        public static bool YanZhengYongHuShenFen(int YongHuID, string youXiang)
        {
            using (var db = new YouXiEntities())
            {
                return db.YongHu.Any(u => u.YongHuID == YongHuID && u.YouXiang == youXiang);
            }
        }

        /// <summary>
        /// 修改密码（直接更新为新密码）
        /// </summary>
        public static void XiuGaiMiMa(int YongHuID, string xinMiMa)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == YongHuID);
                if (user != null)
                {
                    string xinYanZhi = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
                    string xinHaXi = MiMaFuWu.JiSuanHaXi(xinMiMa, xinYanZhi);
                    user.YanZhi = xinYanZhi;
                    user.MiMaHaXi = xinHaXi;
                    db.SaveChanges();
                }
            }
        }
        /// <summary>
        /// 添加新用户（字段与数据库 YongHu 表对应）
        /// </summary>
        /// <returns>成功返回新用户的 YongHuID，失败返回 0</returns>
        public static int TianJiaYongHu(TianJiaZhangHuViewModel model)
        {
            using (var db = new YouXiEntities())
            {
                // 生成随机盐值（16位）
                string yanZhi = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
                // 计算密码哈希
                string miMaHaXi = MiMaFuWu.JiSuanHaXi(model.MiMa, yanZhi);

                var yongHu = new YongHu
                {
                    YongHuMing = model.YongHuMing.Trim(),
                    YouXiang = model.YouXiang.Trim(),
                    QuanXian = "0",
                    YanZhi = yanZhi,
                    MiMaHaXi = miMaHaXi,
                    ZhuangTai = 1,
                    ChuangJianShiJian = DateTime.Now
                };

                db.YongHu.Add(yongHu);
                db.SaveChanges();
                return yongHu.YongHuID;   // 返回数据库生成的自增ID
            }
        }
        /// <summary>
        /// 获取个人信息（根据当前登录用户名）
        /// </summary>
        public static GeRenXinXiViewModel HuoQuGeRenXinXi(int yongHuID)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == yongHuID);
                if (user == null) return null;

                return new GeRenXinXiViewModel
                {
                    YongHuID = user.YongHuID,
                    YongHuMing = user.YongHuMing,
                    YouXiang = user.YouXiang,
                    ZhuangTai = user.ZhuangTai,
                    ChuangJianShiJian = user.ChuangJianShiJian
                };
            }
        }

        /// <summary>
        /// 保存个人信息（更新用户名和邮箱）
        /// </summary>
        public static bool BaoCunGeRenXinXi(int currentYongHuID, GeRenXinXiViewModel model)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == currentYongHuID);
                if (user == null) return false;

                string newYongHuMing = model.YongHuMing.Trim();
                string newYouXiang = model.YouXiang.Trim();
                user.YongHuMing = newYongHuMing;
                user.YouXiang = newYouXiang;
                db.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// 验证当前密码是否正确
        /// </summary>
        /// <param name="yongHuID">当前登录用户ID</param>
        /// <param name="dangQianMiMa">输入的当前明文密码</param>
        /// <returns>正确返回 true，否则 false</returns>
        public static bool YanZhengDangQianMiMa(int yongHuID, string dangQianMiMa)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == yongHuID);
                if (user == null) return false;

                // 计算输入密码 + 数据库盐值的哈希
                string inputHash = MiMaFuWu.JiSuanHaXi(dangQianMiMa, user.YanZhi);
                return inputHash == user.MiMaHaXi;
            }
        }

        /// <summary>
        /// 修改密码（已登录用户）
        /// </summary>
        /// <param name="yongHuMing">当前登录用户名</param>
        /// <param name="xinMiMa">新明文密码</param>
        /// <returns>成功返回 true，失败返回 false</returns>
        public static bool DXiuGaiMiMa(int yongHuID, string xinMiMa)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == yongHuID);
                if (user == null) return false;

                // 生成新盐值（16位随机字符串）
                string xinYanZhi = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
                // 计算新密码哈希
                string xinHaXi = MiMaFuWu.JiSuanHaXi(xinMiMa, xinYanZhi);

                user.YanZhi = xinYanZhi;
                user.MiMaHaXi = xinHaXi;
                db.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// 封禁用户（更新状态并插入封禁日志）
        /// </summary>
        public static bool FengJinYongHu(int yongHuID, string yuanYin, string caoZuoRen)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == yongHuID);
                if (user == null || user.ZhuangTai == 0) return false;

                user.ZhuangTai = 0;
                db.FengJinRiZhi.Add(new FengJinRiZhi
                {
                    YongHuID = yongHuID,
                    YuanYin = yuanYin,
                    FengJinShiJian = DateTime.Now,
                    CaoZuoRen = caoZuoRen
                });
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 解封用户（更新状态并更新对应日志的解封时间）
        /// </summary>
        public static bool JieFengYongHu(int yongHuID)
        {
            using (var db = new YouXiEntities())
            {
                var user = db.YongHu.FirstOrDefault(u => u.YongHuID == yongHuID);
                if (user == null || user.ZhuangTai != 0) return false;

                user.ZhuangTai = 1;
                // 更新最近一条未解封的封禁日志
                var log = db.FengJinRiZhi
                             .Where(f => f.YongHuID == yongHuID && f.JieFengShiJian == null)
                             .OrderByDescending(f => f.FengJinShiJian)
                             .FirstOrDefault();
                if (log != null) log.JieFengShiJian = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }
    }
}