using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    [Table("tbInvitationActivities")]
    public class tbInvitationActivities:BaseEntity
    {
        public string Title { get; set; }

        public int? Year { get; set; }

        public int? Months { get; set; }

        public decimal? Rewards { get; set; }

        public string Activelink { get; set; }

        public string Sharepic { get; set; }

        public string Sharetitle { get; set; }

        public string Sharedescribe { get; set; }

        public bool IsDel { get; set; }

        public int? Creater { get; set; }

        public DateTime? Createtime { get; set; }
    }
}
