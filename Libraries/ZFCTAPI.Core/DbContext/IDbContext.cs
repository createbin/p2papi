using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZFCTAPI.Core.DbContext
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Conn { get; }

        void InitConnection();
    }
}
