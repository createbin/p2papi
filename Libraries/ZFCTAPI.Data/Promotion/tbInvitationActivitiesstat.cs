using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    [Table("tbInvitationActivitiesstat")]
    public class tbInvitationActivitiesstat : BaseEntity
    {
        public int InvestorId { get; set; }

        public decimal InvestmentAmount { get; set; }

        public int loanId { get; set; }

        public int BeneficiaryId { get; set; }

        public int ActivelinkId { get; set; }

        public decimal BeneficiaryAmount { get; set; }

        public bool IsDel { get; set; }

        public DateTime CreateTime { get; set; }

        public int Year { get; set; }

        public int Months { get; set; }
    }
}
