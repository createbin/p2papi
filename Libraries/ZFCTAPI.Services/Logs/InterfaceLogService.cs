using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using ZFCTAPI.Data.Logs;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Logs
{
    public interface IInterfaceLogService : IRepository<InterfaceRequestLog>
    {
        InterfaceRequestLog GetByUnique(string unique);
    }

    public class InterfaceLogService : Repository<InterfaceRequestLog>, IInterfaceLogService
    {
        public InterfaceRequestLog GetByUnique(string unique)
        {
            var selectString = $"select * from InterfaceRequestLog where UniquelyIdentifies='{unique}'";
            return _Conn.QueryFirstOrDefault<InterfaceRequestLog>(selectString);
        }
    }
}
