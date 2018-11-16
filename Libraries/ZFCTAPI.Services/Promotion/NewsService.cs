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
    /// 新闻
    /// </summary>
    public interface INewsService : IRepository<News>
    {
        /// <summary>
        /// 分页获取新闻
        /// </summary>
        /// <param name="newsCategoryId">新闻类别ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageDataView<NewsDetail> GetNewsPage(int[] newsCategoryIds, int pageIndex = 1, int pageSize = 5);

        /// <summary>
        /// 按条获取新闻
        /// </summary>
        /// <param name="newsCategoryId">新闻类别ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<NewsDetail> GetNewsCount(int newsCategoryId,int count);
    }

    public class NewsService : Repository<News>, INewsService
    {
        public IList<NewsDetail> GetNewsCount(int newsCategoryId, int count)
        {
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var feilds = new StringBuilder();
            feilds.Append("Id as Id,");
            feilds.Append("Title as Title,");
            feilds.Append("Short as Short,");
            feilds.Append("ThumbPicture as ImageUrl,");
            feilds.Append("ArticleSource as SkipUrl,");
            feilds.Append("CreatedOnUtc as CreateTime,");
            feilds.Append("StartDateUtc as StartTime,");
            //feilds.Append("[Full] as Details,");
            feilds.Append("EndDateUtc as EndTime");
            var condition = string.Format("(StartDateUtc>='{0}' or StartDateUtc is null) and (EndDateUtc<='{0}' or EndDateUtc is null) and Published=1", nowDate);
            condition += " and NewTyepId = " + newsCategoryId;
            var sqlStr = "";
            //count=-1查询全部
            if (count > -1)
            {
                sqlStr= string.Format("SELECT TOP {0} {1} FROM News WHERE {2} order by CreatedOnUtc desc", count, feilds.ToString(), condition);
            }
            else
            {
                sqlStr = string.Format("SELECT {0} FROM News WHERE {1} order by CreatedOnUtc desc",feilds.ToString(), condition);
            }

            var news = _Conn.Query<NewsDetail>(sqlStr).ToList();
            foreach(var item in news)
            {
                item.ImageUrl = FastDFSHelper.GetImageAbsolutePath(item.ImageUrl);
            }
            return news;
        }

        public PageDataView<NewsDetail> GetNewsPage(int[] newsCategoryIds, int pageIndex = 1, int pageSize = 5)
        {
            if (newsCategoryIds == null && newsCategoryIds.Length == 0)
                return null;
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var table = "News";
            var key = "Id";
            var condition = string.Format("(StartDateUtc>='{0}' or StartDateUtc is null) and (EndDateUtc<='{0}' or EndDateUtc is null) and Published=1 and (", nowDate);

            for(int i=0;i< newsCategoryIds.Length; i++)
            {
                if (i < newsCategoryIds.Length - 1)
                    condition += " NewTyepId = " + newsCategoryIds[i] + " or ";
                else
                    condition += " NewTyepId = " + newsCategoryIds[i];
            }
            //foreach (var newsCategoryId in newsCategoryIds)
            //{
            //    condition += " NewTyepId = " + newsCategoryId+" or ";
            //}
            condition += ")";
            var feilds = new StringBuilder();
            feilds.Append("Id as Id,"); 
            feilds.Append("Title as Title,"); 
            feilds.Append("Short as Short,");
            feilds.Append("ThumbPicture as ImageUrl,");
            feilds.Append("ArticleSource as SkipUrl,");
            feilds.Append("CreatedOnUtc as CreateTime,");
            feilds.Append("StartDateUtc as StartTime,");
            //feilds.Append("[Full] as Details,");
            feilds.Append("EndDateUtc as EndTime");
            var sort = "CreatedOnUtc desc";

            PageCriteria criteria = new PageCriteria()
            {
                TableName=table,
                PrimaryKey=key,
                Condition=condition,
                Fields= feilds.ToString(),
                CurrentPage=pageIndex,
                PageSize=pageSize,
                Sort=sort
            };
            var pageData = GetPageData<NewsDetail>(criteria);
            if (pageData != null)
            {
                foreach(var item in pageData.Items)
                {
                    item.ImageUrl = FastDFSHelper.GetImageAbsolutePath(item.ImageUrl);
                }
            }
            return pageData;
        }
    }
}
