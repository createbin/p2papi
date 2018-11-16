using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.POP;
using ZFCTAPI.Services.LoanInfo;

namespace ZFCTAPI.Services.Popular
{
    public interface  IRedFunctionService
    {
        /// <summary>
        /// 用户系统红包发放
        /// </summary>
        /// <param name="type">使用业务类型</param>
        /// <param name="userId">用户Id</param>
        void GrantRedEnvelope(GrantType type, int userId);
        /// <summary>
        /// 给邀请人发放红包
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        void GrantRedEnvelope(GrantType type, int userId, int id);
        void GrantRedEnvelope(GrantType type, int userId, int loanId, decimal amount = 0m);
    }

    public class RedFunctionService: IRedFunctionService
    {
        private readonly IPopEnvelopeRedService _popEnvelopeRedService;
        private readonly ILoanInfoService _loanInfoService;
        private readonly ICstRedInfoService _cstRedInfoService;
        private readonly IPopEnvelopeRuleService _popEnvelopeRuleService;

        public RedFunctionService(IPopEnvelopeRedService popEnvelopeRedService,
            ILoanInfoService loanInfoService,
            ICstRedInfoService cstRedInfoService,
            IPopEnvelopeRuleService popEnvelopeRuleService)
        {
            _popEnvelopeRedService = popEnvelopeRedService;
            _loanInfoService = loanInfoService;
            _cstRedInfoService = cstRedInfoService;
            _popEnvelopeRuleService = popEnvelopeRuleService;
        }

        /// <summary>
        /// 用户系统红包发放
        /// </summary>
        /// <param name="type">使用业务类型</param>
        /// <param name="userId">用户Id</param>
        public void GrantRedEnvelope(GrantType type, int userId)
        {
            try
            {
                switch (type)
                {
                    case GrantType.BindingBankCard:
                        RedGrant(userId, GrantType.BindingBankCard.ToString());
                        break;
                    case GrantType.MemberBirthday:
                        RedGrant(userId, GrantType.MemberBirthday.ToString());
                        break;
                    case GrantType.MemberUpgrade:
                        RedGrant(userId, GrantType.MemberUpgrade.ToString());
                        break;
                    case GrantType.PhoneCertification:
                        RedGrant(userId, GrantType.PhoneCertification.ToString());
                        break;
                    case GrantType.RealNameCertification:
                        RedGrant(userId, GrantType.RealNameCertification.ToString());
                        break;
                    case GrantType.RegisteredUsers:
                        RedGrant(userId, GrantType.RegisteredUsers.ToString());
                        break;
                    case GrantType.Invite:
                        RedGrant(userId, GrantType.Invite.ToString());
                        break;
                    case GrantType.IntegralExchange:
                        RedGrant(userId, GrantType.IntegralExchange.ToString());
                        break;
                    case GrantType.FirstPrize:
                        RedGrant(userId, GrantType.FirstPrize.ToString());
                        break;
                    case GrantType.SecondPrize:
                        RedGrant(userId, GrantType.SecondPrize.ToString());
                        break;
                    case GrantType.ThirdPrize:
                        RedGrant(userId, GrantType.ThirdPrize.ToString());
                        break;
                }
                //t.Start();
            }
            catch (Exception)
            {

            }
        }

        public void GrantRedEnvelope(GrantType type, int userId, int id)
        {
            RedGrant(userId, GrantType.Invite.ToString(),id);
        }

