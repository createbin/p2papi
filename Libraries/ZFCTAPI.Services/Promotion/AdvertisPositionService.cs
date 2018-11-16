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
    /// 广告位置
    /// </summary>
    public interface IAdvertisPositionService : IRepository<tbAdvertisPosition>
    {
        /// <summary>
        /// 根据关键字获取广告位置
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        tbAdvertisPosition GetAdvertPositionByCode(string code);
    }

    public class AdvertisPositionService : Repository<tbAdvertisPosition>, IAdvertisPositionService
    {
        public tbAdvertisPosition GetAdvertPositionByCode(string code)
        {
            string sqlStr = "SELECT * FROM tbAdvertisPosition where Code = '"+code+"'";
            return _Conn.Query<tbAdvertisPosition>(sqlStr).FirstOrDefault();
        }
    }
}
