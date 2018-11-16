using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class ReturnPageData<T>
    {
        //分页的数据
        public IEnumerable<T> PagingData { get; set; }
        //总数据条数
        public int Total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// 额外数据1
        /// </summary>
        public string Extra1 { get; set; }

        /// <summary>
        /// 额外数据2
        /// </summary>
        public string Extra2 { get; set; }
    }
}
