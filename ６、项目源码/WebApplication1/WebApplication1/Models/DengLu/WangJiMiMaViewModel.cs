using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DengLu
{
    /// <summary>
    /// 忘记密码 - 通过账户和注册邮箱重置密码
    /// </summary>
    public class WangJiMiMaViewModel
    {
        [Required(ErrorMessage = "账户不能为空")]
        [Display(Name = "账户")]
        public string YongHuMing { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        [Display(Name = "注册邮箱")]
        public string YouXiang { get; set; }

        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度需在6-20位之间")]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string XinMiMa { get; set; }

        [Required(ErrorMessage = "请再次输入新密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("XinMiMa", ErrorMessage = "两次输入的密码不一致")]
        public string QueRenXinMiMa { get; set; }
    }
}