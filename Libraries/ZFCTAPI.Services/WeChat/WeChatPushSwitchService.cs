using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Data.WeChat;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Services.WeChat;

namespace ZFCTAPI.Services.Sys
{
    public interface IWeChatPushSwitchService : IRepository<WeChatPushSwitch>
    {
        /// <summary>
        /// 根据userid或type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        WeChatPushSwitch GetByUserIdAndType(string userId, string type);
    }

    public class WeChatPushSwitchService : Repository<WeChatPushSwitch>, IWeChatPushSwitchService
    {
        public WeChatPushSwitch GetByUserIdAndType(string userId, string type)
        {
            return _Conn.QueryFirstOrDefault<WeChatPushSwitch>($"select * from WeChat_Push_Switch where cst_customer_Id ={userId} and template_type = '{type}'");
        }
    }
}