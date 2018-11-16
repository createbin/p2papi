using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Pages
{
    public class PageCriteria
    {
        public string TableName { get; set; }

        public string Fields { get; set; } = "*";

        public string PrimaryKey { get; set; } = "Id";

        public int PageSize { get; set; } = 10;

        public int CurrentPage { get; set; } = 1;

        public string Sort { get; set; } = string.Empty;

        public string Condition { get; set; } = string.Empty;

        public int RecordCount { get; set; }
    }
}
