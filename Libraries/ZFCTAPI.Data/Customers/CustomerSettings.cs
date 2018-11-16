using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Data.Customers
{
    public class CustomerSettings
    {
        public static decimal riskreserve => 0;

        public static decimal counterFeeProportion => 0;

        public static bool EmailEnabled => false;

        public static int alb_customer_Id => 0;

        public static int withdrawalsNum => 0;

        public static int investmentBase => 0;

        public static string InvitationCodePrefix => "";

        public static UserRegistrationType UserRegistrationType => UserRegistrationType.Standard;

        public static PasswordFormat DefaultPasswordFormat => PasswordFormat.Hashed;

        /// <summary>
        /// Gets or sets a customer password format (SHA1, MD5) when passwords are hashed
        /// </summary>
        public static string HashedPasswordFormat => "SHA1";

    }
}
