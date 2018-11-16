using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.UserInfo
{
    public interface IAccountInfoService:IRepository<CST_account_info>
    {
        /// <summary>
        /// 根据用户id获取账户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="platId"></param>
        /// <param name="formId"></param>
        /// <param name="idCard"></param>
        /// <returns></returns>
        CST_account_info GetAccountInfoByUserId(int userId=0,int id=0,string platId="",string formId="",string idCard="");
        /// <summary>
        /// 获取实名用户身份信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        CST_realname_prove GetRealNameInfo(int userId);
        /// <summary>
        /// 获取用户绑定的银行卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        CST_bankcard_info GetBankInfo(int userId);
        /// <summary>
        /// 判断用户银行卡是否存在
        /// </summary>
        /// <param name="bankNo"></param>
        /// <returns></returns>
        CST_bankcard_info GetBankExist(string bankNo);

        void UpdateRealNameProve(CST_realname_prove prove);

        void AddRealNameProve(CST_realname_prove prove);

        void AddBankCardInfo(CST_bankcard_info bankCard);

        void UpdateBankCardInfo(CST_bankcard_info bandCard);
        /// <summary>
        /// 根据投资记录表表的ID查找账号信息
        /// leijun 2017-12-26
        /// </summary>
        /// <param name="investInfoId"></param>
        /// <returns></returns>
        CST_account_info GetAccountInfoByInvestInfoId(int investInfoId);

        /// <summary>
        ///已经子啊汇付开户但未在结算开户的用户
        /// </summary>
        /// <returns></returns>
        List<CST_account_info> NoJieSuanAccount();

        /// <summary>
        /// 删除用户所有银行卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        void DeleteUserBankCard(int userId);

        CST_account_info GetAccountInfoByCompanyId(int companyId);
        /// <summary>
        /// 添加用户权限
        /// </summary>
        /// <param name="model"></param>
        void AddUserAuth(UserAuthorized model);
        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="model"></param>
        void UpdateUserAuth(UserAuthorized model);
        /// <summary>
        /// 查询用户权限
        /// </summary>
        /// <param name="planCust"></param>
        /// <param name="mebillNo"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        UserAuthorized GetUserAuthorized(string planCust="", string mebillNo="", int accountId = 0);

        /// <summary>
        /// 获得用户所有银行卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<CST_bankcard_info> GetUserBankInfos(int accountId);

        /// <summary>
        /// 根据手机号查询已开户用户
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        CST_account_info GetAccountInfoByPhone(string Phone);

        #region 商户端使用
        /// <summary>
        /// 商户端获取用户信息
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        List<RUserInfos> GetMerchatUserInfos(string users);
        /// <summary>
        /// 批量获取用户账户信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<RUserInfos> GetAccountInfos(List<int> ids);
        /// <summary>
        /// 根据条件查询单个用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
        RUserInfos GetAccountInfo(string phone,string realName);

        /// <summary>
        /// 获取需要迁移的存量用户（视图）信息，包含银行卡信息,
        /// </summary>
        /// <returns></returns>
        AccountImportModel GetAccountImportInfo();


        CST_account_info GetUserAccountInfoByPhone(string phone);

        /// <summary>
        /// 添加用户实名信息
        /// </summary>
        /// <param name="accountinfo"></param>
        /// <returns></returns>
        int AddAccountInfo(CST_account_info accountinfo);
        #endregion

    }

    public class AccountInfoService:Repository<CST_account_info>,IAccountInfoService
    {
        public int AddAccountInfo(CST_account_info accountinfo)
        {
            var id = GetMaxId();
            var sql = $@"INSERT INTO dbo.CST_account_info( id,act_user_id ,act_user_type ,act_legal_name ,act_user_name ,act_user_card ,act_user_phone ,financing_platform_id ,JieSuan ,BoHai ,JieSuanTime ,act_business_property ,invest_platform_id ) " +
                $@"VALUES({GetMaxId() },{accountinfo.act_user_id},{accountinfo.act_user_type},'{accountinfo.act_legal_name}','{accountinfo.act_user_name}','{accountinfo.act_user_card}','{accountinfo.act_user_phone}','{accountinfo.financing_platform_id}',0,0,'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',{accountinfo.act_business_property},'{accountinfo.invest_platform_id}')";
            return _Conn.Execute(sql) > 0 ? id : -1;
        }

        private int GetMaxId()
        {
            var sql = "SELECT MAX(id)+1 FROM dbo.CST_account_info";
            return _Conn.QueryFirstOrDefault<int>(sql);
        }

        public CST_account_info GetAccountInfoByInvestInfoId(int investInfoId)
        {
            var sqlStr = "SELECT A.* FROM PRO_invest_info P JOIN CST_account_info A on p.pro_invest_emp = A.act_user_id where p.Id = "+ investInfoId;
            return _Conn.QueryFirstOrDefault<CST_account_info>(sqlStr);
        }

        public List<CST_account_info> NoJieSuanAccount()
        {
            var sqlStr = "select * from CST_account_info where (JieSuan=0 or JieSuan is null)";
            return _Conn.Query<CST_account_info>(sqlStr).ToList();
        }

        public CST_account_info GetAccountInfoByUserId(int userId=0,int id=0, string platId = "", string formId = "", string idCard = "")
        {
            #region 查询条件
            var sqlstence = "1=1";
            if (id != 0)
            {
                sqlstence += string.Format(" and Id='{0}'", id);
            }
            if (userId != 0)
            {
                sqlstence += string.Format(" and act_user_id='{0}'", userId);
            }
            if (!string.IsNullOrEmpty(platId))
            {
                sqlstence += string.Format(" and cst_plaCustId='{0}'", platId);
            }
            if (!string.IsNullOrEmpty(formId))
            {
                sqlstence += string.Format(" and (invest_platform_id='{0}' or financing_platform_id='{0}')", formId);
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                sqlstence += string.Format(" and (act_user_card='{0}' and jiesuan=1)", idCard);
            }

            #endregion 查询条件
            var builder = new StringBuilder();
            builder.Append("select * from CST_account_info");
            builder.Append(" where " + sqlstence);
            var result = _Conn.QueryFirstOrDefault<CST_account_info>(builder.ToString());
            return result;
        }

        public CST_realname_prove GetRealNameInfo(int userId)
        {
            var selectSql = $"select * from CST_realname_prove where Id={userId}";
            return _Conn.QueryFirstOrDefault<CST_realname_prove>(selectSql.ToString());
        }

        public CST_bankcard_info GetBankInfo(int userId)
        {
            var selectSql = $"select * from CST_bankcard_info where mon_account_id={userId} and IsBoHai= 1";
            return _Conn.QueryFirstOrDefault<CST_bankcard_info>(selectSql.ToString());
        }

        public void UpdateRealNameProve(CST_realname_prove prove)
        {
            _Conn.Update<CST_realname_prove>(prove);
        }

        public void AddRealNameProve(CST_realname_prove prove)
        {

            _Conn.Execute($@"INSERT INTO CST_realname_prove VALUES ({prove.Id},{prove.cst_user_id},'{prove.cst_user_realname}',
'{prove.cst_user_sex}',
'{prove.cst_user_nation}',
'{prove.cst_user_birthdate}',
{prove.cst_card_type},
'{prove.cst_card_num}',
'{prove.cst_user_native}',
'{prove.cst_card_front}',
'{prove.cst_card_behind}',
{prove.cst_realname_status})");
        }

        public void AddBankCardInfo(CST_bankcard_info bankCard)
        {
            _Conn.Insert<CST_bankcard_info>(bankCard);
        }

        public void UpdateBankCardInfo(CST_bankcard_info bandCard)
        {
            _Conn.Update<CST_bankcard_info>(bandCard);
        }

        public void DeleteUserBankCard(int userId)
        {
            _Conn.Execute($"DELETE FROM CST_bankcard_info WHERE mon_account_id={userId} and IsBoHai= 1");
        }

        public CST_bankcard_info GetBankExist(string bankNo)
        {
            var sqlStence = $"select * from CST_bankcard_info where bank_no='{bankNo}' and IsBoHai= 1";
            return _Conn.QueryFirstOrDefault<CST_bankcard_info>(sqlStence);
        }

        public CST_account_info GetAccountInfoByCompanyId(int companyId)
        {
            var sqlStence = "select a.* from CST_account_info a ";
            sqlStence += "left join CST_user_info b ";
            sqlStence += $"on a.act_user_id=b.Id where b.CompanyId={companyId}";
            return _Conn.QueryFirstOrDefault<CST_account_info>(sqlStence);
        }

        public void AddUserAuth(UserAuthorized model)
        {
            _Conn.Insert<UserAuthorized>(model);
        }

        public void UpdateUserAuth(UserAuthorized model)
        {
             _Conn.Update<UserAuthorized>(model);
        }

        public UserAuthorized GetUserAuthorized(string planCust="",string mebillNo="", int accountId = 0)
        {
            #region 查询条件
            var sqlstence = "1=1";
            if (accountId != 0)
            {
                sqlstence += string.Format(" and AccountId='{0}'", accountId);
            }
            if (!string.IsNullOrEmpty(planCust))
            {
                sqlstence += string.Format(" and PlanCustId='{0}'", planCust);
            }
            if (!string.IsNullOrEmpty(mebillNo))
            {
                sqlstence += string.Format(" and MerBillNo='{0}'", mebillNo);
            }
            #endregion 查询条件
            var builder = new StringBuilder();
            builder.Append("select * from UserAuthorized");
            builder.Append(" where " + sqlstence);
            var result = _Conn.QueryFirstOrDefault<UserAuthorized>(builder.ToString());
            return result;
        }

        public IEnumerable<CST_bankcard_info> GetUserBankInfos(int accountId)
        {
            return _Conn.Query<CST_bankcard_info>($"select * from CST_bankcard_info where mon_account_id={accountId} and IsBoHai= 1");
        }

        public CST_account_info GetAccountInfoByPhone(string Phone)
        {
            return _Conn.QueryFirstOrDefault<CST_account_info>($"SELECT * FROM CST_account_info WHERE act_user_phone = '{Phone}' AND JieSuan = 1 ");
        }

        public List<RUserInfos> GetMerchatUserInfos(string users)
        {
            var userArrary = users.Split(',').Distinct().ToArray();
            var sqlBuilder=new StringBuilder();
            sqlBuilder.Append("select Id as Id,");
            sqlBuilder.Append(" cst_plaCustId as PlanCustId,");
            sqlBuilder.Append(" act_legal_name as PlanCustName,");
            sqlBuilder.Append(" act_user_phone as PlanCustPhone");
            sqlBuilder.Append(" from CST_account_info where cst_plaCustId in @custs");
            return _Conn.Query<RUserInfos>(sqlBuilder.ToString(),new {custs = userArrary}).ToList();
        }

        public List<RUserInfos> GetAccountInfos(List<int> ids)
        {
            var userArrary = ids.ToArray();
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select Id as UserId,");
            sqlBuilder.Append(" cst_plaCustId as PlanCustId,");
            sqlBuilder.Append(" act_legal_name as PlanCustName,");
            sqlBuilder.Append(" act_user_phone as PlanCustPhone,");
            sqlBuilder.Append(" invest_platform_id as InvestId");
            sqlBuilder.Append(" from CST_account_info where id in @custs");
            return _Conn.Query<RUserInfos>(sqlBuilder.ToString(), new { custs = userArrary }).ToList();
        }

        public RUserInfos GetAccountInfo(string phone,string realName)
        {
            #region 查询条件
            var sqlstence = "1=1 and act_user_type=9 and jiesuan=1 and bohai=1";
            if (!string.IsNullOrEmpty(phone))
            {
                sqlstence += string.Format(" and act_user_phone='{0}'", phone);
            }
            if (!string.IsNullOrEmpty(realName))
            {
                sqlstence += string.Format(" and act_legal_name='{0}'", realName);
            }
            #endregion 查询条件
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select Id as UserId,");
            sqlBuilder.Append(" cst_plaCustId as PlanCustId,");
            sqlBuilder.Append(" act_legal_name as PlanCustName,");
            sqlBuilder.Append(" act_user_phone as PlanCustPhone,");
            sqlBuilder.Append(" invest_platform_id as InvestId from CST_account_info");
            sqlBuilder.Append(" where " + sqlstence);
            var result = _Conn.QueryFirstOrDefault<RUserInfos>(sqlBuilder.ToString());
            return result;
        }

        /// <summary>
        /// 获取需要迁移的存量用户（视图）信息，包含银行卡信息,
        /// </summary>
        /// <returns></returns>
        public AccountImportModel GetAccountImportInfo()
        {
            var sqlstr = "SELECT top 5 * FROM v_personal_account";
            var result = _Conn.QueryFirstOrDefault<AccountImportModel>(sqlstr);
            return result;
        }

        public CST_account_info GetUserAccountInfoByPhone(string phone)
        {
            var sqlstr = $"SELECT * FROM cst_account_info where act_user_phone='{phone}'";
            var result = _Conn.QueryFirstOrDefault<CST_account_info>(sqlstr);
            return result;
        }
    }
}
