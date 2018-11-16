using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration.DataBase
{
    public class EmailAccountSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a store default email account identifier
        /// </summary>
        public int DefaultEmailAccountId { get; set; }
    }
}