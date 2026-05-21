using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.GeRenZhongXin
{
    public class GeRenXinXiViewModel
    {
        public int YongHuID { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名在3-10字之间")]
        public string YongHuMing { get; set; }

        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        [StringLength(100)]
        public string YouXiang { get; set; }

        [Display(Name = "账号状态")]
        public int ZhuangTai { get; set; }

        [Display(Name = "注册时间")]
        public DateTime ChuangJianShiJian { get; set; }

        // 状态文本
        public string ZhuangTaiWenBen => ZhuangTai == 1 ? "正常" : "封禁";
    }
}