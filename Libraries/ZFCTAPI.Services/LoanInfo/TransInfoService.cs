using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ZFCTAPI.Core;
using ZFCTAPI.Core.Enums;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Data.TransferInfo;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.LoanInfo
{
    public interface ITransInfoService : IRepository<PRO_transfer_apply>
    {
        /// <summary>
        /// 获取债转列表页
        /// </summary>
        /// <param name="dataType">0：获取所有审核通过的债转 1：获取可投的债转</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<PRO_transfer_apply> TransInfoPage(int dataType = 1, int page = 1, int pageSize = 5);

        /// <summary>
        /// 用户可转让债权数量
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        int CanTransCount(int id);

        /// <summary>
        /// 债权转让统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TransferStatisticsModel TransferStatistics(int id);

        /// <summary>
        /// 已转入债权数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int TransInCount(int id);

        /// <summary>
        /// 获取债权列表
        /// </summary>
        /// <param name="loanId"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        List<PRO_transfer_apply> GetTransferList(int? loanId,bool? isUse);

        /// <summary>
        /// 可转让债权列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<PRO_invest_info> GetCanTransfer(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 转出中债权
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<PRO_transfer_apply> GetTransfering(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 已转出
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<PRO_transfer_apply> GetTransferOut(int id, int page = 1, int pageSize = 5);

        /// <summary>
        /// 已转入
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<PRO_invest_info> GetTransferIn(int id, int page = 1, int pageSize = 5);
    }

    public class TransInfoService : Repository<PRO_transfer_apply>, ITransInfoService
    {

        public int CanTransCount(int id)
        {
            //统计判断条件
            string statisticsSql = $@"select a.Id,
max(case when d.pro_is_clear=1 then d.pro_collect_date end) as maxLoanPayData,
min(case when d.pro_is_clear=0 then d.pro_pay_date end) as minLoanRePayData,
max(case when d.pro_is_clear=0 then d.pro_pay_date end) as maxLoanRePayData,
count(distinct case when c.pro_transfer_state in({DataDictionary.transferstatus_FullScalePending},{DataDictionary.transferstatus_HasThrough},{DataDictionary.transferstatus_StayTransfer},{DataDictionary.transferstatus_HasTransfer}) then  c.Id end) as applyCount
from
PRO_invest_info a
left join PRO_loan_info b on a.pro_loan_id = b.Id
left join PRO_transfer_apply c  on c.pro_invest_id = a.Id
left join PRO_loan_plan d on d.pro_loan_id = b.Id
where
a.pro_transfer_id is null
and a.is_invest_succ =1
and a.pro_delsign = 0
and a.pro_invest_emp = {id}
and b.pro_loan_state = {DataDictionary.projectstate_Repayment}
and b.pro_prod_typeId != 4
and DATEDIFF(dd,a.pro_invest_date,GETDATE())>=30  group by a.Id";

            var sqlString = $@"select count(*)
from
({statisticsSql}) as a
where
a.applyCount = 0
and DATEDIFF(dd,a.maxLoanPayData,GETDATE())>=3
and DATEDIFF(dd,GETDATE(),a.minLoanRePayData)>=3
and DATEDIFF(dd,GETDATE(),a.maxLoanRePayData)>=30";

            return _Conn.QueryFirstOrDefault<int>(sqlString);
        }

        public PageDataView<PRO_invest_info> GetCanTransfer(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $@" a.applyCount = 0
and DATEDIFF(dd, a.maxLoanPayData, GETDATE())>= 3
and DATEDIFF(dd, GETDATE(), a.minLoanRePayData)>= 3
and DATEDIFF(dd, GETDATE(), a.maxLoanRePayData)>= 30 ";

            string tableSql = $@"select a.Id,
max(case when d.pro_is_clear=1 then d.pro_collect_date end) as maxLoanPayData,
min(case when d.pro_is_clear=0 then d.pro_pay_date end) as minLoanRePayData,
max(case when d.pro_is_clear=0 then d.pro_pay_date end) as maxLoanRePayData,
count(distinct case when c.pro_transfer_state in({DataDictionary.transferstatus_FullScalePending},{DataDictionary.transferstatus_HasThrough},{DataDictionary.transferstatus_StayTransfer},{DataDictionary.transferstatus_HasTransfer}) then  c.Id end) as applyCount
from
PRO_invest_info a
left join PRO_loan_info b on a.pro_loan_id = b.Id
left join PRO_transfer_apply c  on c.pro_invest_id = a.Id
left join PRO_loan_plan d on d.pro_loan_id = b.Id
where
a.pro_transfer_id is null
and a.is_invest_succ =1
and a.pro_delsign = 0
and a.pro_invest_emp = {id}
and b.pro_loan_state = {DataDictionary.projectstate_Repayment}
and b.pro_prod_typeId != 4
and DATEDIFF(dd,a.pro_invest_date,GETDATE())>=30  group by a.Id";

            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = " b.* ";
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = $"({tableSql}) as a left join PRO_invest_info as b on a.Id = b.Id ";
            criteria.Sort = "b.pro_invest_date desc";
            var pageData = GetPageData<PRO_invest_info>(criteria);
            return pageData;
        }

        public PageDataView<PRO_transfer_apply> GetTransfering(int id, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $@" pro_user_id = {id}  
and pro_transfer_state ={ DataDictionary.transferstatus_HasThrough}
and pro_transfer_state ={ DataDictionary.transferstatus_StayTransfer}
and pro_transfer_state ={ DataDictionary.transferstatus_FullScalePending}
and pro_is_del = 0 ";
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = "  * ";
            criteria.PrimaryKey = "Id";
            criteria.TableName = $" PRO_transfer_apply ";
            var pageData = GetPageData<PRO_transfer_apply>(criteria);
            return pageData;
        }

        public PageDataView<PRO_transfer_apply> GetTransferOut(int id, int page = 1, int pageSize = 5)
        {

            var criteria = new PageCriteria();
            criteria.Condition += $@" pro_user_id = {id}  
and pro_transfer_state ={ DataDictionary.transferstatus_HasTransfer}
and pro_is_del = 0 ";

            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = "  * ";
            criteria.PrimaryKey = "Id";
            criteria.TableName = $" PRO_transfer_apply ";
            var pageData = GetPageData<PRO_transfer_apply>(criteria);
            return pageData;
        }

        public PageDataView<PRO_invest_info> GetTransferIn(int id, int page = 1, int pageSize = 5)
        {

            var criteria = new PageCriteria();
            criteria.Condition += $@" pro_invest_type = {DataDictionary.investType_transfer} and is_invest_succ = 1 and
                 pro_delsign = 0 and pro_invest_emp = {id} ";
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = "  * ";
            criteria.PrimaryKey = "Id";
            criteria.TableName = $" PRO_invest_info ";
            var pageData = GetPageData<PRO_invest_info>(criteria);
            return pageData;
        }

        public List<PRO_transfer_apply> GetTransferList(int? loanId, bool? isUse)
        {
            var sqlStr = "SELECT* FROM PRO_transfer_apply WHERE pro_is_del = 0 AND ";
            sqlStr += string.Format("pro_transfer_state IN({0},{1},{2},{3})", 
                DataDictionary.transferstatus_FullScalePending, 
                DataDictionary.transferstatus_HasThrough, 
                DataDictionary.transferstatus_StayTransfer,
                DataDictionary.transferstatus_HasTransfer);
            if (loanId != null)
            {
                sqlStr += " AND pro_loan_id="+ loanId.Value;
            }
            if (isUse != null && isUse.Value)
            {
                sqlStr += " pro_is_use=1";
            }
            else if (isUse != null && !isUse.Value)
            {
                sqlStr += " pro_is_use=0";
            }
            return _Conn.Query<PRO_transfer_apply>(sqlStr).ToList();
        }

        public TransferStatisticsModel TransferStatistics(int id)
        {
            string sqlString = $@"select
count(case when a.pro_transfer_state in({DataDictionary.transferstatus_FullScalePending},
{DataDictionary.transferstatus_HasThrough},
{DataDictionary.transferstatus_StayTransfer}) then  a.Id end) as TransferingCount,
count(case when a.pro_transfer_state = {DataDictionary.transferstatus_HasTransfer} then a.Id end) as TransferedCount
from PRO_transfer_apply a left join PRO_invest_info b on a.pro_invest_id = b.id
where  b.pro_delsign = 0 and a.pro_user_id ={id}";

            return _Conn.QueryFirstOrDefault<TransferStatisticsModel>(sqlString);
        }

        public int TransInCount(int id)
        {
            throw new NotImplementedException();
        }

        public PageDataView<PRO_transfer_apply> TransInfoPage(int dataType = 1, int page = 1, int pageSize = 5)
        {
            var criteria = new PageCriteria { Condition = "1=1" };
            criteria.Condition += " and pro_is_del=0 and b.pro_check_state=109";
            if (dataType == 0)
            {
                criteria.Condition +=
                    $" and pro_transfer_state in({DataDictionary.transferstatus_FullScalePending},{DataDictionary.transferstatus_HasThrough},{DataDictionary.transferstatus_StayTransfer},{DataDictionary.transferstatus_HasTransfer})";
            }
            else
            {
                criteria.Condition += $" and pro_transfer_state={DataDictionary.transferstatus_HasThrough}";
            }
            var innerTable = "PRO_transfer_apply a inner join PRO_transfer_check b on a.Id=b.pro_transfer_id";
            criteria.CurrentPage = page;
            criteria.Fields = "a.*";
            criteria.PrimaryKey = "a.Id";
            criteria.PageSize = pageSize;
            criteria.TableName = innerTable;
            criteria.Sort = "pro_transfer_state,b.pro_check_date desc";
            var pageData = GetPageData<PRO_transfer_apply>(criteria);
            return pageData;
        }

       
    }
}