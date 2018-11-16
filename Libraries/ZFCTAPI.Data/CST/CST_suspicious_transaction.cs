using Dapper.Contrib.Extensions;
using System;
using ZFCTAPI.Data;
/// <summary>
/// 数据表CST_transaction_info的数据库实体类
/// </summary>
[Table("CST_suspicious_transaction")]
public partial class CST_suspicious_transaction : BaseEntity
{

    #region 基本信息
    /// <summary>
    ///  用户主键 
    /// </summary>	
    public int pro_user_id { get; set; }

    /// <summary>
    ///  交易信息主键
    /// </summary>	
    public int pro_transaction_id { get; set; }

    /// <summary>
    /// 危险交易类型
    /// </summary>
    public int suspicious_transaction_type { get; set; }

    /// <summary>
    /// 危险交易描述
    /// </summary>
    public string suspicious_transaction_description { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime pro_create_time { get; set; }


    #endregion

}