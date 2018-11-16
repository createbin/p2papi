using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.UserInfo
{
    public interface ICstRealNameCheckService : IRepository<CST_realname_check>
    {
    }

    public class CstRealNameCheckService : Repository<CST_realname_check>, ICstRealNameCheckService
    {

    }
}
