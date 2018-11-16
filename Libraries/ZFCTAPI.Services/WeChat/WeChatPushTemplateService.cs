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
    public interface IWeChatPushTemplateService : IRepository<WeChatPushTemplate>
    {
        /// <summary>
        /// 根据type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        WeChatPushTemplate GetByType(string type);
    }

    public class WeChatPushTemplateService : Repository<WeChatPushTemplate>, IWeChatPushTemplateService
    {
        public WeChatPushTemplate GetByType(string type)
        {
            return _Conn.QueryFirstOrDefault<WeChatPushTemplate>($"select * from WeChat_Push_Template where type = '{type}' and enabled = 1");
        }
    }
}