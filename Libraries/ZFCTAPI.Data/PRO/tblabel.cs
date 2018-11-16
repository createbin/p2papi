using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.PRO
{
    [Table("tblabel")]
    public class tblabel : BaseEntity
    {
        public string Title { get; set; }

        public string Code { get; set; }

        public string Abstract { get; set; }

        public int Sort { get; set; }

        public bool IsDel { get; set; }

        public int? Creater { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Icon { get; set; }

    }
}
