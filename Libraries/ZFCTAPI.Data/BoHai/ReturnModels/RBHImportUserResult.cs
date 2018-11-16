using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHImportUserResult
    {
        public string char_set { get; set; }

        public string partner_id { get; set; }

        public string BatchNo { get; set; }

        public string TransDate { get; set; }

        public string TotalNum { get; set; }

        public List<ResultUser> ResultUsers { get; set; }=new List<ResultUser>();
    }

    public class ResultUser
    {
        public string TransId { get; set; }

        public string MobileNo { get; set; }

        public string RespCode { get; set; }

        public string PlaCustId { get; set; }

        public string RespDesc { get; set; }

        public string TransTyp { get; set; }
    }
}
