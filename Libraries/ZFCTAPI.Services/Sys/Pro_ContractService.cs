using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Sys
{
    public interface IPro_ContractService : IRepository<Pro_Contract>
    {
        /// <summary>
        /// 获得借款合同
        /// </summary>
        /// <param name="loanId">项目编号</param>
        /// <returns></returns>
        Pro_Contract GetLoanContract(int loanId);

        /// <summary>
        /// 获得投资合同
        /// </summary>
        /// <param name="investId">投资编号</param>
        /// <returns></returns>
        Pro_Contract GetInvestContract(int investId);
    }

    public class Pro_ContractService : Repository<Pro_Contract>, IPro_ContractService
    {
        public Pro_Contract GetInvestContract(int investId)
        {
            return _Conn.QueryFirstOrDefault($"SELECT * FROM Pro_Contract WHERE InvestId = {investId}");
        }

        public Pro_Contract GetLoanContract(int loanId)
        {
            return _Conn.QueryFirstOrDefault($"SELECT * FROM Pro_Contract WHERE LoanId = {loanId}");
        }
    }
}