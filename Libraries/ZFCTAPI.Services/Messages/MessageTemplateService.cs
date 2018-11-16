using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ZFCTAPI.Core.Caching;
using ZFCTAPI.Data.Message;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Messages
{
    public partial class MessageTemplateService : Repository<MessageTemplate>, IMessageTemplateService
    {
        #region Methods

        /// <summary>
        /// Gets a message template
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Message template</returns>
        public virtual MessageTemplate GetMessageTemplateByName(string messageTemplateName, int storeId = 0)
        {
            if (string.IsNullOrWhiteSpace(messageTemplateName))
                throw new ArgumentException("messageTemplateName");
            return _Conn.QueryFirst<MessageTemplate>($@"select * from MessageTemplate where Name ='{messageTemplateName}' order by Id");
        }

        #endregion Methods
    }
}