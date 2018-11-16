using Newtonsoft.Json;
using System;
using System.Linq;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Data.BoHai.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.RabbitMQ;
using ZFCTAPI.Services.RabbitMQ;
using ZFCTAPI.Services.SYS;

namespace ZFCTAPI.Services.BoHai
{
    public interface IBHAccountService
    {
        /// <summary>
        /// 余额查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHAccountQueryAccBalance AccountQueryAccBalance(SBHAccountQueryAccBalance model);

        /// <summary>
        /// 充值
        /// 投资人投标或借款人还款时，均需要保证账户中有足够的余额，因此需要通过充值交易实现充值。
        /// 网贷公司发起充值请求后，跳转到账户存管平台页面，账户存管平台页面输入支付密码/手机动态口令后，完成充值操作，并返回充值结果给网贷公司。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHWebRecharge WebRecharge(SBHWebRecharge model);

        /// <summary>
        /// 跳转 渤海充值页面
        /// </summary>
        /// <param name="model"></param>
        void PostWebRecharge(RBHWebRecharge model);

        /// <summary>
        /// 提现
        /// 投资人或借款需要进行取现操作，触发此交易。网贷公司发起提现请求后，跳转到账户存管平台页面，
        /// 账户存管平台页面输入支付密码/手机动态口令后，完成提现申请操作（提现一般会在T+1工作日划转到绑定的银行卡中，根据业务要求根据业务要求而定）
        /// 即通过“用户提现通知”接口返回结果给网贷公司。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHDrawings Drawings(SBHDrawings model);

        /// <summary>
        /// 用户销户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHCloseAcount CloseAcount(SBBCloseAccount model);

        /// <summary>
        /// 跳转 渤海提现页面
        /// </summary>
        /// <param name="model"></param>
        void PostDrawings(RBHDrawings model);

        /// <summary>
        /// 新增、修改项目
        /// 项目编码不可修改，项目申购以后不能再修改项目信息。
        /// （渤海银行）项目类型、借款人相关信息也不可修改。
        /// </summary>
        RBHBaseModel SubmitCreditProject(SBHSubmitCreditProject model);

        /// <summary>
        /// 申购
        /// 投标
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHClaimsPurchase ClaimsPurchase(SBHClaimsPurchase model);

        /// <summary>
        /// 转让成交(约定价格)。
        /// 转让只允许在项目已放款后、还款之前。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHClaimsTransferDeal ClaimsTransferDeal(SBHClaimsTransferDeal model);

        /// <summary>
        /// 还款充值
        /// 借款人线下入账还款金额后，由平台发起通知入账信息，在项目还款之前调用此接口。
        /// 当借款人为企业用户时需在还款前调用此接口，将线下入账的信息上报。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHBaseModel PayBackRecharge(SBHPayBackRecharge model);

        /// <summary>
        /// 融资账户转账
        /// 支持商户下用户进行由投资账户向融资账户转账，此交易只支持单向由投资账户向融资账户转账。
        /// 必须是同一证件用户的投资账户向融资账户转账。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHFinanceTransfer FinanceTransfer(SBHFinanceTransfer model);

        /// <summary>
        /// 线下充值余额同步
        /// 用户线下充值入账后，请调用此接口。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RBHRechargeSyn RechargeSyn(SBHRechargeSyn model);
    }

    public class BHAccountService : IBHAccountService
    {
        #region Field
        private readonly BoHaiApiConfig _boHaiApiConfig;
        private readonly IRabbitMQEvent _rabbitMQEvent;
        private readonly IWorkContext workContext;
        #endregion Field

        #region Ctor

        public BHAccountService(BoHaiApiConfig boHaiApiConfig,
          IRabbitMQEvent rabbitMQEvent, IWorkContext workContext)
        {
            _boHaiApiConfig = boHaiApiConfig;
            _rabbitMQEvent = rabbitMQEvent;
            this.workContext = workContext;
        }

        #endregion Ctor

        #region Func

