using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    /// <summary>
	/// 数据表tbInternalMagazine的数据库实体类
    /// 内刊，运营报告表
	/// </summary>
    [Table("tbInternalMagazine")]
    public class tbInternalMagazine : BaseEntity
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Skiplinks { get; set; }

        public int? Sort { get; set; }

        public bool IsDel { get; set; }

        public int State { get; set; }

        public int? Creater { get; set; }

        public DateTime CreateDate { get; set; }

        public int? Category { get; set; }

        public DateTime? PublishTime { get; set; }
    }
}
