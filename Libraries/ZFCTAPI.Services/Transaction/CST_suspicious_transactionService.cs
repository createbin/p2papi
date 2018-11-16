using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Transaction
{
    public interface ICST_suspicious_transactionService : IRepository<CST_suspicious_transaction>
    {

    }

    public class CST_suspicious_transactionService : Repository<CST_suspicious_transaction>, ICST_suspicious_transactionService
    {

    }
}
