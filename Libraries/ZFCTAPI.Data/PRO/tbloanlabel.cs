using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.PRO
{
    [Table("tbloanlabel")]
    public class tbloanlabel : BaseEntity
    {
        public int loanId { get; set; }

        public int labelId { get; set; }
    }
}
