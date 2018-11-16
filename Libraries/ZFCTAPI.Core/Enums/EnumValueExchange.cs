using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Enums
{
    public class EnumValueExchange
    {
        public static string ConvertGrantTypeValue(GrantType grantType)
        {
            string result = string.Empty;
            switch (grantType)
            {
                case GrantType.BindingBankCard:
                    {
                        result = "绑定银行卡";
                        break;
                    }
                case GrantType.Borrowing:
                    {
                        result = "借款";
                        break;
                    }
                case GrantType.FirstBorrowing:
                    {
                        result = "首次借款";
                        break;
                    }
                case GrantType.FirstInvestment:
                    {
                        result = "首次投资";
                        break;
                    }
                case GrantType.FirstRecharge:
                    {
                        result = "首次充值";
                        break;
                    }
                case GrantType.FirstRepayment:
                    {
                        result = "首次还款";
                        break;
                    }
                case GrantType.Investment:
                    {
                        result = "投资";
                        break;
                    }
                case GrantType.Invite:
                    {
                        result = "邀请好友";
                        break;
                    }
                case GrantType.MemberBirthday:
                    {
                        result = "会员生日";
                        break;
                    }
                case GrantType.MemberUpgrade:
                    {
                        result = "会员升级";
                        break;
                    }
                case GrantType.PhoneCertification:
                    {
                        result = "手机认证";
                        break;
                    }
                case GrantType.Prepayment:
                    {
                        result = "提前还款";
                        break;
                    }
                case GrantType.RealNameCertification:
                    {
                        result = "实名认证";
                        break;
                    }
                case GrantType.Recharge:
                    {
                        result = "充值";
                        break;
                    }
                case GrantType.RegisteredUsers:
                    {
                        result = "注册用户";
                        break;
                    }
                case GrantType.Repayment:
                    {
                        result = "还款";
                        break;
                    }
                case GrantType.IntegralExchange:
                    {
                        result = "积分兑换";
                        break;
                    }
                case GrantType.CheckIn:
                    {
                        result = "签到";
                        break;
                    }
                case GrantType.FirstPrize:
                    {
                        result = "一等奖";
                        break;
                    }
                case GrantType.SecondPrize:
                    {
                        result = "二等奖";
                        break;
                    }
                case GrantType.ThirdPrize:
                    {
                        result = "三等奖";
                        break;
                    }
                case GrantType.Answer:
                    {
                        result = "答题";
                        break;
                    }
            }
            return result;
        }

        public static string ConvertGrantTypeValueStr(string grantType)
        {
            string result = string.Empty;
            switch (grantType)
            {
                case "BindingBankCard":
                    {
                        result = "绑定银行卡";
                        break;
                    }
                case "Borrowing":
                    {
                        result = "借款";
                        break;
                    }
                case "FirstBorrowing":
                    {
                        result = "首次借款";
                        break;
                    }
                case "FirstInvestment":
                    {
                        result = "首次投资";
                        break;
                    }
                case "FirstRecharge":
                    {
                        result = "首次充值";
                        break;
                    }
                case "FirstRepayment":
                    {
                        result = "首次还款";
                        break;
                    }
                case "Investment":
                    {
                        result = "投资";
                        break;
                    }
                case "Invite":
                    {
                        result = "邀请好友";
                        break;
                    }
                case "MemberBirthday":
                    {
                        result = "会员生日";
                        break;
                    }
                case "MemberUpgrade":
                    {
                        result = "会员升级";
                        break;
                    }
                case "PhoneCertification":
                    {
                        result = "手机认证";
                        break;
                    }
                case "Prepayment":
                    {
                        result = "提前还款";
                        break;
                    }
                case "RealNameCertification":
                    {
                        result = "实名认证";
                        break;
                    }
                case "Recharge":
                    {
                        result = "充值";
                        break;
                    }
                case "RegisteredUsers":
                    {
                        result = "注册用户";
                        break;
                    }
                case "Repayment":
                    {
                        result = "还款";
                        break;
                    }
                case "IntegralExchange":
                    {
                        result = "积分兑换";
                        break;
                    }
                case "FirstPrize":
                    {
                        result = "一等奖";
                        break;
                    }
                case "SecondPrize":
                    {
                        result = "二等奖";
                        break;
                    }
                case "ThirdPrize":
                    {
                        result = "三等奖";
                        break;
                    }
                case "Answer":
                    {
                        result = "答题";
                        break;
                    }
            }
            return result;
        }

        public static string ConvertRuleNameValue(RuleName ruleName)
        {
            string result = string.Empty;
            switch (ruleName)
            {
                case RuleName.Limit:
                    {
                        result = "额度";
                        break;
                    }
                case RuleName.Ranking:
                    {
                        result = "排名";
                        break;
                    }
            }
            return result;
        }

        public static string ConvertPOPRelationValue(POPRelation popRelation)
        {
            string result = string.Empty;
            switch (popRelation)
            {
                case POPRelation.LessThan:
                    {
                        result = "小于";
                        break;
                    }
                case POPRelation.MoreThan:
                    {
                        result = "大于";
                        break;
                    }
                case POPRelation.Equal:
                    {
                        result = "等于";
                        break;
                    }
            }
            return result;
        }
    }
}