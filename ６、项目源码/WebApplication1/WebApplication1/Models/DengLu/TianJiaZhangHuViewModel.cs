using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.RenYuanGuanLi
{
    /// <summary>
    /// 添加账户视图模型（字段与数据库 YongHu 表严格对应）
    /// </summary>
    public class TianJiaZhangHuViewModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度需在3-50字符之间")]
        [Display(Name = "用户名")]
        public string YongHuMing { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        [Display(Name = "邮箱")]
        public string YouXiang { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度需在6-20位之间")]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        public string MiMa { get; set; }

        [Required(ErrorMessage = "请确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("MiMa", ErrorMessage = "两次输入的密码不一致")]
        public string QueRenMiMa { get; set; }
    }
}