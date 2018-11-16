namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class SVCodeToMobile : BaseSubmitModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNumber { get; set; }
    }

    public class SVCodeToEmail : BaseSubmitModel
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailCode { get; set; }
    }

    public class SRetRetVersion : BaseSubmitModel
    {
        public string Type { get; set; }
    }
}