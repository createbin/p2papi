using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Promotion
{
    /// <summary>
    /// 新闻类别
    /// </summary>
    public interface INewsCategoryService : IRepository<NewsCategory>
    {
        /// <summary>
        /// 根据关键字获取新闻类别
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        List<NewsCategory> GetNewsCategoryByCode(string[] codes);
    }

    public class NewsCategoryService : Repository<NewsCategory>, INewsCategoryService
    {
        public List<NewsCategory> GetNewsCategoryByCode(string[] codes)
        {
            if (codes == null || codes.Length == 0)
                return null;
            string sqlStr = "SELECT * FROM NewsCategory WHERE ";
            //foreach(var code in codes)
            //{
            //    sqlStr += " Code='"+code+"' or";
            //}
            for(int i = 0; i < codes.Length; i++)
            {
                if (i < codes.Length - 1)
                    sqlStr += " Code='" + codes[i] + " ' or";
                else
                    sqlStr += " Code='" + codes[i] + " '";
            }
            return _Conn.Query<NewsCategory>(sqlStr).ToList();
        }
    }
}
