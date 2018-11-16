using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using ZFCTAPI.Core;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.LoanInfo
{
    public interface ITransApplyService:IRepository<PRO_transfer_apply>
    {

    }

    public class TransApplyService : Repository<PRO_transfer_apply>, ITransApplyService
    {
       
    }
}
