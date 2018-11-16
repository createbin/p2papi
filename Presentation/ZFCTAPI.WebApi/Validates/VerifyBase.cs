using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Core.Security;
using ZFCTAPI.Data.ApiModels;
using ZFCTAPI.Data.ApiModels.SubmitModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.SYS;
using ZFCTAPI.Services.UserInfo;

namespace ZFCTAPI.WebApi.Validates
{
    public class VerifyBase
    {
        private static int timespan = 1500000;

        /// <summary>
        /// 解析签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="submitModel"></param>
        /// <returns></returns>
        public static ReturnCode Sign<T>(BaseSubmitModel submitModel)
        {
            try
            {
                if (EngineContext.Current.Resolve<CommonConfig>().IsVerify == "true")
                    return ReturnCode.Success;
                var signature = RsaHelper.Decrypt(submitModel.Signature);
                T signModel = JsonConvert.DeserializeObject<T>(signature);
                submitModel.Signature = null;
                if (!JsonConvert.SerializeObject(signModel).Equals(JsonConvert.SerializeObject(submitModel)))
                    return ReturnCode.SignatureFailure;
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
                if (timestamp - Math.Abs(submitModel.TimeStamp) > timespan)
                    return ReturnCode.SignatureFailure;
                return ReturnCode.Success;
            }
            catch (Exception ex)
            {
                LogsHelper.WriteLog("解析签名时发生错误" + submitModel.ApiName + "错误详情：" + ex.Message);
                return ReturnCode.SignatureFailure;
            }
        }

        public static ReturnCode Token(string token, out int userid)
        {
            userid = 0;
            try
            {
                var str = CryptHelper.Decrypt(token);
                var tok = JsonConvert.DeserializeObject<TokenModel>(str);
                var dateNow = DateTime.Now.Ticks;
                if (dateNow > tok.StratDate && dateNow < tok.EndDate)
                {
                    var user = EngineContext.Current.Resolve<ICstUserInfoService>().GetUserInfo(customerId: tok.UserId);
                    if (user != null)
                    {
                        userid = user.Id;
                        EngineContext.Current.Resolve<IWorkContext>().CstUserInfo = user;
                        return ReturnCode.Success;
                    }

                    return ReturnCode.TokenEorr;
                }
                else
                    return ReturnCode.TokenEorr;
            }
            catch
            {
                return ReturnCode.TokenEorr;
            }
        }

        public static ReturnCode Token(string token, out CST_user_info user)
        {
            user = new CST_user_info();
            try
            {
                var str = CryptHelper.Decrypt(token);
                var tok = JsonConvert.DeserializeObject<TokenModel>(str);
                var dateNow = DateTime.Now.Ticks;
                if (dateNow > tok.StratDate && dateNow < tok.EndDate)
                {
                    user = EngineContext.Current.Resolve<ICstUserInfoService>().GetUserInfo(customerId: tok.UserId);
                    if (user != null) {
                        EngineContext.Current.Resolve<IWorkContext>().CstUserInfo = user;
                        return ReturnCode.Success;
                    }
                        
                    return ReturnCode.TokenEorr;
                }
                else
                    return ReturnCode.TokenEorr;
            }
            catch
            {
                return ReturnCode.TokenEorr;
            }
        }

        public static ReturnCode SignAndToken<T>(BaseSubmitModel submitModel, out int userid)
        {
            userid = 0;
            var signResult = Sign<T>(submitModel);
            if (!signResult.Equals(ReturnCode.Success))
                 return ReturnCode.SignatureFailure; 
            var tokenResult = Token(submitModel.Token, out userid);
            if (!tokenResult.Equals(ReturnCode.Success))
                 return ReturnCode.SignatureFailure; 
            return ReturnCode.Success;
        }

        public static ReturnCode SignAndToken<T>(BaseSubmitModel submitModel, out CST_user_info user)
        {
            user = new CST_user_info();
            var signResult = Sign<T>(submitModel);
            if (!signResult.Equals(ReturnCode.Success))
                return ReturnCode.SignatureFailure;
            var tokenResult = Token(submitModel.Token, out user);
            if (!tokenResult.Equals(ReturnCode.Success))
                return ReturnCode.SignatureFailure;
            return ReturnCode.Success;
        }

        public static void AnalysisToken(string token, out CST_user_info user)
        {
            user = new CST_user_info();
            Token(token, out user);
        }
    }
}