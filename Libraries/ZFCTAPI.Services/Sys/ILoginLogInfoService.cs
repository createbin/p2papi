using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Sys
{
    public interface ILoginLogInfoService : IRepository<LoginLogInfo>
    {
    }

    public class LoginLogInfoService : Repository<LoginLogInfo>, ILoginLogInfoService
    {

    }
}
