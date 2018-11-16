using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.DbContext;
using ZFCTAPI.Core.Infrastructure;
using ZFCTAPI.Data;
using ZFCTAPI.Data.Pages;

namespace ZFCTAPI.Services.Repositorys
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected IDbContext _Context;
        protected IDbConnection _Conn;

        public Repository()
        {
            _Context = EngineContext.Current.Resolve<IDbContext>();
            _Conn = _Context.Conn;
        }

        public void Add(T model, IDbTransaction transcation = null)
        {
            _Conn.Insert<T>(model, transcation);
        }

        public void Update(T model,IDbTransaction transcation = null)
        {
            _Conn.Update(model, transcation);
        }

        public T QueryFirst(string sqlParas)
        {
            using (var newConn = CreateConnection())
            {
                return newConn.QueryFirst<T>(sqlParas);
            }
        }

        public void Add(System.Collections.Generic.IList<T> modelList)
        {
            foreach (var model in modelList)
            {
                _Conn.Insert<T>(model);
            }
        }

        public void Update(System.Collections.Generic.IList<T> modelList)
        {
            foreach (var model in modelList)
            {
                _Conn.Update<T>(model);
            }
        }

        public T Find(int id)
        {
            if (_Conn.State == ConnectionState.Open)
            {
                return _Conn.Get<T>(id);
            }
            using (var newConn = CreateConnection())
            {
                return newConn.Get<T>(id);
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(T model)
        {
            _Conn.Delete<T>(model);
        }

        public T QueryFirst(object sqlParas)
        {
            throw new NotImplementedException();
        }

        public T QueryFirst(string sql, object sqlParas)
        {
            throw new System.NotImplementedException();
        }

        public IList<T> GetAll()
        {
            using (var newConn = CreateConnection())
            {
                return newConn.GetAll<T>().ToList();
            }
        }

        public System.Collections.Generic.IList<T> Query(object sqlParas)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IList<T> Query(string sql, object sqlParas)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IList<T> Query(object sqlParas, int pageSize, int pageIndex)
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            _Context = EngineContext.Current.Resolve<IDbContext>();
            _Conn = _Context.Conn;
        }


        public IDbConnection CreateConnection()
        {
            var connection= new SqlConnection(EngineContext.Current.Resolve<ConnectionsConfig>().ConnectionString);
            connection.Open();
            return connection;
        }

        public PageDataView<TS> GetPageData<TS>(PageCriteria criteria, object param = null)
        {
            if (_Conn.State == ConnectionState.Closed)
                Open();
            var p = new DynamicParameters();
            string proName = "ProcGetPageData";
            p.Add("TableName", criteria.TableName);
            p.Add("PrimaryKey", criteria.PrimaryKey);
            p.Add("Fields", criteria.Fields);
            p.Add("Condition", criteria.Condition);
            p.Add("CurrentPage", criteria.CurrentPage);
            p.Add("PageSize", criteria.PageSize);
            p.Add("Sort", criteria.Sort);
            p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var pageData = new PageDataView<TS>
            {
                Items = _Conn.Query<TS>(proName, p, commandType: CommandType.StoredProcedure).ToList(),
                TotalNum = p.Get<int>("RecordCount")
            };
            pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
            pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
            return pageData;
        }

        ~Repository()
        {
            _Conn.Close();
            _Conn.Dispose();
        }
    }
}
