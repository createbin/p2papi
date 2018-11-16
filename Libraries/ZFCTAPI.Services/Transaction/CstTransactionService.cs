using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Core;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Transaction
{
    public interface ICstTransactionService : IRepository<CST_transaction_info>
    {
        /// <summary>
        /// 根据订单号获得交易记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        IEnumerable<CST_transaction_info> GetListByOrderNo(string orderNo);

        /// <summary>
        /// 根据条件 获取用户充值提现交易记录
        /// </summary>
        /// <param name="accountId">用户开户编号</param>
        /// <param name="type">类型</param>
        /// <param name="min">开始时间</param>
        /// <param name="max">结束时间</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="isContain">是否包含</param>
        /// <returns></returns>
        PageDataView<CST_transaction_info> GetListByCondition(int accountId, List<int> types, DateTime? min = null, DateTime? max = null, int page = 1, int pageSize = 5, bool isContain = true);

        /// <summary>
        /// 根据 用户Id和交易流水号 查询交易成功的数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="orderno">交易流水</param>
        /// <returns></returns>
        CST_transaction_info GetSuccessLitByOrderNo(int userId, string orderno);

        /// <summary>
        /// 根据交易类型 统计交易金额
        /// </summary>
        /// <param name="transferType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        decimal AllTransactionMoney(List<int> transferType,int userId);
        /// <summary>
        /// 是否可疑交易
        /// </summary>
        /// <param name="transactionInfo"></param>
        /// <returns></returns>
        bool ExistSuspiciousTransactionInfo(CST_transaction_info transactionInfo);

        /// <summary>
        /// 获取退款记录
        /// </summary>
        /// <param name="userPhone"></param>
        /// <param name="userName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<RefundRecond> GetRefundRecords(string userPhone,string userName,string startDate,string endDate,int page = 1, int pageSize = 10);

        /// <summary>
        /// 用户本月是否提现
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool MonthIsWithdraw(int userId);


    }

    public class CstTransactionService : Repository<CST_transaction_info>, ICstTransactionService
    {
     
        public decimal AllTransactionMoney(List<int> transferTypes, int userId)
        {
            return _Conn.QueryFirstOrDefault<decimal>($"SELECT SUM(pro_transaction_money) FROM CST_transaction_info WHERE pro_user_id = {userId} AND pro_transaction_status = {DataDictionary.transactionstatus_success}  AND pro_transaction_type in ({string.Join(",", transferTypes)})");
        }

        public PageDataView<CST_transaction_info> GetListByCondition(int accountId, List<int> types, DateTime? min, DateTime? max, int page = 1, int pageSize = 5, bool isContain = true)
        {
            var criteria = new PageCriteria();
            criteria.Condition += $" pro_transaction_status = {DataDictionary.transactionstatus_success} ";
            criteria.Condition += $" and pro_user_id = {accountId}";

            if (types != null && types.Count() > 0)
            {
                if (isContain)
                {
                    criteria.Condition += $" and pro_transaction_type in ({string.Join(",", types)}) ";
                }
                else
                {
                    foreach(var item in types){
                        criteria.Condition += $" and pro_transaction_type != {item} ";
                    }
                  
                }
            }
            if (max != null && min != null)
            {
                criteria.Condition += $" and pro_transaction_time >= '{min.Value.ToString("yyyy/MM/dd HH:mm:ss")}' and  pro_transaction_time < '{max.Value.ToString("yyyy/MM/dd HH:mm:ss")}'";
            }
            var innerTable = " CST_transaction_info ";
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.TableName = innerTable;
            criteria.Sort = "pro_transaction_time desc";
            var pageData = GetPageData<CST_transaction_info>(criteria);
            return pageData;
        }

        /// <summary>
        /// 根据订单号获得交易记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public IEnumerable<CST_transaction_info> GetListByOrderNo(string orderNo)
        {
            return _Conn.Query<CST_transaction_info>($"select * from CST_transaction_info where pro_transaction_no = '{orderNo}'");
        }

        public CST_transaction_info GetSuccessLitByOrderNo(int userId, string orderno)
        {
            var sqlStr = $"SELECT * FROM CST_transaction_info WHERE pro_user_id={userId} AND pro_transaction_no={orderno} AND pro_transaction_status={DataDictionary.transactionstatus_success}";
            return _Conn.QueryFirstOrDefault<CST_transaction_info>(sqlStr);
        }

        public bool ExistSuspiciousTransactionInfo(CST_transaction_info transactionInfo)
        {
            //3日内 充值提现数据
            var list = _Conn.Query<CST_transaction_info>($@"SELECT * FROM CST_transaction_info 
WHERE (pro_transaction_type = 459 OR pro_transaction_type = 460)
AND pro_transaction_status = 462 AND pro_user_id = {transactionInfo.pro_user_id}
AND DATEDIFF(DD,pro_complete_time,GETDATE()) BETWEEN 0 AND 2").ToList();

            //3日内 同时存在充值、提现
            if (list != null && list.Count > 0) {
                //总交易金额不小于10万
                if (list.Sum(p => Math.Abs(p.pro_transaction_money.GetValueOrDefault())) < 100000)
                    return false;
                else
                    return true;
            }
            return false;
        }

        public PageDataView<RefundRecond> GetRefundRecords(string userPhone, string userName, string startDate, string endDate, int page = 1, int pageSize = 10)
        {

            var criteria = new PageCriteria();
            criteria.Condition += $" a.pro_transaction_type = {DataDictionary.transactiontype_CostReturn} ";
            if (!string.IsNullOrEmpty(userPhone))
            {
                criteria.Condition += $" and b.act_user_phone='{userPhone}' ";
            }
            if (!string.IsNullOrEmpty(userName))
            {
                criteria.Condition += $" and b.act_legal_name='{userName}' ";
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                criteria.Condition += $" and a.pro_transaction_time>'{startDate}' ";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                criteria.Condition += $" and a.pro_transaction_time<'{endDate}' ";
            }
            var innerTable = " CST_transaction_info a inner join CST_account_info b on a.pro_user_id=b.act_user_id ";
            var selectSql = new StringBuilder();
            selectSql.Append(" a.DepoBankSeq as SerialNumber,");
            selectSql.Append(" a.pro_transaction_no as OrderNumber,");
            selectSql.Append(" a.TrxId as ProjectCode,");
            selectSql.Append(" a.MarketingInformation as ProjectInfo,");
            selectSql.Append(" a.pro_transaction_time as OperateDate,");
            selectSql.Append(" a.pro_transaction_status as OperateState,");
            selectSql.Append(" a.pro_transaction_remarks as FailReason,");
            selectSql.Append(" b.act_legal_name as UserName,");
            selectSql.Append(" b.act_user_phone as UserPhone");
            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.Fields = selectSql.ToString();
            criteria.PrimaryKey = "a.Id";
            criteria.TableName = innerTable;
            criteria.Sort = "a.id desc";
            var pageData = GetPageData<RefundRecond>(criteria);
            return pageData;

        }

        public bool MonthIsWithdraw(int userId)
        {
            return _Conn.QueryFirstOrDefault<int>($@"SELECT COUNT(*) FROM CST_transaction_info 
WHERE pro_user_id = {userId} 
AND DATEPART(MONTH,pro_transaction_time) = DATEPART(MONTH,'{DateTime.Now.ToString("yyyy-MM-dd")}')
AND DATEPART(YEAR,pro_transaction_time) = DATEPART(YEAR,'{DateTime.Now.ToString("yyyy-MM-dd")}')
AND pro_transaction_type = {DataDictionary.transactiontype_Withdrawals}
AND pro_transaction_status = {DataDictionary.transactionstatus_success}")>0;
        }
    }
}