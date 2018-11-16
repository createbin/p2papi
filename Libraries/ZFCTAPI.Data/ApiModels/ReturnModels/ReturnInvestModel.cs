using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RLoanRedPackages
    {
        public RLoanRedPackages()
        {
            RedPackages = new List<RRedPackage>();
        }
        public int LoanId { get; set; }
        public int Count { get; set; }

        /// <summary>
        /// 红包可用状态
        /// </summary>
        public bool State { get; set; }

        public List<RRedPackage> RedPackages { get; set; }
    }

    public class RRedPackage
    {
        public int RedId { get; set; }

        public string RedName { get; set; }
        public decimal RedMoney { get; set; }

        public int RedPackageState { get; set; }

        public string EndDate { get; set; }

        public string Condition { get; set; }

        public decimal? AtLeatMoney { get; set; }
    }
}
