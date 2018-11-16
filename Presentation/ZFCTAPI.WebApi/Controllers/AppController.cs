using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Services.Helpers;

namespace ZFCTAPI.WebApi.Controllers
{
    public class AppController : Controller
    {
        private IHttpContextAccessor _accessor;
        private readonly IUserAgentHelper _userAgentHelper;
        public AppController(IHttpContextAccessor accessor,
            IUserAgentHelper userAgentHelper)
        {
            _accessor = accessor;
            _userAgentHelper = userAgentHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DefaultPage(string msg, ReturnPageType type)
        {
            ViewBag.Mobile = "0";
            ViewBag.ReturnType = (int) type;
            ViewBag.Mobile = _userAgentHelper.IsIphone() ? "1" : "0";
            ViewBag.Msg = msg;
            return View();
        }
    }
}