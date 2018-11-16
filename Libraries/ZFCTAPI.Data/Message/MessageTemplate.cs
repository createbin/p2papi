using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.Localization;

namespace ZFCTAPI.Data.Message
{
    /// <summary>
    /// Represents a message template
    /// </summary>
    [Table("MessageTemplate")]
    public partial class MessageTemplate : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 显示名称
        /// Gets or sets the name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the BCC Email addresses
        /// </summary>
        public string BccEmailAddresses { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the template is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the used email account identifier
        /// </summary>
        public int EmailAccountId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }
    }
}