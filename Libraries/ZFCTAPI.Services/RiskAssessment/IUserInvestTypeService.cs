using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Data.RiskAssessment;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.RiskAssessment
{
    public interface IUserInvestTypeService : IRepository<CST_user_investtype>
    {
        /// <summary>
        /// 查用户类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CST_user_investtype GetUserTypes(int id);

        /// <summary>
        /// 删除用户的类型记录
        /// </summary>
        /// <param name="id"></param>
        void DeleteInvesttypeByUserId(int id);
    }

    public class UserInvestTypeService : Repository<CST_user_investtype>, IUserInvestTypeService
    {
        public CST_user_investtype GetUserTypes(int id)
        {
            var sqlStr = $"SELECT top 1 * FROM CST_user_investtype where cst_user_id={id} and DATEDIFF(dd,cst_update_time,GETDATE())<=30 and cst_delsign=0";
            return _Conn.Query<CST_user_investtype>(sqlStr).FirstOrDefault();
        }

        public void DeleteInvesttypeByUserId(int id)
        {
            var sqlStr = "update CST_user_investtype set cst_delsign=1 ,cst_update_time=GETDATE() where cst_user_id=" + id;
            _Conn.ExecuteAsync(sqlStr);
        }
    }
}
