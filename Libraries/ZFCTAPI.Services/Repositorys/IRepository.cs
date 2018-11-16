using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZFCTAPI.Data.Pages;

namespace ZFCTAPI.Services.Repositorys
{
    public interface IRepository<T>
        where T : class
    {
        void Add(T model, IDbTransaction transcation = null);

        void Add(IList<T> modeList);

        void Update(T model, IDbTransaction transcation = null);

        void Update(IList<T> modeList);

        T Find(int id);

        void Remove(int id);

        void Remove(T model);

        T QueryFirst(object sqlParas);

        T QueryFirst(string sql, object sqlParas);

        IList<T> GetAll();

        IList<T> Query(object sqlParas);

        IList<T> Query(string sql, object sqlParas);

        IList<T> Query(object sqlParas, int pageSize, int pageIndex);
        /// <summary>
        /// 考虑到分页过程中会根据需要定义实体类 所以实体类传过来吧
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        PageDataView<TS> GetPageData<TS>(PageCriteria criteria, object param = null);
        /// <summary>
        /// 开启连接池
        /// </summary>
        void Open();
        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();
    }
}
