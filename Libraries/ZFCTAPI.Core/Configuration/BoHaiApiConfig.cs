using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration
{
    public class BoHaiApiConfig
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string MerId { get; set; }
        public string ConsumerId { get; set; }
        public string InstId { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public string ApiAddress { get; set; }
        public string BoHaiAddress { get; set; }
        public string BoHaiMobileAddress { get; set; }

        public string MerchantControlAddress { get; set; }
        public string StatisticApiAddress { get; set; }
        public List<Interfaces> Interfaces { get; set; }
    }
    public class Interfaces
    {
        public string Name { get; set; }

        public string ActionUrl { get; set; }

        public string IsHtml { get; set; }

        public string Desc { get; set; }
    }
}
