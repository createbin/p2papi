using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.BoHai.SubmitModels;
using ZFCTAPI.Data.Repayment;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Repayment
{
    public interface IPayRecordService : IRepository<PRO_pay_record>
    {
        /// <summary>
        /// 根据投资计划ID查找投资人收益以及账户
        /// </summary>
        /// <param name="planId">还款计划表ID</param>
        /// <param name="isClear">是否结清</param>
        /// <returns></returns>
        List<ReturnRepaymentModel> GetInvesterByPlanId(int planId,bool isClear=false);

        /// <summary>
        /// 根据标的ID获取投资人信息
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        List<InvestPerson> GetInvestPersons(int loanId);

        /// <summary>
        /// 获取当个投资用户投资信息
        /// </summary>
        /// <param name="investerId"></param>
        /// <returns></returns>
        InvestPerson GetInvesteInfo(int investerId);
    }

    public class PayRecordService : Repository<PRO_pay_record>, IPayRecordService
    {
        public InvestPerson GetInvesteInfo(int investerId)
        {
            var feilds = @"ROW_NUMBER() OVER(ORDER BY I.ID) AS ID,
I.pro_fro_orderno as pro_fro_orderno,
A.act_account_no as Act_account_no,
A.cst_plaCustId as cst_plaCustId,
A.act_business_property as act_business_property,
A.invest_platform_id as invest_platform_id,
A.financing_platform_id as financing_platform_id,
A.personal_charge_account as personal_charge_account,
A.act_legal_name as cst_plaCustName,
I.pro_order_no AS pro_order_no,
I.pro_loan_id AS LoanId,
I.pro_invest_money as Pro_invest_money";

            var tables = @"PRO_invest_info I
left join CST_account_info A
on I.pro_invest_emp = A.act_user_id";

            var where = "I.is_invest_succ=1 AND I.pro_delsign=0 AND I.Id=" + investerId;

            var sqlStr = string.Format("SELECT {0} FROM {1} WHERE {2}", feilds, tables, where);

            return _Conn.Query<InvestPerson>(sqlStr)?.FirstOrDefault();
        }

        public List<ReturnRepaymentModel> GetInvesterByPlanId(int planId, bool isClear = false)
        {

            var feilds = @"ROW_NUMBER() over(order by P.id) AS ID,
A.act_account_no as Act_account_no,
A.JieSuan as JieSuan,
A.BoHai AS BoHai,
A.cst_plaCustId AS cst_plaCustId,
A.act_business_property AS act_business_property,
A.invest_platform_id AS invest_platform_id,
A.financing_platform_id AS financing_platform_id,
A.act_legal_name as cst_plaCustName,
p.pro_pay_over_rate as Pro_pay_over_rate,
p.pro_pay_rate as Pro_pay_rate,
p.pro_pay_money as Pro_pay_money,
p.Pro_pay_total as Pro_pay_total,
P.Pro_collect_money AS Pro_collect_money,
P.Pro_collect_rate AS Pro_collect_rate,
P.Id as InvestPlanId,
I.Pro_invest_money AS Pro_invest_money,
I.pro_experience_money as pro_experience_money";

            var tables = @"PRO_invester_plan P
JOIN PRO_invest_info I
ON P.pro_invest_id = I.Id
JOIN CST_account_info A
on I.pro_invest_emp = a.act_user_id";

            //该还款计划未结清的标
            var where = " A.JieSuan=1 and A.BoHai=1 and P.pro_plan_id=" + planId;
            where += isClear ? " and p.pro_is_clear=1" : " and p.pro_is_clear=0";

            //var where = " A.JieSuan=1 and A.BoHai=1 and p.pro_is_clear=0 and P.pro_plan_id=" + planId;

            var sqlStr = string.Format("select {0} from {1} where {2}",feilds,tables,where);

            return _Conn.Query<ReturnRepaymentModel>(sqlStr)?.ToList();
        }

        public List<InvestPerson> GetInvestPersons(int loanId)
        {
            var feilds = @"ROW_NUMBER() OVER(ORDER BY I.ID) AS ID,
I.pro_fro_orderno as pro_fro_orderno,
A.act_account_no as Act_account_no,
A.cst_plaCustId as cst_plaCustId,
A.act_business_property as act_business_property,
A.invest_platform_id as invest_platform_id,
A.financing_platform_id as financing_platform_id,
A.personal_charge_account as personal_charge_account,
A.act_legal_name as cst_plaCustName,
I.pro_order_no AS pro_order_no,
I.pro_loan_id as LoanId,
I.pro_invest_money as Pro_invest_money";

            var tables = @"PRO_invest_info I
left join CST_account_info A
on I.pro_invest_emp = A.act_user_id";

            var where = "I.is_invest_succ=1 AND I.pro_delsign=0 AND I.pro_loan_id="+loanId;

            var sqlStr = string.Format("SELECT {0} FROM {1} WHERE {2}",feilds,tables,where);

            return _Conn.Query<InvestPerson>(sqlStr)?.ToList();
        }
    }
}
