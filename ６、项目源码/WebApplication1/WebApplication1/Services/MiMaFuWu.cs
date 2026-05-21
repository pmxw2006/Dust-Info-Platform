using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    /// <summary>
    /// 密码服务类，负责计算密码的哈希值，使用 SHA256 算法，并结合盐值增强安全性。
    /// </summary>
    public class MiMaFuWu
    {
        /// <summary>
        /// 计算 SHA256 哈希（密码 + 盐值）
        /// </summary>
        /// <param name="mingWenMiMa">明文密码</param>
        /// <param name="yanZhi">盐值</param>
        /// <returns>小写十六进制哈希字符串</returns>
        public static string JiSuanHaXi(string mingWenMiMa, string yanZhi)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string combined = mingWenMiMa + yanZhi;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static int JieMi(string YongHuUID)
        {
            using (YouXiEntities youxi = new YouXiEntities())
            {
                var YongHu = youxi.YongHu.ToList();
                int YongHuID = 0;
                foreach (var i in YongHu as List<YongHu>)
                {
                    var yonghuUID = MiMaFuWu.JiSuanHaXi(i.YongHuID.ToString(), i.YanZhi);
                    if (YongHuUID == yonghuUID)
                    {
                        YongHuID = i.YongHuID;
                    }
                }
                return YongHuID;
            }
        }
    }
}