using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncCloseAccount : RBHAsyncBase
    {
        /// <summary>
        /// 账户类型  1 投资户  2 融资户
        /// </summary>
        public string transTyp { get; set; }
    }
}
