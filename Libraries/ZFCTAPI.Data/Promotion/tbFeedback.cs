using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    /// <summary>
	/// 数据表tbFeedback的数据库实体类
    /// 意见反馈
	/// </summary>
    [Table("tbFeedback")]
    public class tbFeedback : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Telephone { get; set; }

        public int? Source { get; set; }

        public DateTime? Feedbacktime { get; set; }

        public int? Handler { get; set; }

        public string Processingresult { get; set; }

        public DateTime? Processingtime { get; set; }

        public int? State { get; set; }

        public bool IsDel { get; set; }
    }
}
