using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    /// <summary>
    /// 新增、修改项目
    /// </summary>
    public class SBHSubmitCreditProject : SBHBaseModel
    {
        public SBHSubmitCreditProjectBody SvcBody { get; set; }
    }

    public class SBHSubmitCreditProjectBody
    {
        /// <summary>
        /// 项目编码
        /// Y
        /// 渤海银行项目编码最大支持10位。
        /// </summary>
        public string projectCode { get; set; }

        /// <summary>
        /// 项目名称
        /// Y
        /// </summary>
        public string projectName { get; set; }

        /// <summary>
        /// 项目类型
        /// Y
        /// 参照【码值】
        /// </summary>
        public string projectType { get; set; }

        /// <summary>
        /// 项目分类
        /// N
        /// 用于授权投标时指定可授权何种分类的项目（由平台自定义）
        /// </summary>
        public string projectCategory { get; set; }

        /// <summary>
        /// 贷款类别
        /// Y
        /// 1信用贷款/固收理财，2担保贷款，3抵押贷款，4混合贷款
        /// 贷款类别为[担保贷款] 时，担保人信息列表必填，审核需校验必要的担保人信息是否完整
        /// </summary>
        public string loanType { get; set; }

        /// <summary>
        /// 贷款金额
        /// Y
        /// 数字，单位：元 (所有接口中金额都保留两位小数）
        /// </summary>
        public string loanAmount { get; set; }

        /// <summary>
        /// 是否可转让
        /// Y
        /// 0-不可转让；1-可转让
        /// </summary>
        public string transferFlag { get; set; }

        /// <summary>
        /// 回购期限
        /// N
        /// 为空时，表示不可回购；不为空时，表示募集成功后多少天后方可回购(以第1笔收取融资日期开始计)
        /// </summary>
        //public string repurchaseTerm { get; set; }

        /// <summary>
        /// 起息方式
        /// Y
        /// 1:标准起息 - 同一项目仅有同一个起息日,[还本付息频率]、[各期还本比例] 必填
        /// 2:按投资起息 - 按申购情况，同一项目多个起息日
        /// </summary>
        public string interestType { get; set; }

        /// <summary>
        /// 起息日
        /// N
        /// 项目起息日，YYYYMMDD；
        /// </summary>
        public string valueDate { get; set; }

        /// <summary>
        /// 还款方式
        /// Y
        /// 1:标准定期本息 - 指固定起息日、固定利率（日息/月息）、固定周期（含一次性、按月/季/半年/指定天数），每期同时还本付息
        ///2:定期付息、统一到期还本,同一还款日
        ///3:定期付息、分散到期还本,不同还款日
        ///4:分散一次性本息清,不同还款日
        ///5:等额本息
        ///6:等额本金
        ///9:其他还款方式
        ///标准定期本息以外，[起息方式] 必填
        /// </summary>
        public string repayType { get; set; }

        /// <summary>
        /// 还款方式说明
        /// C
        /// 非[标准定期本息]的，需做详细描述，包括起息、计息、还款期等
        /// </summary>
        public string repayTypeComment { get; set; }

        /// <summary>
        /// 还款期数
        /// Y
        /// </summary>
        public string numberOfPayments { get; set; }

        /// <summary>
        /// 还本付息频率
        /// C
        /// 1:一次还本付息（项目周期结束日期时还本付息）
        /// 2:按月（以起息日的日为每月到期还款日；遇二月、小月的月末日期小于还款日期的，按当月最后一天计）
        /// 3:按季度（以起息日的日为每3个月到期还款日；遇二月、小月的月末日期小于还款日期的，按当月最后一天计）
        /// 4:按半年（以起息日的日为每6个月到期还款日；遇二月、小月的月末日期小于还款日期的，按当月最后一天计）
        /// 9:按指定天数（起息日起，固定每隔多少天还款1次）
        /// </summary>
        public string repayFrequency { get; set; }

        /// <summary>
        /// 项目计息频度
        /// N
        /// 还本付息频率为1[一次还本付息]时，此项必填，单位为天数，周期=计息频度；
        /// 还本付息频率为2、3、4，非必填，周期 = 还款期数* 还本付息频率代表的计息频度（2时为1个月、3时为3个月、4时为6个月，单位为月）
        /// 还本付息频率为9时，必填，为每隔的天数，单位为天，周期 = 还款期数* 计息频度
        /// </summary>
        public string projectInterestPeriod { get; set; }

        /// <summary>
        /// 各期还本比例
        /// C
        /// 以半角逗号分隔各期还本比例（0-1之间的小数），各期比例之和为1，总期数与还款期数一致
        /// </summary>
        public string principalRepayRate { get; set; }

        /// <summary>
        /// 付息方式
        /// Y
        /// 还本付息时，付息方式：
        /// 1：日息 - 按年化收益率Y、计息周期天数D，年化天数N，换算日息比率 P = Y / N * D
        /// 2：月息 - 按年化收益率Y、计息周期天数D，换算月息比率 P = Y / 12，月息余额=月平均日余额
        /// </summary>
        public string interestPayMode { get; set; }

        /// <summary>
        /// 年化天数
        /// Y
        /// 1：360天
        /// 2：365天
        /// </summary>
        public string annualPeriod { get; set; }

        /// <summary>
        /// 年化收益率
        /// Y
        /// 根据付息方式计算有别，计算时等同于1日年化收益率、或者1月年化收益率
        /// </summary>
        public string APR { get; set; }

        /// <summary>
        /// 平台补贴收益率
        /// N
        /// 在年化收益率上增补
        /// </summary>
        public string bonusAPR { get; set; }

        /// <summary>
        /// 募集开始时间
        /// Y
        /// 格式：yyyy-MM-ddHH:mm:ss如：2016-10-31 09:30:00
        /// </summary>
        public string startTimeOfRaising { get; set; }

        /// <summary>
        /// 募集结束时间
        /// Y
        /// 格式：yyyy-MM-ddHH:mm:ss如：2016-10-31 09:30:00，晚于开始时间、晚于请求当日
        /// </summary>
        public string endTimeOfRaising { get; set; }

        /// <summary>
        /// 允许超募比例
        /// N
        /// 0-1间的小数，如为空，则不可超募
        /// </summary>
        public string allowOverRaisedFlag { get; set; }

        /// <summary>
        /// 允许募集不足比例
        /// N
        /// 0-1间的小数，如为空，则不允许募集不足时达标
        /// </summary>
        public string allowLackFlag { get; set; }

        /// <summary>
        /// 收取融资列表
        /// Y
        /// 收取融资计划固定一期，融资人收费时点固定为放款
        /// </summary>
        public List<SBHFund> fundList { get; set; }

        /// <summary>
        /// 借款人平台编码
        /// Y
        /// </summary>
        public string borrowerUserCode { get; set; }

        /// <summary>
        /// 借款人名称
        /// Y
        /// </summary>
        public string borrowerUserName { get; set; }

        /// <summary>
        /// 借款人性质
        /// Y
        /// 1个人
        /// 2企业
        /// </summary>
        public string borrowerUserProperty { get; set; }

        /// <summary>
        /// 证件类型
        /// Y
        /// 码值.【证件类型】
        /// </summary>
        public string borrowerUserIDType { get; set; }

        /// <summary>
        /// 证件号码
        /// Y
        /// 企业时为三证号，未更换三证号的填写营业执照编号
        /// </summary>
        public string borrowerUserID { get; set; }

        /// <summary>
        /// 法定代表人姓名
        /// C
        /// 借款人性质=2，企业时，必填
        /// </summary>
        public string borrowerLRName { get; set; }

        /// <summary>
        /// 法定代表人身份证号
        /// C
        /// 借款人性质=2，企业时，必填
        /// </summary>
        public string borrowerLRID { get; set; }

        /// <summary>
        /// 手机号码
        /// N
        /// </summary>
        public string borrowerPhone { get; set; }

        /// <summary>
        /// 注册地址
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerRegisteredAddress { get; set; }

        /// <summary>
        /// 注册资本
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerRegisteredCapital { get; set; }

        /// <summary>
        /// 经营范围
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerBusinessScope { get; set; }

        /// <summary>
        /// 股东情况
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerShareholderInfo { get; set; }

        /// <summary>
        /// 资产状况说明
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerAssetInfo { get; set; }

        /// <summary>
        /// 办公地址
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerOfficeAddress { get; set; }

        /// <summary>
        /// 邮寄地址
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerPostAddress { get; set; }

        /// <summary>
        /// 邮编
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerPostcode { get; set; }

        /// <summary>
        /// 公司电话
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerCompanyTelephone { get; set; }

        /// <summary>
        /// 传真
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerFax { get; set; }

        /// <summary>
        /// 区域名称
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerArea { get; set; }

        /// <summary>
        /// 信用记录
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string borrowerCreditRecord { get; set; }

        /// <summary>
        /// 抵质押物信息
        /// Y
        /// </summary>
        public string collateralInformation { get; set; }

        /// <summary>
        /// 项目用途
        /// Y
        /// </summary>
        public string purpose { get; set; }

        /// <summary>
        /// 担保人信息列表
        /// N
        /// </summary>
        public List<SBHGuarantee> guaranteeList { get; set; }

        /// <summary>
        /// 推荐人信息列表
        /// N
        /// </summary>
        public List<SBHGuarantee> refereeList { get; set; }

        /// <summary>
        /// 利息管理费率
        /// N
        /// 0-1间的小数，如为空，以平台配置利息管理费率为准。
        /// </summary>
        public string interestRate { get; set; }

        /// <summary>
        /// 放款账户账号
        /// C
        /// 借款人为企业时填写-银行卡号
        /// </summary>
        public string borrowerAccountNo { get; set; }

        /// <summary>
        /// 放款账户户名
        /// C
        /// 借款人为企业时填写
        /// </summary>
        public string borrowerAccountName { get; set; }

        /// <summary>
        /// 自动投标
        /// N
        /// 渤海银行专用。0-不允许（默认） 1-允许
        /// </summary>
        public string autoFlag { get; set; } = "1";
    }

    /// <summary>
    /// 收融资计划
    /// </summary>
    public class SBHFund
    {
        /// <summary>
        /// 资金
        /// N
        /// 最后一期，资金项不填，表示剩余所有，其他期必填
        /// </summary>
        public string fund { get; set; }

        /// <summary>
        /// 收取时间
        /// Y
        /// 分批次可进行收取的期限日 格式：yyyy-MM-dd
        /// </summary>
        public string collectDate { get; set; }

        /// <summary>
        /// 借款手续费收取方式
        /// Y
        /// 1：固定金额
        /// 2：比例
        /// </summary>
        public string commissionFeeMode { get; set; }

        /// <summary>
        /// 借款手续费
        /// Y
        /// 固定金额时，填单笔金额；比例时，填0-1的小数。
        /// 收取方式为[比例] 时，手续费=本次收取的立即可用资金* 比例
        /// 用于收取融资时，与上报手续费核验
        /// </summary>
        public string commissionFee { get; set; }
    }

    /// <summary>
    /// 担保人信息
    /// </summary>
    public class SBHGuarantee
    {
        /// <summary>
        /// 担保人平台编码
        /// Y
        /// </summary>
        public string guaranteeUserCode { get; set; }

        /// <summary>
        /// 担保人名称
        /// Y
        /// </summary>
        public string guaranteeUserName { get; set; }

        /// <summary>
        /// 担保人性质
        /// Y
        /// 1个人
        /// 2企业
        /// </summary>
        public string guaranteeUserProperty { get; set; }

        /// <summary>
        /// 证件类型
        /// Y
        /// 码值.【证件类型】
        /// </summary>
        public string guaranteeUserIDType { get; set; }

        /// <summary>
        /// 证件号码
        /// Y
        /// 企业时为三证号，未更换三证号的填写营业执照编号
        /// </summary>
        public string guaranteeUserID { get; set; }

        /// <summary>
        /// 法定代表人姓名
        /// C
        /// 企业用户字段，企业时必填
        /// </summary>
        public string guaranteeLRName { get; set; }

        /// <summary>
        /// 法定代表人身份证号
        /// C
        /// 企业用户字段，企业时必填
        /// </summary>
        public string guaranteeLRID { get; set; }

        /// <summary>
        /// 手机号码
        /// N
        /// </summary>
        public string guaranteePhone { get; set; }

        /// <summary>
        /// 注册地址
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeRegisteredAddress { get; set; }

        /// <summary>
        /// 注册资本
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeRegisteredCapital { get; set; }

        /// <summary>
        /// 经营范围
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeBusinessScope { get; set; }

        /// <summary>
        /// 股东情况
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeShareholderInfo { get; set; }

        /// <summary>
        /// 资产状况说明
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeAssetInfo { get; set; }

        /// <summary>
        /// 办公地址
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeOfficeAddress { get; set; }

        /// <summary>
        /// 邮寄地址
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteePostAddress { get; set; }

        /// <summary>
        /// 邮编
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteePostcode { get; set; }

        /// <summary>
        /// 公司电话
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeCompanyTelephone { get; set; }

        /// <summary>
        /// 传真
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeFax { get; set; }

        /// <summary>
        /// 区域名称
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeArea { get; set; }

        /// <summary>
        /// 担保记录
        /// N
        /// 企业时填写，非必填
        /// </summary>
        public string guaranteeCreditRecord { get; set; }

    }

    /// <summary>
    /// 推荐人信息
    /// </summary>
    public class SBHReferee
    {
        /// <summary>
        /// 推荐人平台编码
        /// N
        /// </summary>
        public string refereeUserCode { get; set; }

        /// <summary>
        /// 推荐人名称
        /// Y
        /// </summary>
        public string refereeUserName { get; set; }

        /// <summary>
        /// 推荐人性质
        /// Y
        /// 1个人
        /// 2企业
        /// </summary>
        public string refereeUserProperty { get; set; }

        /// <summary>
        /// 证件类型
        /// N
        /// 码值.【证件类型】
        /// </summary>
        public string refereeUserIDType { get; set; }

        /// <summary>
        /// 证件号码
        /// N
        /// 企业时为三证号，未更换三证号的填写营业执照编号
        /// </summary>
        public string refereeUserID { get; set; }

        /// <summary>
        /// 法定代表人姓名
        /// C
        /// 企业用户字段，企业时必填
        /// </summary>
        public string refereeLRName { get; set; }

        /// <summary>
        /// 法定代表人身份证号
        /// C
        /// 企业用户字段，企业时必填
        /// </summary>
        public string refereeLRID { get; set; }

        /// <summary>
        /// 手机号码
        /// N
        /// </summary>
        public string refereePhone { get; set; }
    }
}