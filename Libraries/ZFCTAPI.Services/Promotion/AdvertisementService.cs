using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.ApiModels.ReturnModels;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Promotion
{
    /// <summary>
    /// 广告
    /// </summary>
    public interface IAdvertisementService : IRepository<tbAdvertising>
    {
        /// <summary>
        /// 分页获取广告
        /// </summary>
        /// <param name="advertisePositionId">广告类别ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        PageDataView<AdvertisementDetail> GetAdvertisementsPage(int advertisePositionId, int pageIndex = 1, int pageSize = 5);

        /// <summary>
        /// 按条获取广告
        /// </summary>
        /// <param name="advertisePositionId">广告类别ID</param>
        /// <param name="count">广告条数,count=-1查询全部</param>
        /// <returns></returns>
        IList<AdvertisementDetail> GetAdvertisementsCount(int advertisePositionId, int count);
    }

    public class AdvertisementService : Repository<tbAdvertising>, IAdvertisementService
    {
        public IList<AdvertisementDetail> GetAdvertisementsCount(int advertisePositionId, int count)
        {
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var feilds = new StringBuilder();
            feilds.Append("Skiplinks as SkipUrl");
            //开始时间,结束时，未删除，已发布
            var condition = string.Format("(StartinTime<='{0}' or StartinTime is null) and (EndTime>='{0}' or EndTime is null) and IsDel=0 and State=2", nowDate);
            condition += " and AdvertisPositionId="+ advertisePositionId;
            var sort = "Sort desc,CreateDate desc";

            var sqlStr = "";
            //count=-1查询全部
            if (count > -1)
            {
                sqlStr = string.Format("SELECT TOP {0} {1},* FROM tbAdvertising WHERE {2} ORDER BY {3}", count, feilds.ToString(), condition, sort);
            }
            else
            {
                sqlStr= string.Format("SELECT {0},* FROM tbAdvertising WHERE {1} ORDER BY {2}", feilds.ToString(), condition, sort);
            }
            var advertises = _Conn.Query<AdvertisementDetail>(sqlStr).ToList();
            foreach(var advertise in advertises)
            {
                advertise.ImageUrl = FastDFSHelper.GetImageAbsolutePath(advertise.ImageUrl);
            }
            return advertises;
        }

        public PageDataView<AdvertisementDetail> GetAdvertisementsPage(int advertisePositionId, int pageIndex = 1, int pageSize = 5)
        {
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var table = "tbAdvertising";
            var key = "Id";
            //开始时间,结束时，未删除，已发布
            var condition = string.Format("(StartinTime>='{0}' or StartinTime is null) and (EndTime<='{0}' or EndTime is null) and IsDel=0 and State=2", nowDate);
            condition += " and AdvertisPositionId=" + advertisePositionId;
            var feilds = new StringBuilder();
            feilds.Append("Skiplinks as SkipUrl,*");
            var sort = "Sort desc,CreateDate desc";

            PageCriteria criteria = new PageCriteria()
            {
                TableName = table,
                PrimaryKey = key,
                Condition = condition,
                Fields = feilds.ToString(),
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Sort = sort
            };

            var pageData = GetPageData<AdvertisementDetail>(criteria);
            if (pageData != null)
            {
                foreach(var item in pageData.Items)
                {
                    item.ImageUrl= FastDFSHelper.GetImageAbsolutePath(item.ImageUrl);
                }
            }
            return pageData;
        }
    }
}
