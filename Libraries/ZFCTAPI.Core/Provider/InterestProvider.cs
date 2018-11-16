using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFCTAPI.Core.Provider
{
    public class InterestType
    {
        public string InterestName { get; set; }

        public string Code { get; set; }
    }
    public class InterestProvider
    {
        public static string GetInterest(string repayment)
        {
            switch (repayment)
            {
                case "Interests.AverageCapitalPlusInterest":
                    return "1";
                case "Interests.MonthlyInterestDuePrincipal":
                    return "2";
                case "Interests.WithBenefitClear":
                    return "3";
                default:
                    return "1";
            }
        }

        public static List<InterestType> LoadAllInterestProviders()
        {
            var result = new List<InterestType>
            {
                new InterestType{Code = "Interests.AverageCapitalPlusInterest",InterestName = "每月等额返还本息"},
                new InterestType{Code = "Interests.MonthlyInterestDuePrincipal",InterestName = "每月返息到期还本"},
                new InterestType{Code = "Interests.WithBenefitClear",InterestName = "一次性到期返还本息"},
            };
            return result;
        }

        public static string LoadInterestProviderGetByFriendlyName(string systemName)
        {
            var result = LoadAllInterestProviders();
            var result2 = result.FirstOrDefault(p => p.Code == systemName);
            return result2 == null ? "" : result2.InterestName;
        }
    }
}