        /// <summary>
        /// 用户系统红包发放
        /// </summary>
        /// <param name="type">使用业务类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="loanId">项目Id</param>
        /// <param name="amount">当前业务的金额（如：投资金额、还款金额）</param>
        /// <returns></returns>
        public void GrantRedEnvelope(GrantType type, int userId, int loanId, decimal amount = 0m)
        {
            try
            {
                switch (type)
                {
                    case GrantType.Borrowing:
                        RedGrant(userId, loanId, amount, GrantType.Borrowing.ToString());
                        //t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Borrowing.ToString()); });
                        break;
                    case GrantType.FirstBorrowing:
                        RedGrant(userId, loanId, amount, GrantType.FirstBorrowing.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.FirstBorrowing.ToString()); });
                        break;
                    case GrantType.FirstInvestment:
                        RedGrant(userId, loanId, amount, GrantType.FirstInvestment.ToString());
                        //t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.FirstInvestment.ToString()); });
                        break;
                    case GrantType.Repayment:
                        RedGrant(userId, loanId, amount, GrantType.Repayment.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Repayment.ToString()); });
                        break;
                    case GrantType.FirstRepayment:
                        RedGrant(userId, loanId, amount, GrantType.FirstRepayment.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.FirstRepayment.ToString()); });
                        break;
                    case GrantType.Prepayment:
                        RedGrant(userId, loanId, amount, GrantType.Prepayment.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Prepayment.ToString()); });
                        break;
                    case GrantType.Investment:
                        RedGrant(userId, loanId, amount, GrantType.Investment.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Investment.ToString()); });
                        break;
                    case GrantType.FirstRecharge:
                        RedGrant(userId, loanId, amount, GrantType.FirstRecharge.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.FirstRecharge.ToString()); });
                        break;
                    case GrantType.Recharge:
                        RedGrant(userId, loanId, amount, GrantType.Recharge.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Recharge.ToString()); });
                        break;
                    case GrantType.Product:
                        RedGrant(userId, loanId, amount, GrantType.Product.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Recharge.ToString()); });
                        break;
                    case GrantType.Answer:
                        RedGrant(userId, loanId, amount, GrantType.Answer.ToString());
                        // t = new Thread(delegate() { RedGrant(userId, loanId, amount, GrantType.Answer.ToString()); });
                        break;
                }
                //t.Start();
            }
            catch
            {
                throw;
            }
        }

        #region helper
        /// <summary>
        /// 发放红包
        /// </summary>
        public void RedGrant(int userId, string type,int? beInvited=null)
        {
            var redGrant = _popEnvelopeRedService.GetRedType(type);
            foreach (var item in redGrant)
            {
                decimal money = _cstRedInfoService.GetRedInfosAggregateAmount(item.Id); //已领取金额
                //是否在有效期内
                if (IsExpiryDate(item) && IsAdditionalCondition(item, userId, type))
                {
                    CST_red_info userRed = new CST_red_info();
                    userRed.cst_red_id = item.Id;
                    userRed.cst_red_exc = false;
                    userRed.cst_create_date = DateTime.Now.Date;
                    userRed.cst_red_startDate = DateTime.Now.Date;
                    userRed.cst_red_employ = false;
                    userRed.cst_user_id = userId;
                    if (GrantType.Invite.ToString().Equals(type))
                    {
                        //这边应该是给邀请的人发红包时记录邀请的是谁 api处 传值过来保存吧 calabash 2017-12-8 
                        userRed.cst_red_beInvited = beInvited;
                    }
                    //面值
                    if (item.pop_red_faceValue == POPValueType.FixedValue.ToString())
                    {
                        userRed.cst_red_money = item.pop_red_faceValueMax;
                    }
                    else if (item.pop_red_faceValue == POPValueType.RadomValue.ToString())
                    {
                        Random random = new Random();
                        int redface = random.Next(Convert.ToInt32(item.pop_red_faceValueMin),
                            Convert.ToInt32(item.pop_red_faceValueMax));
                        if (money + redface > item.pop_red_distributionBudget)
                        {
                            break;
                        }
                        userRed.cst_red_money = redface;
                    }

                    //有效期
                    if (item.pop_red_expiryDateType == RedEnvelopeExpiryDate.Absolute.ToString())
                    {
                        userRed.cst_red_endDate = item.pop_red_expiryDateAbsolute;
                    }
                    else if (item.pop_red_expiryDateType == RedEnvelopeExpiryDate.Relatively.ToString())
                    {
                        userRed.cst_red_endDate = DateTime.Now.Date.AddDays(item.pop_red_expiryDateRelatively);
                    }
                    if (userRed.cst_red_money != null && IsGrant((decimal)userRed.cst_red_money, item))
                        _cstRedInfoService.Add(userRed);
                }
                else
                {
                    item.pop_red_grant = true;
                    _popEnvelopeRedService.Update(item);
                }
            }
        }

        /// <summary>
        /// 发放红包
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loanId"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        public void RedGrant(int userId, int loanId, decimal amount, string type)
        {
            var redGrant = _popEnvelopeRedService.GetRedType(type);
            if (type.Equals(GrantType.Product.ToString()))
            {
                //获取项目的项目类型
                int prductId = _loanInfoService.GetProReleaseById(loanId).pro_prod_typeId ?? 0;
                var redProductGrants = _popEnvelopeRedService.GetRedTypeProduct();
                foreach (var redProductGrant in redProductGrants)
                {
                    if (IsExpiryDate(redProductGrant) && IsAdditionalCondition(redProductGrant, userId, type, loanId, amount))
                    {
                        var products = _popEnvelopeRedService.GetRedProductsByRedId(redProductGrant.Id);
                        redGrant.AddRange(from item in products where item.PRO_product_info_Id == prductId select redProductGrant);
                    }
                }
            }
            foreach (var item in redGrant)
            { //是否在有效期内
                decimal money = _cstRedInfoService.GetRedInfosAggregateAmount(item.Id); //已领取金额
                if (IsExpiryDate(item) && IsAdditionalCondition(item, userId, type, loanId, amount))
                {
                    CST_red_info userRed = new CST_red_info();

                    userRed.cst_red_id = item.Id;
                    userRed.cst_red_exc = false;
                    userRed.cst_create_date = DateTime.Now.Date;
                    userRed.cst_red_startDate = DateTime.Now.Date;
                    userRed.cst_red_employ = false;
                    userRed.cst_user_id = userId;
                    //面值
                    if (item.pop_red_faceValue == POPValueType.FixedValue.ToString())
                    {
                        userRed.cst_red_money = item.pop_red_faceValueMax;
                    }
                    else if (item.pop_red_faceValue == POPValueType.RadomValue.ToString())
                    {
                        Random random = new Random();
                        int redface = random.Next(Convert.ToInt32(item.pop_red_faceValueMin),
                            Convert.ToInt32(item.pop_red_faceValueMax));
                        if (money + redface > item.pop_red_distributionBudget)
                        {
                            break;
                        }
                        userRed.cst_red_money = redface;
                    }
                    else if (item.pop_red_faceValue == POPValueType.StepValue.ToString())
                    {
                        var rules = _popEnvelopeRuleService.GetRedStepValue(item.Id);
                        foreach (var rule in rules)
                        {
                            if (IsEligibility(amount, (decimal)rule.pop_envelope_term, rule.pop_envelope_rel))
                            {
                                userRed.cst_red_money = rule.pop_envelope_Amount;
                                break;
                            }
                        }
                        if (money + userRed.cst_red_money > item.pop_red_distributionBudget)
                        {
                            break;
                        }

                    }
                    //有效期
                    if (item.pop_red_expiryDateType == RedEnvelopeExpiryDate.Absolute.ToString())
                    {
                        userRed.cst_red_endDate = item.pop_red_expiryDateAbsolute;
                    }
                    else if (item.pop_red_expiryDateType == RedEnvelopeExpiryDate.Relatively.ToString())
                    {
                        userRed.cst_red_endDate = DateTime.Now.Date.AddDays(item.pop_red_expiryDateRelatively);
                    }

                    //判断当前是否设置了发送个数和总额度。
                    if (IsGrant((decimal)userRed.cst_red_money, item))
                        _cstRedInfoService.Add(userRed);
                }
                else
                {
                    item.pop_red_grant = true;
                    _popEnvelopeRedService.Update(item);
                }
            }
        }
        /// <summary>
        /// 是否在有效期内（预计费用和发行数量的判断）
        /// </summary>
        /// <returns></returns>
        private bool IsExpiryDate(POP_envelope_red red)
        {
            var flg = false;
            if (red.pop_red_distributionStartEnd < DateTime.Now.Date || red.pop_red_distributionStartDate > DateTime.Now.Date)
                return false;
            {
                if (red.pop_red_expiryDateType == RedEnvelopeExpiryDate.Unlimited.ToString())
                    flg = true;
                else if (red.pop_red_expiryDateType == RedEnvelopeExpiryDate.Absolute.ToString())
                {
                    if (Convert.ToDateTime(red.pop_red_expiryDateAbsolute).Date >= DateTime.Now.Date)
                    {
                        flg = true;
                    }
                }
                else if (red.pop_red_expiryDateType == RedEnvelopeExpiryDate.Relatively.ToString())
                {
                    flg = true;
                }
                var num = _cstRedInfoService.GetRedInfosByredIdNumber(red.Id);
                //红包数量是否溢出
                if (red.pop_red_distributionNnumber != null && num >= red.pop_red_distributionNnumber)
                {
                    flg = false;
                }
                var money = _cstRedInfoService.GetRedInfosAggregateAmount(red.Id); //已领取金额
                if (red.pop_red_faceValue != POPValueType.FixedValue.ToString() &&
                    red.pop_red_faceValue != POPValueType.RadomValue.ToString()) return flg;
                //加上本次红包金额会不会超出预计费用
                if (red.pop_red_distributionBudget != null && money + red.pop_red_faceValueMin > red.pop_red_distributionBudget)
                {
                    flg = false;
                }
            }
            return flg;
        }
        /// <summary>
        /// 红包发放额外条件
        /// </summary>
        /// <returns></returns>
        private bool IsAdditionalCondition(POP_envelope_red red, int userId = 0, string type = "", int loanId = 0, decimal amount = 0m)
        {
            var flg = false;
            if (type == GrantType.RealNameCertification.ToString())
            {
                flg = _cstRedInfoService.IsRedInfoExistence(userId, red.Id);
                flg = !flg;
            }
            else if (type == GrantType.FirstRecharge.ToString())
            {
                flg = _cstRedInfoService.IsRedInfoExistence(userId, red.Id);
                flg = !flg;
            }
            else if (type == GrantType.FirstInvestment.ToString())
            {
                flg = _cstRedInfoService.IsRedInfoExistence(userId, red.Id);
                flg = !flg;
            }
            else if (type == GrantType.FirstBorrowing.ToString())
            {
                flg = _cstRedInfoService.IsRedInfoExistence(userId, red.Id);
                flg = !flg;
            }
            else if (type == GrantType.FirstRepayment.ToString())
            {
                flg = _cstRedInfoService.IsRedInfoExistence(userId, red.Id);
                flg = !flg;
            }
            else
            {
                flg = true;
            }
            return flg;
        }
        /// <summary>
        /// 比较   列：condition0 >  condition1
        /// 返回true活false
        /// </summary>
        /// <param name="condition0">第一条件：用户输入金额</param>
        /// <param name="condition1">第二条件：系统设置金额</param>
        /// <param name="type">比较类型：大于，小于，等于</param>
        /// <returns></returns>
        private bool IsEligibility(decimal condition0, decimal condition1, string type)
        {
            var flg = false;
            switch (type)
            {
                case "Equal":
                    flg = condition1.Equals(condition0) ? true : false;
                    break;
                case "LessThan":
                    flg = condition0 < condition1 ? true : false;
                    break;
                case "MoreThan":
                    flg = condition0 > condition1 ? true : false;
                    break;
            }
            return flg;
        }
        /// <summary>
        /// 是否符合发行预算条件发放
        /// </summary>
        /// <param name="currentAmount">本次发放金额</param>
        /// <param name="red">红包信息</param>
        /// <returns></returns>
        private bool IsGrant(decimal currentAmount, POP_envelope_red red)
        {
            bool flg = false;
            if (red != null)
            {
                decimal cstRedTotalAmount = _cstRedInfoService.GetRedInfosAggregateAmount(red.Id);
                int cstRedInfosByredIdNumber = _cstRedInfoService.GetRedInfosByredIdNumber(red.Id);
                cstRedTotalAmount = cstRedTotalAmount + currentAmount;
                //发行预算(总额)为0或者为null时，没有限制
                if (cstRedTotalAmount <= red.pop_red_distributionBudget.GetValueOrDefault() || red.pop_red_distributionBudget.GetValueOrDefault() == 0)
                    flg = true;
                //发行预算(总的发行的个数)为0或者为null时，没有限制
                if (cstRedInfosByredIdNumber <= red.pop_red_distributionNnumber.GetValueOrDefault() || red.pop_red_distributionNnumber.GetValueOrDefault() == 0)
                    flg = true;
            }
            return flg;
        }
        #endregion

    }
}
