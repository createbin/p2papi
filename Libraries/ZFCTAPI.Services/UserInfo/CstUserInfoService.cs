using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Repositorys;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.MultiTable;

namespace ZFCTAPI.Services.UserInfo
{
    public partial interface ICstUserInfoService : IRepository<CST_user_info>
    {
        /// <summary>
        /// 根据查询条件获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        CST_user_info GetUserInfo(int id = 0, string phone = null, int customerId = 0, string email = null);

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RUseBaseInfoModel GetUserBaseInfo(int id = 0);

        /// <summary>
        /// 校验用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool CheckUserNameExist(string userName);


        /// <summary>
        /// 校验用户手机号是否存在
        /// </summary>
        /// <param name="userPhone"></param>
        /// <returns></returns>
        bool CheckPhoneExist(string userPhone);
        /// <summary>
        /// 注册用户数量
        /// </summary>
        /// <returns></returns>
        int RegisterUserCount();

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompleteUserAccount GetCompleteUserAccount(int? id);

        /// <summary>
        /// 更新UserInfo表中cst_account_id字段
        /// </summary>
        /// <param name="userinfoid"></param>
        /// <param name="accountinfoid"></param>
        /// <returns></returns>
        int UpdateUserInfoByID(int userinfoid, int accountinfoid);
    }

    public partial class CstUserInfoService : Repository<CST_user_info>, ICstUserInfoService
    {
        public CST_user_info GetUserInfo(int id = 0, string phone = null, int customerId = 0, string email = null)
        {
            #region 查询条件

            var sqlstence = "1=1";
            if (id != 0)
            {
                sqlstence += string.Format(" and Id='{0}'", id);
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sqlstence += string.Format(" and cst_user_phone='{0}'", phone);
            }
            if (customerId != 0)
            {
                sqlstence += string.Format(" and cst_customer_id='{0}'", customerId);
            }
            if (!string.IsNullOrEmpty(email))
            {
                sqlstence += string.Format(" and cst_user_email='{0}'", email);
            }

            #endregion 查询条件

            var builder = new StringBuilder();
            builder.Append("select * from CST_user_info");
            builder.Append(" where " + sqlstence);
            var result = _Conn.QueryFirstOrDefault<CST_user_info>(builder.ToString());
            return result;
        }

        public bool CheckUserNameExist(string userName)
        {
            var exist = _Conn.QueryFirstOrDefault("select * from CST_user_info where cst_user_name='" + userName + "'");
            return exist != null;
        }

        public bool CheckPhoneExist(string userPhone)
        {
            var exist = _Conn.QueryFirstOrDefault("select * from CST_user_info where cst_user_phone='" + userPhone + "'");
            return exist != null;
        }

        public int RegisterUserCount()
        {
            var sqlStence = new StringBuilder();
            sqlStence.Append("select COUNT(a.Id) from CST_user_info a");
            sqlStence.Append(" inner join Customer b");
            sqlStence.Append(" on a.cst_customer_id=b.Id");
            sqlStence.Append(" where b.IsAdminAccount=0");
            return _Conn.QueryFirst<int>(sqlStence.ToString());
        }

        public RUseBaseInfoModel GetUserBaseInfo(int id)
        {
            StringBuilder sqlOutParameters = new StringBuilder();
            sqlOutParameters.Append("select b.act_legal_name as RealName,");
            sqlOutParameters.Append("b.act_user_card as UserCard,");
            sqlOutParameters.Append("b.act_user_name as AccountNo,");
            sqlOutParameters.Append("a.cst_user_phone as Phone,");
            sqlOutParameters.Append("a.cst_user_email as Email,");
            sqlOutParameters.Append("a.cst_user_name as UserName,");
            sqlOutParameters.Append("c.bank_no as BankCard, ");
            sqlOutParameters.Append("b.BoHai as BoHai, ");
            sqlOutParameters.Append("b.JieSuan as JieSuan,");
            sqlOutParameters.Append("b.BhMsg as BoHaiMsg,");
            sqlOutParameters.Append("b.JieSuanMsg as JieSuanMsg,");
            sqlOutParameters.Append("b.BhCode as BoHaiCode,");
            sqlOutParameters.Append("b.JieSuanCode as JieSuanCode,");
            sqlOutParameters.Append("b.personal_charge_account as PersonalChargeAccount");
            var sqlString = new StringBuilder();
            sqlString.Append(sqlOutParameters);
            sqlString.Append(" from CST_user_info a");
            sqlString.Append(" left join CST_account_info b on a.Id = b.act_user_id ");
            sqlString.Append(" left join CST_bankcard_info c on b.Id =c.mon_account_id ");
            sqlString.Append($" where a.Id = {id} order by c.IsBoHai desc");

            return _Conn.QueryFirstOrDefault<RUseBaseInfoModel>(sqlString.ToString());
        }

        public CompleteUserAccount GetCompleteUserAccount(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"select * from CST_user_info where id={id}");
            stringBuilder.Append($" select * from Cst_Company_Info where UserId={id}");
            stringBuilder.Append($" select * from CST_account_info where act_user_id={id}");
            stringBuilder.Append($" select * from CST_realname_prove where cst_user_id={id}");
            stringBuilder.Append($" select * from CST_user_company where cst_user_id={id}");
            using (var newConn = CreateConnection())
            {
                var sqlResult = newConn.QueryMultiple(stringBuilder.ToString());
                var result = new CompleteUserAccount();
                if (!sqlResult.IsConsumed)
                {
                    result.LoanUserInfo = sqlResult.ReadFirst<CST_user_info>();
                    result.CstCompanyInfo = sqlResult.ReadFirstOrDefault<Cst_Company_Info>();
                    result.AccountInfo = sqlResult.ReadFirstOrDefault<CST_account_info>();
                    result.RealnameProves = sqlResult.ReadFirstOrDefault<CST_realname_prove>();
                    result.UserCompanies = sqlResult.ReadFirstOrDefault<CST_user_company>();
                }
                return result;
            }
        }

        public int UpdateUserInfoByID(int userinfoid,int accountinfoid)
        {
            var sql = $"UPDATE dbo.CST_user_info SET cst_account_id={accountinfoid} WHERE Id={userinfoid}";
            return _Conn.QueryFirst<int>(sql);
        }
        
    }
}