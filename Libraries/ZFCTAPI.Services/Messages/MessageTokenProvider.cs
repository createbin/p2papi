using System;
using System.Collections.Generic;
using ZFCTAPI.Data.Customers;
using ZFCTAPI.Data.Message;
using ZFCTAPI.Data.SYS;
using ZFCTAPI.Services.Localization;

namespace ZFCTAPI.Services.Messages
{
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        #region Methods

        public virtual void AddStoreTokens(IList<Token> tokens, Store store, EmailAccount emailAccount)
        {
            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");

            tokens.Add(new Token("Store.Name", store.Name));
            tokens.Add(new Token("Store.URL", store.Url, true));
            tokens.Add(new Token("Store.Email", emailAccount.Email));

            //event notification
            //_eventPublisher.EntityTokensAdded(store, tokens);
        }

        /// <summary>
        /// 增加验证码
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="newsComment"></param>
        public virtual void AddValidateTokens(IList<Token> tokens, string validateCode)
        {
            tokens.Add(new Token("Validate.Access", validateCode));
        }

        #endregion Methods
    }
}