        /// <summary>
        /// post至结算中心
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private T PostJszx<T>(SBHBaseModel model, string serviceName) where T : RBHBaseModel
        {
            model.serviceName = serviceName;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var post = JsonConvert.SerializeObject(model, Formatting.Indented, jSetting);
            _rabbitMQEvent.Publish(new SYS_Interface_Info
            {
                ITF_info_type = LogsEnum.InputParameters.ToString(),
                ITF_req_parameters = post,
                ITF_Info_addtime = DateTime.Now,
                ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                cmdid = serviceName
            });
            try
            {
                var returnJson = HttpClientHelper.PostAsync(_boHaiApiConfig.ApiAddress +"/"+ serviceName, post).Result.Content.ReadAsStringAsync().Result;
                _rabbitMQEvent.Publish(new SYS_Interface_Info
                {
                    ITF_info_type = LogsEnum.OutputParameters.ToString(),
                    ITF_ret_parameters = returnJson,
                    ITF_Info_addtime = DateTime.Now,
                    ITF_Info_adduser = workContext.CstUserInfo?.cst_user_phone,
                    cmdid = serviceName
                });
                return JsonConvert.DeserializeObject<T>(returnJson);
            }
            catch (Exception e)
            {
                _rabbitMQEvent.Publish(new SYS_Interface_Info
                {
                    ITF_info_type = LogsEnum.OutputParameters.ToString(),
                    ITF_ret_parameters = e.ToString(),
                    ITF_Info_addtime = DateTime.Now,
                    cmdid = serviceName
                });
                return null;
            }
        }

        #endregion Func

        #region Method

        /// <summary>
        /// 余额查询
        /// </summary>
        /// <param name="model"></param>
        public RBHAccountQueryAccBalance AccountQueryAccBalance(SBHAccountQueryAccBalance model)
        {
            return PostJszx<RBHAccountQueryAccBalance>(model, InterfaceName.p_AccountQueryAccBalance.ToString());
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        public RBHWebRecharge WebRecharge(SBHWebRecharge model)
        {
            return PostJszx<RBHWebRecharge>(model, InterfaceName.p_WebRecharge.ToString());
        }

        /// <summary>
        /// 跳转 渤海充值页面
        /// </summary>
        /// <param name="model"></param>
        public async void PostWebRecharge(RBHWebRecharge model)
        {
            var requestAddress = "http://www.baidu.com";
            var post = new HttpClientPageHelper
            {
                AcceptCharset = "GB2312",
                FormName = "WebRecharge",
                Url = requestAddress,
                Method = "POST"
            };
            var nameValueCollection = CommonHelper.ObjectToNameValueCollection(model.SvcBody, true);
            post.Add(nameValueCollection);
            await post.Post();
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="model"></param>
        public RBHDrawings Drawings(SBHDrawings model)
        {
            return PostJszx<RBHDrawings>(model, InterfaceName.p_Drawings.ToString());
        }
        /// <summary>
        /// 销户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHDrawings CloseAccount(SBHDrawings model)
        {
            return PostJszx<RBHDrawings>(model, InterfaceName.p_Drawings.ToString());
        }

        /// <summary>
        /// 跳转 渤海提现页面
        /// </summary>
        /// <param name="model"></param>
        public async void PostDrawings(RBHDrawings model)
        {
            var requestAddress = "http://www.baidu.com";
            var post = new HttpClientPageHelper
            {
                AcceptCharset = "UTF-8",
                FormName = "Drawings",
                Url = requestAddress,
                Method = "POST"
            };
            var nameValueCollection = CommonHelper.ObjectToNameValueCollection(model.SvcBody, true);
            post.Add(nameValueCollection);
            await post.Post();
        }

        /// <summary>
        /// 新增、修改项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHBaseModel SubmitCreditProject(SBHSubmitCreditProject model)
        {
            return PostJszx<RBHBaseModel>(model, InterfaceName.p_submitCreditProject.ToString());
        }

        /// <summary>
        /// 申购
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHClaimsPurchase ClaimsPurchase(SBHClaimsPurchase model)
        {
            return PostJszx<RBHClaimsPurchase>(model, InterfaceName.p_claimsPurchase.ToString());
        }

        /// <summary>
        /// 转让
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHClaimsTransferDeal ClaimsTransferDeal(SBHClaimsTransferDeal model)
        {
            return PostJszx<RBHClaimsTransferDeal>(model, InterfaceName.p_claimsTransferDeal.ToString());
        }

        /// <summary>
        /// 还款充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHBaseModel PayBackRecharge(SBHPayBackRecharge model)
        {
            return PostJszx<RBHBaseModel>(model, InterfaceName.p_PayBackRecharge.ToString());
        }

        /// <summary>
        /// 通知转账
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHFinanceTransfer FinanceTransfer(SBHFinanceTransfer model)
        {
            return PostJszx<RBHFinanceTransfer>(model, InterfaceName.p_FinanceTransfer.ToString());
        }

        /// <summary>
        /// 线下充值余额同步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHRechargeSyn RechargeSyn(SBHRechargeSyn model)
        {
            return PostJszx<RBHRechargeSyn>(model, InterfaceName.p_RechargeSyn.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RBHCloseAcount CloseAcount(SBBCloseAccount model)
        {
            throw new NotImplementedException();
        }

        #endregion Method
    }
}