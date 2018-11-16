using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    /// <summary>
	/// 数据表tbAdvertisPosition的数据库实体类
    /// 广告位置标
	/// </summary>
    [Table("tbAdvertisPosition")]
    public class tbAdvertisPosition : BaseEntity
    {
        public string Title { get; set; }

        public string Abstract { get; set; }

        public string Code { get; set; }

        public int? Sort { get; set; }

        public bool IsDel { get; set; }

        public int? Creater { get; set; }

        public DateTime CreateTime { get; set; }

        public int Type { get; set; }
    }
}
