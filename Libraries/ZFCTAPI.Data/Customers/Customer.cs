using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Dapper.Contrib.Extensions;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Data.Customers
{
    [Table("Customer")]
    public class Customer:BaseEntity
    {
        #region 基本信息
        /// <summary>
        /// Gets or sets the customer Guid
        /// </summary>
        public Guid CustomerGuid { get; set; }
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public int PasswordFormatId { get; set; }
        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        [Computed]
        public PasswordFormat PasswordFormat
        {
            get => (PasswordFormat)PasswordFormatId;
            set => this.PasswordFormatId = (int)value;
        }
        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }


        /// <summary>
        ///设置账户是否有后台系统权限
        /// </summary>
        public bool IsAdminAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// 用户积分
        /// </summary>
        public int cst_user_score { get; set; }

        /// <summary>
        /// 用户可用积分
        /// </summary>
        public int cst_user_available { get; set; }

        /// <summary>
        /// 父级用户Id 
        /// </summary>
        public int? cst_parent_id { get; set; }

        /// <summary>
        /// 用户来源 calabash add 2017-03-03
        /// </summary>
        public int? cst_user_source { get; set; }

        /// <summary>
        /// 用户邀请码
        /// </summary>
        public string cst_invitation_code { get; set; }

        /// <summary>
        /// 页面推广
        /// </summary>
        public string WebPromotion { get; set; }

        #region 企业用户新增
        /// <summary>
        /// 是否为企业用户
        /// </summary>
        public bool? IsCompany { get; set; }
        #endregion

        #region 2015-1-21 理财
        /// <summary>
        /// 省
        /// </summary>
        public string area_provinces { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string area_cities { get; set; }
        /// <summary>
        /// 用于理财经理的真实姓名
        /// </summary>
        public string RealNames { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        ///
        /// </summary>

        public int? licaiType { get; set; }

        #endregion
        #endregion

    }
}
