using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.Logs
{
    [Table("InterfaceRequestLog")]
    public class InterfaceRequestLog:BaseEntity
    {
        public string RequestSource { get; set; }

        public string Token { get; set; }

        public string Timestamps { get; set; }
        public string IpAddress { get; set; }
        public string FacilityType { get; set; }
        public string InterfaceName { get; set; }

        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateTime { get; set; }
        public int Attributes { get; set; } = 2;
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniquelyIdentifies { get; set; }
    }
}
