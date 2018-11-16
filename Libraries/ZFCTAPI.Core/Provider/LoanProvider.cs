using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Core.Provider
{
    public class LoanProvider
    {

        /// <summary>
        /// 转换还款方式
        /// </summary>
        /// <param name="repayment"></param>
        /// <returns></returns>
        public static string GetInterest(string repayment)
        {
            switch (repayment)
            {
                //每月等额返还本息
                case "Interests.AverageCapitalPlusInterest":
                    return "5";
                //每月返息到期还本
                case "Interests.MonthlyInterestDuePrincipal":
                    return "2";
                //一次性到期返还本息
                case "Interests.WithBenefitClear":
                    return "1";
                default:
                    return "2";
            }
        }

        /// <summary>
        /// 转换标类型
        /// </summary>
        /// <param name="typeId">产品类型编号</param>
        /// <param name="govId">担保人编号</param>
        /// <returns></returns>
        public static string GetLoanType(int typeId, int govId)
        {
            string loanType = ((int)BHLoanType.Credit).ToString();
            if (typeId == 2 || typeId == 3)
                loanType = ((int)BHLoanType.Pledge).ToString();
            if (govId != 0)
            {
                if (loanType == ((int)BHLoanType.Pledge).ToString())
                    loanType = ((int)BHLoanType.Mixture).ToString();
                else
                    loanType = ((int)BHLoanType.Guarantee).ToString();
            }
            return loanType;
        }

        /// <summary>
        /// 转换借款期限
        /// </summary>
        /// <param name="period">期限</param>
        /// <param name="periodType">期限类型</param>
        /// <returns></returns>
        public static string GetLoanPeriod(int period, int periodType)
        {
            if (periodType == DataDictionary.deadlinetype_Month)
            {
                return $"{period}个月";

            }
            else
            {
                return $"{period}天";
            }
        }

        /// <summary>
        /// 转换项目状态描述
        /// </summary>
        /// <param name="stateId">状态编号</param>
        /// <param name="defaultString">默认描述</param>
        /// <returns></returns>
        public static string GetLoanState(int? stateId, string defaultString="未知")
        {
            if (stateId == DataDictionary.projectstate_StaySend)
            {
                return "待发送";
            }
            else if (stateId == DataDictionary.projectstate_StayPlatformaudit)
            {
                return "待审核";
            }
            else if (stateId == DataDictionary.projectstate_Tender)
            {
                return "招标中";
            }
            else if (stateId == DataDictionary.projectstate_StayRelease)
            {
                return "待发布";
            }
            else if (stateId == DataDictionary.auditlink_Fullstandardaudit)
            {
                return "满标审核";
            }
            else if (stateId == DataDictionary.projectstate_FullScalePending)
            {
                return "满标待审";
            }
            else if (stateId == DataDictionary.projectstate_StayTransfer)
            {
                return "待划转";
            }
            else if (stateId == DataDictionary.projectstate_Overdue)
            {
                return "已逾期";
            }
            else if (stateId == DataDictionary.projectstate_Repayment)
            {
                return "还款中";
            }
            else if (stateId == DataDictionary.projectstate_Settled)
            {
                return "已结清";
            }
            else
            {
                return defaultString;
            }
        }

        /// <summary>
        /// 获得还款期数
        /// </summary>
        /// <returns></returns>
        public static int GetLoanRepayPeriod(string repayType,int periodType, int period)
        {
            if (repayType.Equals("Interests.WithBenefitClear") || periodType == DataDictionary.deadlinetype_Day)
            {
                return 1;
            }
            else {
                return period;
            }
        }


    }
}
