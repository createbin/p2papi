using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Core.Infrastructure.Dispose;

namespace ZFCTAPI.Core.DbContext
{
    public class DapperDbContext : DisposableObject, IDbContext
    {
        private readonly string _connections = EngineContext.Current.Resolve<ConnectionsConfig>().ConnectionString;
        private bool _committed = true;
        private readonly object _sync = new object();

        public IDbConnection Conn { private set; get; }

        public DapperDbContext()
        {
            InitConnection();
        }

        public DapperDbContext(string connectionSeting)
        {
            // TODO: Complete member initialization
            this._connections = connectionSeting;
            InitConnection();
        }

        public void InitConnection()
        {
            var connection = new SqlConnection(_connections);
            connection.Open();
            this.Conn = connection;
        }

        public bool Committed
        {
            set => _committed = value;
            get => _committed;
        }

        public IDbTransaction Tran { private set; get; }

        public void BeginTran()
        {
            this.Tran = this.Conn.BeginTransaction();
            this.Committed = false;
        }

        public void Commit()
        {
            if (Committed) return;
            lock (_sync)
            {
                this.Tran.Rollback();
                this._committed = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            if (this.Conn.State != ConnectionState.Open) return;
            Commit();
            this.Conn.Close();
            this.Conn.Dispose();
        }
    }
}
