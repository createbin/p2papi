using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.Customers
{
    [Table("Customer_CustomerRole_Mapping")]
    public class Customer_CustomerRole_Mapping
    {
        public int Customer_Id { get; set; }

        public int CustomerRole_Id { get; set; }
    }
}
