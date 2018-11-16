using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Transaction
{
    public interface IProIntentCheck : IRepository<PRO_intent_check>
    {

    }

    public class ProIntentCheck : Repository<PRO_intent_check>, IProIntentCheck
    {

    }
}
