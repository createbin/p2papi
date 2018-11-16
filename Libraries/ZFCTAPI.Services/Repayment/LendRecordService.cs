using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Repayment
{
    public interface ILendRecordService : IRepository<PRO_lend_record>
    {

    }

    public class LendRecordService : Repository<PRO_lend_record>, ILendRecordService
    {
    }
}
