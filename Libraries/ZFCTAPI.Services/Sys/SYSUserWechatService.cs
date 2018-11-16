using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Sys
{
    public interface ISYSUserWechatService : IRepository<SYS_User_Wechat>
    {
        /// <summary>
        /// 通过OpenId
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        SYS_User_Wechat GetByOpenId(string openid);

        /// <summary>
        /// 通过CustomerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        SYS_User_Wechat GetByCustomerId(string customerId);
    }

    public class SYSUserWechatService : Repository<SYS_User_Wechat>, ISYSUserWechatService
    {
        public SYS_User_Wechat GetByCustomerId(string customerId)
        {
            return _Conn.QueryFirstOrDefault<SYS_User_Wechat>($"select * from SYS_User_Wechat where cst_customer_id = '{customerId}'");
        }

        public SYS_User_Wechat GetByOpenId(string openid)
        {
            return _Conn.QueryFirstOrDefault<SYS_User_Wechat>($"select * from SYS_User_Wechat where cst_user_uopenid = '{openid}'");
        }
    }
}