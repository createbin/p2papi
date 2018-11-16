using System.Collections.Generic;
using ZFCTAPI.Data.Message;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Messages
{
    /// <summary>
    /// Message template service
    /// </summary>
    public partial interface IMessageTemplateService : IRepository<MessageTemplate>
    {
        /// <summary>
        /// Gets a message template by name
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Message template</returns>
        MessageTemplate GetMessageTemplateByName(string messageTemplateName, int storeId = 0);
    }
}