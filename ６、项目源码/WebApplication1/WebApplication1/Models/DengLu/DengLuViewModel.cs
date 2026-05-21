using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DengLu
{
    public class DengLuViewModel
    {
        [Required(ErrorMessage = "账户不能为空")]
        [Display(Name = "账户")]
        public string YongHuMing { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string MiMa { get; set; }

        [Display(Name = "记住密码")]
        public bool JiZhuMiMa { get; set; }
    }
}