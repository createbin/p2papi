using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Promotion
{
    public class GroupByMonth
    {
        public int Count { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }
    }

    public class GroupByInvestDate
    {
        public DateTime InvestDate { get; set; }

        public decimal Money { get; set; }
    }
}
