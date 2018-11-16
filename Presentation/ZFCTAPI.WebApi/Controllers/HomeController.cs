using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZFCTAPI.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }

        /// <summary>
        /// 给其他调用接口热身使用
        /// </summary>
        /// <returns></returns>
        public IActionResult WarmUp()
        {
            return Content("yes");
        }
    }
}