using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    /// <summary>
	/// 数据表tbAdvertising的数据库实体类
    /// 广告内容表
	/// </summary>
    [Table("tbAdvertising")]
    public class tbAdvertising: BaseEntity
    {
        public string Title { get; set; }

        public int? AdvertisPositionId { get; set; }

        public string ImageUrl { get; set; }

        public string Skiplinks { get; set; }

        public int? Sort { get; set; }

        public string ProjectType { get; set; }

        public int? JumpPosition { get; set; }

        public string JumpInfo { get; set; }

        public int? State { get; set; }

        public DateTime? StartinTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime Modificationtime { get; set; }

        public int? Creater { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Modifier { get; set; }

        public bool IsDel { get; set; }
    }
}
