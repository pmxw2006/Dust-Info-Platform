using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.GeRenZhongXin
{
    public class XiuGaiMiMaViewModel
    {
        [Required(ErrorMessage = "当前密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string DangQianMiMa { get; set; }

        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度需在6-20位之间")]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string XinMiMa { get; set; }

        [Required(ErrorMessage = "请确认新密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("XinMiMa", ErrorMessage = "两次输入的密码不一致")]
        public string QueRenMiMa { get; set; }
    }
}