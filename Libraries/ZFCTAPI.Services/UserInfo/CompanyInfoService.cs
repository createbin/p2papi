using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Repositorys;
using System.Linq;

namespace ZFCTAPI.Services.UserInfo
{
    public interface ICompanyInfoService:IRepository<Cst_Company_Info>
    {
        ChargeAccount GetChargeAccount(string accountNo="",string accountName="",string billNo="",int companyId = 0);

        /// <summary>
        /// 根据用户编号查询公司信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        Cst_Company_Info GetCompanyInfo(int userId);

        void AddChargeAccount(ChargeAccount model);

        void UpdateChargeAccount(ChargeAccount model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instuCode">组织机构代码</param>
        /// <param name="busiCode">营业执照编号</param>
        /// <param name="taxCode">税务登记号</param>
        /// <param name="corperationIdCard">法人证件号</param>
        /// <returns></returns>
        Cst_Company_Info GetComayInfo(string instuCode="",string busiCode="",string taxCode="",string corperationIdCard="");

        /// <summary>
        /// 手机号码获取
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Cst_Company_Info GetCompanyInfoByPhone(string phone);
    }
     
    public class CompanyInfoService :Repository<Cst_Company_Info>,ICompanyInfoService
    {
        public ChargeAccount GetChargeAccount(string accountNo="", string accountName = "", string billNo = "", int companyId=0)
        {
            #region 查询条件

            var sqlstence = "1=1";
            if (companyId != 0)
            {
                sqlstence += string.Format(" and CompanyId='{0}'", companyId);
            }
            if (!string.IsNullOrEmpty(accountNo))
            {
                sqlstence += string.Format(" and AccountNo='{0}'", accountNo);
            }
            if (!string.IsNullOrEmpty(accountName))
            {
                sqlstence += string.Format("and accountname='{0}'", accountName);
            }
            if (!string.IsNullOrEmpty(billNo))
            {
                sqlstence += string.Format("and MerBillNo='{0}'", billNo);
            }

            #endregion 查询条件

            var sqlStence = "select * from ChargeAccount where " + sqlstence;
            var result = this._Conn.QueryFirstOrDefault<ChargeAccount>(sqlStence);
            return result;
        }

        public void AddChargeAccount(ChargeAccount model)
        {
            this._Conn.Insert<ChargeAccount>(model);
        }


        public void UpdateChargeAccount(ChargeAccount model)
        {
            this._Conn.Update<ChargeAccount>(model);
        }

        public Cst_Company_Info GetCompanyInfo(int userId)
        {
            return _Conn.QueryFirstOrDefault<Cst_Company_Info>($"SELECT * FROM Cst_Company_Info WHERE UserId = {userId}");
        }

        public Cst_Company_Info GetComayInfo(string instuCode = "", string busiCode = "", string taxCode = "", string corperationIdCard = "")
        {
            var model = new Cst_Company_Info();

            var sqlStr = "SELECT * FROM Cst_Company_Info where";
            var sql = "";
            if (!string.IsNullOrEmpty(instuCode))
            {
                sql = sqlStr+ $" InstuCode='{instuCode}'";
                
            }
            if (!string.IsNullOrEmpty(busiCode))
            {
                sql = sqlStr + $" BusiCode='{busiCode}'";
            }
            if (!string.IsNullOrEmpty(taxCode))
            {
                sql = sqlStr + $" TaxCode='{taxCode}'";
            }
            if (!string.IsNullOrEmpty(corperationIdCard))
            {
                sql = sqlStr + $" CorperationIdCard='{corperationIdCard}'";
            }

            model = _Conn.Query<Cst_Company_Info>(sql).FirstOrDefault();
            return model;
        }

        public Cst_Company_Info GetCompanyInfoByPhone(string phone)
        {
            var sqlStr = $"SELECT * FROM Cst_Company_Info WHERE ContactPhone='{phone}'";
            return _Conn.Query<Cst_Company_Info>(sqlStr).FirstOrDefault();
        }
    }
}
