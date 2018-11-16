using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Data.Pages;
using ZFCTAPI.Data.Promotion;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Promotion
{
    /// <summary>
    /// 企业内刊，运营报告
    /// </summary>
    public interface IInternalMagazineService: IRepository<tbInternalMagazine>
    {
        PageDataView<tbInternalMagazine> GetManagize(int type, int pageIndex, int pageSize);

        List<tbInternalMagazine> GetManagizeCount(int type,int count, int year);
    }

    public class InternalMagazineService : Repository<tbInternalMagazine>, IInternalMagazineService
    {
        public PageDataView<tbInternalMagazine> GetManagize(int type, int pageIndex, int pageSize)
        {
            var table = "tbInternalMagazine";
            var key = "Id";
            var feilds = "*";
            var condition = "State=1 and IsDel=0 and ";
            condition += "Category=" + type;
            var sort = "Sort desc,CreateDate desc";
            PageCriteria criteria = new PageCriteria()
            {
                TableName = table,
                PrimaryKey = key,
                Condition = condition,
                Fields = feilds,
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Sort = sort
            };
            var pageData = GetPageData<tbInternalMagazine>(criteria);
            if (pageData != null)
            {
                foreach(var item in pageData.Items)
                {
                    item.ImageUrl = FastDFSHelper.GetImageAbsolutePath(item.ImageUrl);
                }
            }
            return pageData;
        }

        public List<tbInternalMagazine> GetManagizeCount(int type, int count,int year)
        {
            var sqlStr = $"SELECT * FROM tbInternalMagazine WHERE [YEAR]={year} And  Category={type} AND State=1 and IsDel=0 ORDER BY Sort DESC,CreateDate DESC";
            return _Conn.Query<tbInternalMagazine>(sqlStr).ToList();
        }
    }
}
