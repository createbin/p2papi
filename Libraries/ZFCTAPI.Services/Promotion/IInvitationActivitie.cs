using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Promotion
{
    public interface IInvitationActivitie : IRepository<tbInvitationActivitiesstat>
    {
        List<tbInvitationActivitiesstat> GetListsByBeneficiaryId(int beneficiaryId);

        List<GroupByMonth> GetGroupByMonth(int id);

        List<GroupByInvestDate> GetGroupByInvestDate(int userId,int month,int year,int investerId);

        /// <summary>
        /// 查询活动当月投资人数
        /// </summary>
        /// <param name="userIn"></param>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <returns></returns>
        int GetInvestCount(int userId, int year, int month);

        /// <summary>
        /// 查询活动当月开户人数
        /// </summary>
        /// <param name="userIn"></param>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <returns></returns>
        int GetOpenAccountCount(int userId, int year, int month);

        List<CST_user_info> GetCstUserInfos(int userId, int year, int month);

        /// <summary>
        /// 获取邀请活动信息
        /// </summary>
        /// <returns></returns>
        tbInvitationActivities InvitationActivity(int year,int month);
    }

    public class InvitationActivitie : Repository<tbInvitationActivitiesstat>, IInvitationActivitie
    {
        public int GetOpenAccountCount(int userId, int year, int month)
        {
            var tables = "Customer a inner join CST_user_info b on a.Id=b.cst_customer_id";
            var where = $"b.cst_account_id is not null and a.cst_parent_id={userId} and YEAR(a.CreatedOnUtc)={year} and MONTH(a.CreatedOnUtc)={month}";
            var sqlStr = $"select count(*) from {tables} where {where}";

            return _Conn.Query<int>(sqlStr).First();
        }

        public List<GroupByMonth> GetGroupByMonth(int id)
        {
            var sqlStr = "select count(*) as count, year(CreatedOnUtc) as year,MONTH(CreatedOnUtc) as month from Customer where cst_parent_id=" + id + " group by YEAR(CreatedOnUtc),MONTH(CreatedOnUtc)";

            return _Conn.Query<GroupByMonth>(sqlStr).ToList();
        }

        public int GetInvestCount(int userId, int year, int month)
        {
            var feilds = "count(distinct a.pro_invest_emp)";

            var tables = "PRO_invest_info a inner join CST_user_info b on a.pro_invest_emp=b.Id inner join Customer c on b.cst_customer_id=c.Id";

            var where = $"a.is_invest_succ=1  and a.pro_invest_date< DATEADD (DAY,30,b.cst_add_date) and c.cst_parent_id={userId} and MONTH(b.cst_add_date)={month} and YEAR(b.cst_add_date)={year}";

            var sqlStr = $"select {feilds} from {tables} where {where}";

            return _Conn.Query<int>(sqlStr).First();
        }

        public List<tbInvitationActivitiesstat> GetListsByBeneficiaryId(int beneficiaryId)
        {
            var sqlStr = "SELECT * FROM tbInvitationActivitiesstat where IsDel=0 and BeneficiaryId=" + beneficiaryId;

            return _Conn.Query<tbInvitationActivitiesstat>(sqlStr).ToList();
        }

        public List<CST_user_info> GetCstUserInfos(int userId, int year, int month)
        {
            var tables = "Customer a inner join CST_user_info b on a.Id=b.cst_customer_id";
            var where = $"a.cst_parent_id={userId} and MONTH(a.CreatedOnUtc)={month} and YEAR(a.CreatedOnUtc)= {year}";
            var sqlStr = $"select b.* from {tables} where {where}";
            return _Conn.Query<CST_user_info>(sqlStr).ToList();
        }

        public tbInvitationActivities InvitationActivity(int year, int month)
        {
            var sqlStr = $"select * from tbInvitationActivities where Year={year} and Months={month} and IsDel=0";
            return _Conn.QueryFirstOrDefault<tbInvitationActivities>(sqlStr);
        }

        public List<GroupByInvestDate> GetGroupByInvestDate(int userId, int month, int year, int investerId)
        {
            var feilds = "a.pro_invest_date as investDate,SUM(a.pro_invest_money) as money";
            var tables = "PRO_invest_info a inner join CST_user_info b on a.pro_invest_emp=b.Id inner join Customer c on b.cst_customer_id=c.Id";
            var where = $"a.is_invest_succ=1  and a.pro_invest_date< DATEADD (MONTH,1,b.cst_add_date) and c.cst_parent_id={userId} and MONTH(b.cst_add_date)={month} and YEAR(b.cst_add_date)={year} and pro_invest_emp={investerId}";

            var sqlStr = $"select {feilds} from {tables} where {where} group by a.pro_invest_date";

            return _Conn.Query<GroupByInvestDate>(sqlStr).ToList();
        }
    }
}
