using Dapper;
using System.Collections.Generic;
using ZFCTAPI.Core;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Sys
{
    public interface IProTranferApplyService : IRepository<PRO_transfer_apply>
    {
        /// <summary>
        /// 根据用户id获得转让申请
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<PRO_transfer_apply> GetListByUserId(string userId);

        /// <summary>
        /// 根据投资id获得转让申请
        /// </summary>
        /// <param name="investId"></param>
        /// <returns></returns>
        IEnumerable<PRO_transfer_apply> GetListByInvestId(string investId);

        /// <summary>
        /// 根据标id获得转让申请
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        IEnumerable<PRO_transfer_apply> GetListByLoanId(string loanId);

        /// <summary>
        /// 获得用户未删除的申请
        /// </summary>
        /// <param name="userId"></param>
        /// /// <param name="loanId"></param>
        /// <returns></returns>
        IEnumerable<PRO_transfer_apply> GetListNodel(string userId, string loanId);

        /// <summary>
        /// 获得投资 申请中的债权
        /// </summary>
        /// /// <param name="investId"></param>
        /// <returns></returns>
        PRO_transfer_apply GetInvestApply(int investId);
    }

    public class ProTranferApplyService : Repository<PRO_transfer_apply>, IProTranferApplyService
    {
        public IEnumerable<PRO_transfer_apply> GetListByUserId(string userId)
        {
            return _Conn.Query<PRO_transfer_apply>($@"select * from PRO_transfer_apply
where pro_user_id = {userId}
and pro_transfer_state !={ DataDictionary.transferstatus_NotThrough}
and pro_transfer_state !={DataDictionary.transferstatus_HasFlowStandard}
and and pro_is_del = 0");
        }

        public IEnumerable<PRO_transfer_apply> GetListByInvestId(string investId)
        {
            return _Conn.Query<PRO_transfer_apply>($@"select * from PRO_transfer_apply
where pro_invest_id = {investId}
and pro_transfer_state !={ DataDictionary.transferstatus_NotThrough}
and pro_transfer_state !={DataDictionary.transferstatus_HasFlowStandard}
and pro_is_del = 0");
        }

        public IEnumerable<PRO_transfer_apply> GetListByLoanId(string loanId)
        {
            return _Conn.Query<PRO_transfer_apply>($@"select * from PRO_transfer_apply
where pro_loan_id = {loanId}
and pro_transfer_state !={ DataDictionary.transferstatus_NotThrough}
and pro_transfer_state !={DataDictionary.transferstatus_HasFlowStandard}
and pro_is_del = 0");
        }

        public IEnumerable<PRO_transfer_apply> GetListNodel(string userId, string loanId)
        {
            return _Conn.Query<PRO_transfer_apply>($@"select * from PRO_transfer_apply
where pro_loan_id = {loanId}
and pro_user_id = {userId}
and (pro_transfer_state =={ DataDictionary.transferstatus_NotThrough}
or pro_transfer_state =={DataDictionary.transferstatus_HasFlowStandard})
and pro_is_del = 0");
        }

        public PRO_transfer_apply GetInvestApply(int investId)
        {
            return _Conn.QueryFirstOrDefault<PRO_transfer_apply>($@"select * from PRO_transfer_apply
where pro_invest_id = {investId}
and pro_transfer_state =={DataDictionary.transferstatus_Pendingaudit}
and pro_is_del = 0");
        }
    }
}