using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Promotion
{
    /// <summary>
    /// 活动统计
    /// </summary>
    public interface IActivityStatsService : IRepository<ActivityPrizeUserInvest>
    {
        /// <summary>
        /// 根据活动ID获取活动排行榜
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="count">条数</param>
        /// <returns></returns>
        List<RActivityRankModel> GetActivitiesStateByActId(int activityId,int count);

        /// <summary>
        /// 获取最新投资记录
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns></returns>
        List<RInvestRecordsModel> GetInvestRecords(int count);

        /// <summary>
        /// 活动期间用户投资金额
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        RInvestRewardModel GetActivityInvest(int activityId, int userId);
    }

    public class ActivityStatsService : Repository<ActivityPrizeUserInvest>, IActivityStatsService
    {
        public List<RActivityRankModel> GetActivitiesStateByActId(int activityId, int count)
        {
            var feilds = "act_user_name as RealName,act_user_phone as Phone,AnnualAmount as InvestMoney,RANK() over(order by AnnualAmount desc) as Number ";

            var table = "ActivityPrizeUserInvest A JOIN CST_account_info C on A.UserId = c.act_user_id";

            var sqlStr = string.Format("SELECT Top {0} {1} FROM  {2}  where ActivityId = {3}", count, feilds, table, activityId);

            sqlStr += " order by AnnualAmount desc";//投资总额倒叙排

            var persons = _Conn.Query<RActivityRankModel>(sqlStr).ToList();
            //将电话号码和真实姓名隐藏
            foreach(var person in persons)
            {
                person.RealName = string.IsNullOrEmpty(person.RealName) ? null : person.RealName.Substring(0, 1) + "**";
                person.Phone = string.IsNullOrEmpty(person.Phone) ? null : person.Phone.Substring(0, 3) + "********";
            }
            return persons;
        }

        public RInvestRewardModel GetActivityInvest(int activityId, int userId)
        {
            string sqlStr =string.Format("SELECT AnnualAmount as InvestMoney FROM ActivityPrizeUserInvest where UserId={0} and ActivityId={1}",userId,activityId);
            return _Conn.Query<RInvestRewardModel>(sqlStr).FirstOrDefault();
        }

        public List<RInvestRecordsModel> GetInvestRecords(int count)
        {
            var feilds = "act_user_name as RealName,act_user_phone as Phone,p.pro_invest_money as InvestMoney,p.pro_invest_date as InvestDate,L.pro_loan_period as Period";
            var table = " PRO_invest_info P JOIN CST_account_info C on p.pro_invest_emp=c.act_user_id JOIN PRO_loan_info L ON P.PRO_LOAN_ID=L.Id";
            var sqlStr =string.Format("SELECT Top {0} {1} FROM {2}", count,feilds, table);
            sqlStr += " order by InvestDate desc";//投资时间倒叙

            var records = _Conn.Query<RInvestRecordsModel>(sqlStr).ToList();
            //将电话号码和真实姓名隐藏
            foreach (var record in records)
            {
                record.RealName = string.IsNullOrEmpty(record.RealName) ? null : record.RealName.Substring(0, 1) + "**";
                record.Phone = string.IsNullOrEmpty(record.Phone) ? null : record.Phone.Substring(0, 3) + "********";
            }
            return records;
        }
    }
}
