using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHAsyncBindPass: RBHAsyncBase
    {
        /// <summary>
        /// 结果类型(0-请求结果 1-处理结果)
        /// </summary>
        public string resultFlag { get; set; }
    }
}
