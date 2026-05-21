using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Services
{
    /// <summary>
    /// 权限跳转服务：根据权限整数值返回对应的跳转 URL
    /// 权限字典：0 = 普通用户，1 = 管理员
    /// </summary>
    public static class QuanXianTiaoZhuanFuWu
    {
        /// <summary>
        /// 根据权限整数值获取跳转目标 URL
        /// </summary>
        /// <param name="quanXian">数据库中 QuanXian 字段的值（int 类型）</param>
        /// <returns>跳转 URL；如果权限值不在定义范围内则返回 null</returns>
        public static string HuoQuTiaoZhuanUrl(int quanXian)
        {
            switch (quanXian)
            {
                case 0:  // 普通用户
                    return "/YongHu/Index";
                case 1:  // 管理员（系统管理员）
                    return "/GuanLi/Index";
                default: // 未知权限值
                    return null;
            }
        }
    }
}