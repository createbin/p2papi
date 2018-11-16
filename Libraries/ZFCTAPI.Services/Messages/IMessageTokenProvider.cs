using System.Collections.Generic;
using ZFCTAPI.Data.Message;
using ZFCTAPI.Data.SYS;

namespace ZFCTAPI.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        /// <summary>
        /// 添加网站相关
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="store"></param>
        /// <param name="emailAccount"></param>
        void AddStoreTokens(IList<Token> tokens, Store store, EmailAccount emailAccount);

        /// <summary>
        /// 验证码相关
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="validateCode"></param>
        void AddValidateTokens(IList<Token> tokens, string validateCode);
    }
}