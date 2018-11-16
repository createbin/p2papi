using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.DbContext;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Data.BoHai.TransferData;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Views
{
    public interface IAccountBatchImportService
    {
        List<AccountImportModel> GetImportModels(string start,int end);
       LoanCountModel GetLoanInfo(int loanid);
        List<LoanInvestDetailsModel> GetLoanInvestDetails(int loanid);

    }
    public  class AccountBatchImportService:IAccountBatchImportService
    {
        protected IDbContext _Context;
        protected IDbConnection _Conn;
        public AccountBatchImportService()
        {
            _Context = EngineContext.Current.Resolve<IDbContext>();
            _Conn = _Context.Conn;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<AccountImportModel> GetImportModels(string start, int end)
        {
            var selectSql = $"select * from v_personal_account where platform_uid in ({start})";
            var result = _Conn.Query<AccountImportModel>(selectSql).ToList();
            return result;
        }

        /// <summary>
        /// 获取标的信息
        /// </summary>
        /// <param name="loanid">标的编号</param>
        /// <returns></returns>
        public LoanCountModel GetLoanInfo(int loanid)
        {
            var merid = BoHaiApiEngineToConfiguration.InstId();
            var selectSql = $"SELECT '{merid}' partner_id,id BorrowId,CASE WHEN pro_add_emp_type=9 THEN '1' ELSE '2' END BorrowTyp," +
                $"pro_loan_money BorrowerAmt,pro_loan_rate BorrowerInterestAmt,";
            selectSql += $"(SELECT cst_plaCustId FROM dbo.CST_account_info WHERE act_user_id=pro_add_emp) BorrCustId," +
                $"(SELECT AccountNo FROM dbo.ChargeAccount WHERE CompanyId=(SELECT id FROM cst_company_info WHERE UserId=pro_add_emp)) AS AccountName," +
                $"(SELECT cst_plaCustId FROM CST_account_info where act_user_id=(SELECT userid FROM dbo.cst_company_info WHERE id=pro_loan_guar_company)) AS GuaranteeNo," +
                $"pro_invest_startTime BorrowerStartDate,pro_invest_endDate BorrowerEndDate,pro_end_date BorrowerRepayDate,";
            selectSql += $"'0' ReleaseType,'2' InvestDateType,pro_loan_period AS InvestPeriod,";
            selectSql += $"(SELECT COUNT(Id) FROM dbo.PRO_loan_plan WHERE pro_loan_id = id) TotalNum,'' MerPriv1,'' MerPriv2 ";
              selectSql += $"FROM dbo.PRO_loan_info WHERE Id='{loanid}' ORDER BY PRO_loan_info.Id desc ";
            var result = _Conn.QueryFirstOrDefault<LoanCountModel>(selectSql);
            return result;
        }

        /// <summary>
        /// 获取标的投资明细
        /// </summary>
        /// <param name="loanid">标的编号</param>
        /// <returns></returns>
        public List<LoanInvestDetailsModel> GetLoanInvestDetails(int loanid)
        {
            var selectSql = $"SELECT Row_Number() over ( order by getdate() ) AS ID,id AS InvestMerNo,(SELECT cst_plaCustId FROM dbo.CST_account_info WHERE act_user_id=pro_invest_emp) AS PlaCustId,";
            selectSql += $"pro_invest_money AS TransAmt,'' MerPriv FROM PRO_invest_info WHERE pro_loan_id={loanid} AND pro_transfer_state=1 ORDER BY pro_invest_money";

            var result = _Conn.Query<LoanInvestDetailsModel>(selectSql).ToList();
            return result;
        }
    }
}
