using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core
{
    public class DataDictionary
    {

        /// <summary>
        /// 证件类型
        /// </summary>
        public static int documenttype = 43;

        /// <summary>
        /// 手续费
        /// </summary>
        public static int feetype_CounterFee = 59;

        /// <summary>
        /// 管理费
        /// </summary>
        public static int feetype_Managementexpense = 60;

        /// <summary>
        /// 担保费
        /// </summary>
        public static int feetype_Guaranteefee = 61;

        /// <summary>
        /// 提示类型
        /// </summary>
        public static int prompttype = 74;

        /// <summary>
        /// 提示方式
        /// </summary>
        public static int promptway = 75;

        /// <summary>
        /// 到期提示
        /// </summary>
        public static int prompttype_Expirationtips = 76;

        /// <summary>
        /// 逾期催收
        /// </summary>
        public static int prompttype_Overduecollection = 77;

        /// <summary>
        /// 短信
        /// </summary>
        public static int promptway_ShortMessage = 78;

        /// <summary>
        /// 电话
        /// </summary>
        public static int promptway_Telephone = 79;

        /// <summary>
        /// 上门催收
        /// </summary>
        public static int promptway_Door = 80;

        /// <summary>
        /// 单位性质
        /// </summary>
        public static int unitproperty = 81;

        /// <summary>
        /// 政府机构
        /// </summary>
        public static int unitproperty_Govt = 82;

        /// <summary>
        /// 国有企业
        /// </summary>
        public static int unitproperty_StateOwned = 83;

        /// <summary>
        /// 台（港、澳）资企业
        /// </summary>
        public static int unitproperty_TaiwanHongKongMacao = 84;

        /// <summary>
        /// 合资企业
        /// </summary>
        public static int unitproperty_JointVenture = 85;

        /// <summary>
        /// 个体户
        /// </summary>
        public static int unitproperty_Sole = 86;

        /// <summary>
        /// 事业性单位
        /// </summary>
        public static int unitproperty_CareerUnit = 87;

        /// <summary>
        /// 私营企业
        /// </summary>
        public static int unitproperty_PrivateCompanies = 88;

        /// <summary>
        /// 工作年限
        /// </summary>
        public static int workingyears = 89;

        /// <summary>
        /// 一年以内
        /// </summary>
        public static int workingyears_OneWithin = 90;

        /// <summary>
        /// 一年以上
        /// </summary>
        public static int workingyears_OneOver = 91;

        /// <summary>
        /// 二年以上
        /// </summary>
        public static int workingyears_TwoOver = 92;

        /// <summary>
        /// 三年以上
        /// </summary>
        public static int workingyears_ThreeOver = 93;

        /// <summary>
        /// 四年以上
        /// </summary>
        public static int workingyears_FourOver = 94;

        /// <summary>
        /// 五年以上
        /// </summary>
        public static int workingyears_FiveOver = 95;

        /// <summary>
        /// 六年以上
        /// </summary>
        public static int workingyears_SixOver = 96;

        /// <summary>
        /// 转让状态
        /// </summary>
        public static int transferstatus = 107;

        /// <summary>
        /// 待审核
        /// </summary>
        public static int transferstatus_Pendingaudit = 108;

        /// <summary>
        /// 企业规模
        /// </summary>
        public static int enterprisesize = 39;

        /// <summary>
        /// 众筹类型
        /// </summary>
        public static int ModelType = 162;

        /// <summary>
        /// 股权
        /// </summary>
        public static int ModelType_Equity = 163;

        /// <summary>
        /// 启用
        /// </summary>
        public static int ModelTypeState_Enable = 168;

        /// <summary>
        /// 未审核
        /// </summary>
        public static int AuditState_Not = 171;

        /// <summary>
        /// 未审核
        /// </summary>
        public static int ProjectAuditState_Not = 177;

        /// <summary>
        /// 评论
        /// </summary>
        public static int CommentType_Comments = 184;

        /// <summary>
        /// 编辑信息
        /// </summary>
        public static int RaiseOperType_Editing = 189;

        /// <summary>
        /// 支出
        /// </summary>
        public static int ReturnType_Pay = 196;

        /// <summary>
        /// 个人
        /// </summary>
        public static int InvestorType_Personal = 201;

        /// <summary>
        /// 个人
        /// </summary>
        public static int Raise_InvestorType_Personal = 321;

        /// <summary>
        /// 图片
        /// </summary>
        public static int Raise_AnnexMediaType_Pic = 327;

        /// <summary>
        /// 所有
        /// </summary>
        public static int Raise_AnnexCan_All = 336;

        /// <summary>
        /// 无担保
        /// </summary>
        public static int Raise_info_GuaranteeModel_No = 341;

        /// <summary>
        /// 房屋
        /// </summary>
        public static int Raise_ProGuaranty_House = 345;

        /// <summary>
        /// 前台
        /// </summary>
        public static int Raise_ProSource_Front = 358;

        /// <summary>
        /// 满标划转
        /// </summary>
        public static int RaiseCapTran_FullTransfer = 425;

        /// <summary>
        /// 已划转
        /// </summary>
        public static int RaiseFundsTransferState_Haved = 432;

        /// <summary>
        /// 强制满标
        /// </summary>
        public static int RaiseFullType_Mandatory = 435;

        /// <summary>
        /// 保证金支付
        /// </summary>
        public static int RecordType_BondPay = 536;

        /// <summary>
        /// 退还
        /// </summary>
        public static int RecordType_Back = 543;

        /// <summary>
        /// 项目终止退还
        /// </summary>
        public static int RecordType_StopBack = 666;

        /// <summary>
        /// 合同类型
        /// </summary>
        public static int RaiseContractType = 677;

        /// <summary>
        /// 股权众筹项目合同
        /// </summary>
        public static int RaiseContractType_Equity = 678;

        /// <summary>
        /// 产品众筹项目合同
        /// </summary>
        public static int RaiseContractType_Reward = 679;

        /// <summary>
        /// 公益众筹项目合同
        /// </summary>
        public static int RaiseContractType_Donate = 680;

        /// <summary>
        /// 股权项目领投合同
        /// </summary>
        public static int RaiseContractType_Leader = 681;

        /// <summary>
        /// 股权项目投资合同
        /// </summary>
        public static int RaiseContractType_EquityInv = 682;

        /// <summary>
        /// 项目支持合同
        /// </summary>
        public static int RaiseContractType_Support = 683;

        /// <summary>
        /// 项目无私支持合同
        /// </summary>
        public static int RaiseContractType_DonateSupport = 684;

        /// <summary>
        /// 金额类型
        /// </summary>
        public static int MoneyType = 643;

        /// <summary>
        /// 投资余额
        /// </summary>
        public static int MoneyType_InvertMent = 644;

        /// <summary>
        /// 加盟商
        /// </summary>
        public static int albType_aLb = 651;

        /// <summary>
        /// 投资金额退还
        /// </summary>
        public static int RaiseCapTran_InvBack = 653;

        /// <summary>
        /// 平台红包划转
        /// </summary>
        public static int RaiseCapTran_RedTransfer = 654;

        /// <summary>
        /// 线上支付
        /// </summary>
        public static int SettlementPaymentType_OnLine = 656;

        /// <summary>
        /// 加盟商提成审核状态
        /// </summary>
        public static int albAuditStatus = 659;

        /// <summary>
        /// 通过
        /// </summary>
        public static int albAuditStatus_Agree = 660;

        /// <summary>
        /// 邀请人提成规则
        /// </summary>
        public static int deductRuleType_invitor = 606;

        /// <summary>
        /// 抽奖
        /// </summary>
        public static int RewardReturnLogType_LuckDraw = 604;

        /// <summary>
        /// 邀请注册数
        /// </summary>
        public static int deductType_register = 610;

        /// <summary>
        /// 借款成功一次结算
        /// </summary>
        public static int payType_borrowSuc = 619;

        /// <summary>
        /// 按人均固定值
        /// </summary>
        public static int counterFee_fixedValuePer = 627;

        /// <summary>
        /// 短信
        /// </summary>
        public static int RaiseMsgType_Sms = 633;

        /// <summary>
        /// 待发送审核
        /// </summary>
        public static int SettlementStatus_PendingVerification = 637;

        /// <summary>
        /// 积分
        /// </summary>
        public static int PrizeType_Score = 687;

        /// <summary>
        /// 一等奖
        /// </summary>
        public static int PrizeLevel_One = 691;

        /// <summary>
        /// 跳转方式
        /// </summary>
        public static int JumpMode = 698;

        /// <summary>
        /// 当前页
        /// </summary>
        public static int JumpMode_CurrentPage = 699;

        /// <summary>
        /// 新打开标签
        /// </summary>
        public static int JumpMode_OpenLabel = 700;

        /// <summary>
        /// 当前标签
        /// </summary>
        public static int JumpMode_CurrentLabel = 701;

        /// <summary>
        /// 启用
        /// </summary>
        public static int PrizeState_Enable = 696;

        /// <summary>
        /// T+0起息
        /// </summary>
        public static int InterestType_T0 = 704;

        /// <summary>
        /// 转入
        /// </summary>
        public static int FinancialTransType_TranIn = 710;

        /// <summary>
        /// 待兑换
        /// </summary>
        public static int OrderStatus_Converted = 714;

        /// <summary>
        /// 活期理财
        /// </summary>
        public static int FinancialType_Current = 707;

        /// <summary>
        /// 领投金额支付
        /// </summary>
        public static int RecordType_LeaderPay = 537;

        /// <summary>
        /// 两年以上创业经验（只限第一创始人经验）
        /// </summary>
        public static int RaiseLed_Entrepreneurship = 508;

        /// <summary>
        /// 三年以上企业总监级以上岗位工作经验
        /// </summary>
        public static int RaiseLed_ThreeYears = 509;

        /// <summary>
        /// 五年以上企业经理级岗位工作经验
        /// </summary>
        public static int RaiseLed_FiveYears = 510;

        /// <summary>
        /// 两个以上奥拓思维投资案例
        /// </summary>
        public static int RaiseLed_TwoCase = 511;

        /// <summary>
        /// 项目分红
        /// </summary>
        public static int RecordType_Bonus = 552;

        /// <summary>
        /// 项目投资
        /// </summary>
        public static int RecordType_InvestPay = 553;

        /// <summary>
        /// 申请划转
        /// </summary>
        public static int RaiseFundsTransferState_Apply = 554;

        /// <summary>
        /// 领投奖励
        /// </summary>
        public static int RecordType_LeaderInvReward = 555;

        /// <summary>
        /// 退回审核状态
        /// </summary>
        public static int backapply = 556;

        /// <summary>
        /// 待审核
        /// </summary>
        public static int backapply_StayAudit = 557;

        /// <summary>
        /// 创始人
        /// </summary>
        public static int RaiseIdentityType_Founder = 476;

        /// <summary>
        /// 团队合伙人
        /// </summary>
        public static int RaiseIdentityType_TeamPartners = 477;

        /// <summary>
        /// 服务费划转
        /// </summary>
        public static int RaiseCapTran_PoundageTransfer = 563;

        /// <summary>
        /// 领投保证金划转
        /// </summary>
        public static int RaiseCapTran_LeaderTransfer = 564;

        /// <summary>
        /// 图文列表
        /// </summary>
        public static int NewsTemplate_Graphic = 566;

        /// <summary>
        /// 个人投资资格申请
        /// </summary>
        public static int Qa_PersonInvUserApply = 572;

        /// <summary>
        /// 常规
        /// </summary>
        public static int RewardReturnWay_Rule = 578;

        /// <summary>
        /// 申请审核
        /// </summary>
        public static int RaiseOperType_CacnelAuditSuccessApply = 580;

        /// <summary>
        /// 公益
        /// </summary>
        public static int EntreprenProp_Welfare = 582;

        /// <summary>
        /// 国营
        /// </summary>
        public static int CompanyProp_StateRun = 586;

        /// <summary>
        /// 股权
        /// </summary>
        public static int InvExpReturnType_Equity = 590;

        /// <summary>
        /// 领投人
        /// </summary>
        public static int InvExpRole_Leader = 596;

        /// <summary>
        /// 领投代表人
        /// </summary>
        public static int RaiseDueUploadSource_LeaderInvRep = 600;

        /// <summary>
        /// 自动满标
        /// </summary>
        public static int RaiseFullType_Automatic = 437;

        /// <summary>
        /// 未划转
        /// </summary>
        public static int RaiseFundsTransferState_Not = 433;

        /// <summary>
        /// 结清划转
        /// </summary>
        public static int RaiseCapTran_SettleTransfer = 428;

        /// <summary>
        /// 下单
        /// </summary>
        public static int RewardReturnLogType_Order = 443;

        /// <summary>
        /// 成功
        /// </summary>
        public static int transactionstatus_success = 462;

        /// <summary>
        /// 取消订单
        /// </summary>
        public static int RewardReturnLogType_CancelOrder = 471;

        /// <summary>
        /// 删除订单
        /// </summary>
        public static int RewardReturnLogType_DeleteOrder = 472;

        /// <summary>
        /// 撤回订单
        /// </summary>
        public static int RewardReturnLogType_RevokeOrder = 473;

        /// <summary>
        /// 我的年收入超过30万元
        /// </summary>
        public static int InvestorPeCase_MoreThanThree = 498;

        /// <summary>
        /// 我是专业的风险投资人
        /// </summary>
        public static int InvestorPeCase_Professional = 499;

        /// <summary>
        /// 我不符合上述任何条件之一
        /// </summary>
        public static int InvestorPeCase_Not = 500;

        /// <summary>
        /// 我们公司净资产不低于1000万元
        /// </summary>
        public static int InvestComCase_MoreThanThousand = 503;

        /// <summary>
        /// 我们公司是专业的投资公司
        /// </summary>
        public static int InvestComCase_Professional = 504;

        /// <summary>
        /// 我不符合上述任何条件之一
        /// </summary>
        public static int InvestComCase_Not = 505;

        /// <summary>
        /// 未确认
        /// </summary>
        public static int RaiseInterView_NotSure = 513;

        /// <summary>
        /// 已确认
        /// </summary>
        public static int RaiseInterView_Sure = 514;

        /// <summary>
        /// 已拒绝
        /// </summary>
        public static int RaiseInterView_Refused = 515;

        /// <summary>
        /// 已保存
        /// </summary>
        public static int RaiseState_Saved = 405;

        /// <summary>
        /// 等额本金
        /// </summary>
        public static int ProPayType_AverageCapital = 380;

        /// <summary>
        /// 投标类型
        /// </summary>
        public static int investType = 395;

        /// <summary>
        /// 正常标
        /// </summary>
        public static int investType_normal = 396;

        /// <summary>
        /// 否
        /// </summary>
        public static int RealProveStatus_no = 400;

        /// <summary>
        /// 后台
        /// </summary>
        public static int Raise_ProSource_BackGround = 359;

        /// <summary>
        /// 用户资金交易类型
        /// </summary>
        public static int transactiontype = 366;

        /// <summary>
        /// 项目投资-冻结金额
        /// </summary>
        public static int transactiontype_InvestFrazon = 367;

        /// <summary>
        /// 汽车
        /// </summary>
        public static int Raise_ProGuaranty_Car = 346;

        /// <summary>
        /// 担保物
        /// </summary>
        public static int Raise_info_GuaranteeModel_Collateral = 342;

        /// <summary>
        /// 会员
        /// </summary>
        public static int Raise_AnnexCan_Member = 337;

        /// <summary>
        /// 视频
        /// </summary>
        public static int Raise_AnnexMediaType_Video = 328;

        /// <summary>
        /// 机构
        /// </summary>
        public static int Raise_InvestorType_Mechanism = 322;

        /// <summary>
        /// 失败
        /// </summary>
        public static int RecordState_Failure = 325;

        /// <summary>
        /// 正常
        /// </summary>
        public static int RaiseUserState_Enable = 310;

        /// <summary>
        /// 增加
        /// </summary>
        public static int RaiseIntegralType_Add = 313;

        /// <summary>
        /// 众筹项目类型
        /// </summary>
        public static int ModelTypeState = 167;

        /// <summary>
        /// 未回复
        /// </summary>
        public static int InsideBox_NoReply = 318;

        /// <summary>
        /// 首页轮播
        /// </summary>
        public static int AdPlacement_HomeCarousel = 294;

        /// <summary>
        /// 一级省
        /// </summary>
        public static int AreaType_Pro = 299;

        /// <summary>
        /// 邮箱注册
        /// </summary>
        public static int RaiseUserSource_Email = 303;

        /// <summary>
        /// 图片
        /// </summary>
        public static int AdType_Pic = 297;

        /// <summary>
        /// 个人
        /// </summary>
        public static int RaiseUserType_Personal = 307;

        /// <summary>
        /// 未支付
        /// </summary>
        public static int RaiseOrderState_Unpaid = 223;

        /// <summary>
        /// 付款
        /// </summary>
        public static int RewardReturnLogType_Pay = 230;

        /// <summary>
        /// 概念
        /// </summary>
        public static int RewardStage_Idea = 235;

        /// <summary>
        /// 概念
        /// </summary>
        public static int EquityStage_Idea = 249;

        /// <summary>
        /// 未付款
        /// </summary>
        public static int LoanInvestmenState_Unpaid = 276;

        /// <summary>
        /// 积分动作类型
        /// </summary>
        public static int scoreusetype = 281;

        /// <summary>
        /// 获得积分
        /// </summary>
        public static int scoreusetype_get = 282;

        /// <summary>
        /// 等额本息
        /// </summary>
        public static int LoanRepaymentType_Matching = 272;

        /// <summary>
        /// 未还款
        /// </summary>
        public static int LoanRepaymentState_Unpaid = 285;

        /// <summary>
        /// 申请中
        /// </summary>
        public static int LoadTransferState_Apply = 290;

        /// <summary>
        /// 机构
        /// </summary>
        public static int InvestorType_Mechanism = 202;

        /// <summary>
        /// 虚拟
        /// </summary>
        public static int RewardReturnType_Virtual = 220;

        /// <summary>
        /// 收入
        /// </summary>
        public static int ReturnType_Income = 197;

        /// <summary>
        /// 申请审核
        /// </summary>
        public static int RaiseOperType_Apply = 190;

        /// <summary>
        /// 回复
        /// </summary>
        public static int CommentType_Reply = 186;

        /// <summary>
        /// 申请审核
        /// </summary>
        public static int ProjectAuditState_Apply = 178;

        /// <summary>
        /// 审核中
        /// </summary>
        public static int AuditState_Ing = 173;

        /// <summary>
        /// 禁用
        /// </summary>
        public static int ModelTypeState_Disable = 169;

        /// <summary>
        /// 债权
        /// </summary>
        public static int ModelType_Lending = 164;

        /// <summary>
        /// 同意
        /// </summary>
        public static int auditstatus_Agree = 124;

        /// <summary>
        /// 意向审核
        /// </summary>
        public static int auditlink_Intentaudit = 118;

        /// <summary>
        /// 待审核
        /// </summary>
        public static int realname_Pendingaudit = 142;

        /// <summary>
        /// 项目影像资料
        /// </summary>
        public static int filmsdatatype_Project = 145;

        /// <summary>
        /// 日
        /// </summary>
        public static int bearingway_Day = 148;

        /// <summary>
        /// 前台还款
        /// </summary>
        public static int RepaymentType_FrontRepay = 153;

        /// <summary>
        /// 50人以下
        /// </summary>
        public static int enterprisesize_FiftyFollowing = 40;

        /// <summary>
        /// 投资方式一
        /// </summary>
        public static int investmentway_11101 = 56;

        /// <summary>
        /// 转让初审
        /// </summary>
        public static int transferstatuslink_Prelim = 115;

        /// <summary>
        /// 初审已通过
        /// </summary>
        public static int transferstatus_HasThrough = 109;

        /// <summary>
        /// 发送状态
        /// </summary>
        public static int sendstate = 100;

        /// <summary>
        /// 待发送
        /// </summary>
        public static int sendstate_12101 = 101;

        /// <summary>
        /// 身份证
        /// </summary>
        public static int documenttype_ID = 44;

        /// <summary>
        /// 正常还款
        /// </summary>
        public static int repaymenstate_Normal = 27;

        

        /// <summary>
        /// 笔
        /// </summary>
        public static int chargeway_Pen = 6;

        /// <summary>
        /// 固定还款日
        /// </summary>
        public static int settlementway_Fixed = 31;

        /// <summary>
        /// 网贷3.0项目数据字典
        /// </summary>
        public static int datadictionary = 1;

        /// <summary>
        /// 期限类型
        /// </summary>
        public static int deadlinetype = 2;

        /// <summary>
        /// 月
        /// </summary>
        public static int deadlinetype_Month = 4;

        /// <summary>
        /// 个人
        /// </summary>
        public static int projecttype_Individual = 9;

        /// <summary>
        /// 待发送
        /// </summary>
        public static int projectstate_StaySend = 14;

        /// <summary>
        /// 待平台审核
        /// </summary>
        public static int projectstate_StayPlatformaudit = 15;

        /// <summary>
        /// 企业
        /// </summary>
        public static int projecttype_Enterprise = 10;

        /// <summary>
        /// 日
        /// </summary>
        public static int deadlinetype_Day = 5;

        /// <summary>
        /// 收费方式
        /// </summary>
        public static int chargeway = 3;

        /// <summary>
        /// 比例
        /// </summary>
        public static int chargeway_Proportion = 7;

        /// <summary>
        /// 强制还款
        /// </summary>
        public static int repaymenstate_Forced = 28;

        /// <summary>
        /// 临时身份证
        /// </summary>
        public static int documenttype_TemporaryID = 45;

        /// <summary>
        /// 已发送平台
        /// </summary>
        public static int sendstate_12102 = 102;

        /// <summary>
        /// 初审未通过
        /// </summary>
        public static int transferstatus_NotThrough = 110;

        /// <summary>
        /// 满标待审
        /// </summary>
        public static int transferstatus_FullScalePending = 111;

        /// <summary>
        /// 待划转
        /// </summary>
        public static int transferstatus_StayTransfer = 112;

        /// <summary>
        /// 已划转
        /// </summary>
        public static int transferstatus_HasTransfer = 113;

        /// <summary>
        /// 转让满标审核
        /// </summary>
        public static int transferstatuslink_FullScaleaudit = 116;

        /// <summary>
        /// 投资方式二
        /// </summary>
        public static int investmentway_11102 = 57;

        /// <summary>
        /// 50~500人
        /// </summary>
        public static int enterprisesize_FiftyToFiveHundred = 41;

        /// <summary>
        /// 前台还平台代还
        /// </summary>
        public static int RepaymentType_FrontRepayDaihuan = 154;

        /// <summary>
        /// 月
        /// </summary>
        public static int bearingway_Month = 149;

        /// <summary>
        /// 非固定还款日
        /// </summary>
        public static int settlementway_NotFixed = 150;

        /// <summary>
        /// 项目初审
        /// </summary>
        public static int auditlink_Projectprelim = 119;

        /// <summary>
        /// 已退回
        /// </summary>
        public static int auditstatus_HasReturn = 125;

        /// <summary>
        /// 产品
        /// </summary>
        public static int ModelType_Reward = 165;

        /// <summary>
        /// 认证未通过
        /// </summary>
        public static int realname_Notthrough = 141;

        /// <summary>
        /// 众筹项目数据字典
        /// </summary>
        public static int crowdfunding = 161;

        /// <summary>
        /// 审核成功
        /// </summary>
        public static int AuditState_Success = 174;

        /// <summary>
        /// 客服审核通过
        /// </summary>
        public static int ProjectAuditState_ServiceSuccess = 179;

        /// <summary>
        /// 官方解释
        /// </summary>
        public static int CommentType_Official = 187;

        /// <summary>
        /// 其他
        /// </summary>
        public static int RecordType_Others = 198;

        /// <summary>
        /// 实物
        /// </summary>
        public static int RewardReturnType_Material = 221;

        /// <summary>
        /// 已转让
        /// </summary>
        public static int LoadTransferState_Transfer = 291;

        /// <summary>
        /// 等额本金
        /// </summary>
        public static int LoanRepaymentType_Principal = 273;

        /// <summary>
        /// 使用积分
        /// </summary>
        public static int scoreusetype_use = 283;

        /// <summary>
        /// 已付款
        /// </summary>
        public static int LoanInvestmenState_Pay = 277;

        /// <summary>
        /// 研发中
        /// </summary>
        public static int EquityStage_Developing = 250;

        /// <summary>
        /// 研发中
        /// </summary>
        public static int RewardStage_Developing = 236;

        /// <summary>
        /// 发货
        /// </summary>
        public static int RewardReturnLogType_Deliver = 231;

        /// <summary>
        /// 已支付
        /// </summary>
        public static int RaiseOrderState_HavedPad = 224;

        /// <summary>
        /// 机构
        /// </summary>
        public static int RaiseUserType_Mechanism = 308;

        /// <summary>
        /// 手机注册
        /// </summary>
        public static int RaiseUserSource_Phone = 304;

        /// <summary>
        /// 二级市
        /// </summary>
        public static int AreaType_City = 300;

        /// <summary>
        /// 左侧广告
        /// </summary>
        public static int AdPlacement_LeftAdvert = 295;

        /// <summary>
        /// 已还款
        /// </summary>
        public static int LoanRepaymentState_Pay = 287;

        /// <summary>
        /// 已转让
        /// </summary>
        public static int LoanRepaymentState_Transfer = 288;

        /// <summary>
        /// 减少
        /// </summary>
        public static int RaiseIntegralType_Reduce = 314;

        /// <summary>
        /// 禁用
        /// </summary>
        public static int RaiseUserState_Disable = 311;

        /// <summary>
        /// PDF
        /// </summary>
        public static int Raise_AnnexMediaType_Pdf = 329;

        /// <summary>
        /// 实名认证会员
        /// </summary>
        public static int Raise_AnnexCan_RealNameMem = 338;

        /// <summary>
        /// 保证金
        /// </summary>
        public static int Raise_info_GuaranteeModel_Bond = 343;

        /// <summary>
        /// 客服审核
        /// </summary>
        public static int RaiseOperType_Service = 347;

        /// <summary>
        /// 项目投资-解冻金额
        /// </summary>
        public static int transactiontype_InvestThaw = 368;

        /// <summary>
        /// 实名认证审核中
        /// </summary>
        public static int RealProveStatus_Audit = 401;

        /// <summary>
        /// 转让标
        /// </summary>
        public static int investType_transfer = 397;

        /// <summary>
        /// 按月还息到期还本
        /// </summary>
        public static int ProPayType_MonthlyReturn = 381;

        /// <summary>
        /// 申请审核
        /// </summary>
        public static int RaiseState_Apply = 406;

        /// <summary>
        /// 团队照片
        /// </summary>
        public static int Raise_AnnexType_TeamPic = 333;

        /// <summary>
        /// 失败
        /// </summary>
        public static int transactionstatus_failed = 463;

        /// <summary>
        /// 意向审核
        /// </summary>
        public static int LoanStepName_Intention = 479;

        /// <summary>
        /// 满标划转手续费划转
        /// </summary>
        public static int RaiseCapTran_FullTranFee = 444;

        /// <summary>
        /// 保证金划转
        /// </summary>
        public static int RaiseCapTran_MarginTran = 429;

        /// <summary>
        /// 平台客服
        /// </summary>
        public static int RaiseDueUploadSource_Service = 601;

        /// <summary>
        /// 跟投人
        /// </summary>
        public static int InvExpRole_Inv = 597;

        /// <summary>
        /// 股权详情页
        /// </summary>
        public static int Qa_EquityInfo = 598;

        /// <summary>
        /// 产品
        /// </summary>
        public static int InvExpReturnType_Product = 591;

        /// <summary>
        /// 公益
        /// </summary>
        public static int InvExpReturnType_Welfare = 592;

        /// <summary>
        /// 利润
        /// </summary>
        public static int InvExpReturnType_Profit = 593;

        /// <summary>
        /// 其他
        /// </summary>
        public static int InvExpReturnType_Other = 594;

        /// <summary>
        /// 私营
        /// </summary>
        public static int CompanyProp_Private = 587;

        /// <summary>
        /// 其他
        /// </summary>
        public static int CompanyProp_Other = 588;

        /// <summary>
        /// 赚钱
        /// </summary>
        public static int EntreprenProp_MakeMoney = 583;

        /// <summary>
        /// 其他
        /// </summary>
        public static int EntreprenProp_Other = 584;

        /// <summary>
        /// 抽奖
        /// </summary>
        public static int RewardReturnWay_LuckDraw = 579;

        /// <summary>
        /// 机构投资资格申请
        /// </summary>
        public static int Qa_ComInvUserApply = 573;

        /// <summary>
        /// 领投人资格申请
        /// </summary>
        public static int Qa_LeaderInvUserApply = 574;

        /// <summary>
        /// 发起项目
        /// </summary>
        public static int Qa_LaunchProjectApply = 575;

        /// <summary>
        /// 回报计划
        /// </summary>
        public static int Qa_ReturnPlan = 576;

        /// <summary>
        /// 文字列表
        /// </summary>
        public static int NewsTemplate_Text = 567;

        /// <summary>
        /// 退回通过
        /// </summary>
        public static int backapply_Through = 558;

        /// <summary>
        /// 定期理财
        /// </summary>
        public static int FinancialType_Regularly = 708;

        /// <summary>
        /// 待发货
        /// </summary>
        public static int OrderStatus_FinancialTransType_Shipped = 715;

        /// <summary>
        /// 转出
        /// </summary>
        public static int FinancialTransType_TranOut = 711;

        /// <summary>
        /// 满标起息
        /// </summary>
        public static int InterestType_Full = 705;

        /// <summary>
        /// 禁用
        /// </summary>
        public static int PrizeState_Disabled = 697;

        /// <summary>
        /// 二等奖
        /// </summary>
        public static int PrizeLevel_Two = 692;

        /// <summary>
        /// 红包
        /// </summary>
        public static int PrizeType_Red = 688;

        /// <summary>
        /// 待审核
        /// </summary>
        public static int SettlementStatus_PendingAudit = 638;

        /// <summary>
        /// 邮件
        /// </summary>
        public static int RaiseMsgType_Email = 634;

        /// <summary>
        /// 按金额固定值
        /// </summary>
        public static int counterFee_fixedValueMoney = 628;

        /// <summary>
        /// 按项目分次还款结算
        /// </summary>
        public static int payType_perBackMoney = 620;

        /// <summary>
        /// 实名认证数
        /// </summary>
        public static int deductType_realprove = 611;

        /// <summary>
        /// 邀请投资提成规则
        /// </summary>
        public static int deductRuleType_invest = 607;

        /// <summary>
        /// 不通过
        /// </summary>
        public static int albAuditStatus_DisAgree = 661;

        /// <summary>
        /// 满标划转
        /// </summary>
        public static int RecordType_FullTransfer = 662;

        /// <summary>
        /// 结清划转
        /// </summary>
        public static int RecordType_SettleTransfer = 663;

        /// <summary>
        /// 线下银行转账
        /// </summary>
        public static int SettlementPaymentType_BankTransfer = 657;

        /// <summary>
        /// 支付方式
        /// </summary>
        public static int SettlementPaymentType = 655;

        /// <summary>
        /// 加盟商员工
        /// </summary>
        public static int albType_albEmp = 652;

        /// <summary>
        /// 加盟商类型
        /// </summary>
        public static int albType = 650;

        /// <summary>
        /// 借款余额
        /// </summary>
        public static int MoneyType_Lend = 645;

        /// <summary>
        /// 满标投资划转
        /// </summary>
        public static int RecordType_InvFullTransfer = 667;

        /// <summary>
        /// 满标领投划转
        /// </summary>
        public static int RecordType_LeadFullTransfer = 668;

        /// <summary>
        /// 领投保证金退回划转
        /// </summary>
        public static int RaiseCapTran_BondTransfer = 669;

        /// <summary>
        /// 全部
        /// </summary>
        public static int MoneyType_All = 646;

        /// <summary>
        /// 预热超时
        /// </summary>
        public static int RaiseOperType_PreheatDue = 642;

        /// <summary>
        /// 线下现金支付
        /// </summary>
        public static int SettlementPaymentType_CashPayment = 658;

        /// <summary>
        /// 众筹中未到期终止
        /// </summary>
        public static int RaiseStopType_IngNotTime = 664;

        /// <summary>
        /// 满标申请已满标终止
        /// </summary>
        public static int RaiseStopType_FullApplyHavedFull = 665;

        /// <summary>
        /// 邀请借款提成规则
        /// </summary>
        public static int deductRuleType_borrow = 608;

        /// <summary>
        /// 项目结清后一次结算
        /// </summary>
        public static int payType_PaySuc = 621;

        /// <summary>
        /// 按金额百分比
        /// </summary>
        public static int counterFee_percentMoney = 629;

        /// <summary>
        /// 投资总额
        /// </summary>
        public static int deductType_investSum = 613;

        /// <summary>
        /// 站内信
        /// </summary>
        public static int RaiseMsgType_Letter = 635;

        /// <summary>
        /// 待支付
        /// </summary>
        public static int SettlementStatus_ToPaid = 639;

        /// <summary>
        /// 其它
        /// </summary>
        public static int PrizeType_Other = 689;

        /// <summary>
        /// 三等奖
        /// </summary>
        public static int PrizeLevel_Three = 693;

        /// <summary>
        /// 清算
        /// </summary>
        public static int FinancialTransType_Clear = 712;

        /// <summary>
        /// 已发货
        /// </summary>
        public static int OrderStatus_FinancialTransType_Delivered = 716;

        /// <summary>
        /// 退回不通过
        /// </summary>
        public static int backapply_NotThrough = 559;

        /// <summary>
        /// 分红明细状态
        /// </summary>
        public static int RaiseBonusLogState = 560;

        /// <summary>
        /// 支付成功
        /// </summary>
        public static int RaiseBonusLogState_PaySuccess = 561;

        /// <summary>
        /// 支付失败
        /// </summary>
        public static int RaiseBonusLogState_PayFail = 562;

        /// <summary>
        /// 已退还
        /// </summary>
        public static int RaiseBondState_HaveBack = 551;

        /// <summary>
        /// 约谈状态
        /// </summary>
        public static int RaiseInterView = 512;

        /// <summary>
        /// 领投奖励类型
        /// </summary>
        public static int RaiseLedRewardType = 544;

        /// <summary>
        /// 跟投奖励
        /// </summary>
        public static int RaiseLedRewardType_Follow = 545;

        /// <summary>
        /// 管理奖励
        /// </summary>
        public static int RaiseLedRewardType_Manage = 546;

        /// <summary>
        /// 领投奖励方式
        /// </summary>
        public static int RaiseLedRewardMethod = 547;

        /// <summary>
        /// 奖励金额
        /// </summary>
        public static int RaiseLedRewardMethod_Money = 548;

        /// <summary>
        /// 奖励股份
        /// </summary>
        public static int RaiseLedRewardMethod_Equtiy = 549;

        /// <summary>
        /// 问答对话
        /// </summary>
        public static int NewsTemplate_Dialogue = 568;

        /// <summary>
        /// 成功后已操作
        /// </summary>
        public static int AuditState_SuccessOper = 445;

        /// <summary>
        /// 项目暂停类型
        /// </summary>
        public static int RaiseProPause = 446;

        /// <summary>
        /// 项目上线暂停
        /// </summary>
        public static int RaiseProPause_OnLine = 447;

        /// <summary>
        /// 项目预热暂停
        /// </summary>
        public static int RaiseProPause_preheating = 448;

        /// <summary>
        /// 项目终止类型
        /// </summary>
        public static int RaiseStopType = 449;

        /// <summary>
        /// 众筹中到期未满标
        /// </summary>
        public static int RaiseStopType_IngNotFull = 450;

        /// <summary>
        /// 预热中未到期终止
        /// </summary>
        public static int RaiseStopType_PreNotTime = 451;

        /// <summary>
        /// 众筹中已满标终止
        /// </summary>
        public static int RaiseStopType_IngHavedFull = 452;

        /// <summary>
        /// 项目初审
        /// </summary>
        public static int LoanStepName_ProjectAssesment = 480;

        /// <summary>
        /// 处理中
        /// </summary>
        public static int transactionstatus_processing = 464;

        /// <summary>
        /// 已划转终止
        /// </summary>
        public static int RaiseStopType_HavedTran = 454;

        /// <summary>
        /// 项目领投状态
        /// </summary>
        public static int RaiseProLedState = 516;

        /// <summary>
        /// 已保存
        /// </summary>
        public static int RaiseProLedState_Save = 517;

        /// <summary>
        /// 申请中
        /// </summary>
        public static int RaiseProLedState_Apply = 518;

        /// <summary>
        /// 已确认
        /// </summary>
        public static int RaiseProLedState_Confirm = 519;

        /// <summary>
        /// 已支付
        /// </summary>
        public static int RaiseProLedState_Pay = 520;

        /// <summary>
        /// 已取消
        /// </summary>
        public static int RaiseProLedState_Cancel = 521;

        /// <summary>
        /// 保证金支付状态
        /// </summary>
        public static int RaiseBondState = 522;

        /// <summary>
        /// 已支付
        /// </summary>
        public static int RaiseBondState_Pay = 523;

        /// <summary>
        /// 未支付
        /// </summary>
        public static int RaiseBondState_NotPay = 524;

        /// <summary>
        /// 已失败
        /// </summary>
        public static int RaiseProLedState_Failure = 525;

        /// <summary>
        /// 领投操作类型
        /// </summary>
        public static int RaiseLedOperType = 526;

        /// <summary>
        /// 新增
        /// </summary>
        public static int RaiseLedOperType_Add = 527;

        /// <summary>
        /// 支付保证金
        /// </summary>
        public static int RaiseLedOperType_PayBond = 528;

        /// <summary>
        /// 领投申请失败
        /// </summary>
        public static int RaiseLedOperType_LeaderFail = 529;

        /// <summary>
        /// 确认领投人
        /// </summary>
        public static int RaiseLedOperType_Confirm = 530;

        /// <summary>
        /// 撤回领投申请
        /// </summary>
        public static int RaiseLedOperType_Back = 531;

        /// <summary>
        /// 支付金额
        /// </summary>
        public static int RaiseLedOperType_PayLeadMoney = 532;

        /// <summary>
        /// 未支付金额
        /// </summary>
        public static int RaiseLedOperType_NotPayMoney = 533;

        /// <summary>
        /// 取消领投申请
        /// </summary>
        public static int RaiseLedOperType_Cancel = 534;

        /// <summary>
        /// 删除领投申请
        /// </summary>
        public static int RaiseLedOperType_Delete = 535;

        /// <summary>
        /// 领投人资格的条件
        /// </summary>
        public static int RaiseLed = 506;

        /// <summary>
        /// 符合机构投资的条件
        /// </summary>
        public static int InvestComCase = 501;

        /// <summary>
        /// 资产利润表近报
        /// </summary>
        public static int Raise_AnnexType_NearIncomeStatement = 491;

        /// <summary>
        /// 资产利润表年报
        /// </summary>
        public static int Raise_AnnexType_YearIncomeStatement = 492;

        /// <summary>
        /// 现金流量表近报
        /// </summary>
        public static int Raise_AnnexType_NearCashFlow = 493;

        /// <summary>
        /// 现金流量表年报
        /// </summary>
        public static int Raise_AnnexType_YearCashFlow = 494;

        /// <summary>
        /// 资产负债表近报
        /// </summary>
        public static int Raise_AnnexType_NearBalSheet = 495;

        /// <summary>
        /// 资产负债表年报
        /// </summary>
        public static int Raise_AnnexType_YearBalSheet = 496;

        /// <summary>
        /// 公司荣誉
        /// </summary>
        public static int Raise_AnnexType_ComHonor = 334;

        /// <summary>
        /// 满标解冻
        /// </summary>
        public static int RaiseOperType_ProjectThaw = 426;

        /// <summary>
        /// 资金划转
        /// </summary>
        public static int RaiseOperType_FundsTransfer = 420;

        /// <summary>
        /// 项目结清
        /// </summary>
        public static int RaiseOperType_ProjectSettle = 423;

        /// <summary>
        /// 审核成功
        /// </summary>
        public static int RaiseState_AuditSuccess = 407;

        /// <summary>
        /// 等额本息
        /// </summary>
        public static int ProPayType_InterestCapital = 382;

        /// <summary>
        /// 项目删除
        /// </summary>
        public static int RaiseOperType_Delete = 383;

        /// <summary>
        /// 项目失败
        /// </summary>
        public static int RaiseOperType_ProFailure = 384;

        /// <summary>
        /// 撤回审核申请
        /// </summary>
        public static int RaiseOperType_CacnelAuditApply = 385;

        /// <summary>
        /// 项目上线
        /// </summary>
        public static int RaiseOperType_ProRelease = 386;

        /// <summary>
        /// 项目暂停
        /// </summary>
        public static int RaiseOperType_ProStop = 387;

        /// <summary>
        /// 项目终止
        /// </summary>
        public static int RaiseOperType_BackFinance = 388;

        /// <summary>
        /// 项目预热
        /// </summary>
        public static int RaiseOperType_ProPreheating = 389;

        /// <summary>
        /// 满标审核
        /// </summary>
        public static int RaiseOperType_FullAudit = 392;

        /// <summary>
        /// 项目投资-划转金额
        /// </summary>
        public static int transactiontype_InvestTransfer = 369;

        /// <summary>
        /// 风控操作
        /// </summary>
        public static int RaiseOperType_Control = 348;

        /// <summary>
        /// 文档
        /// </summary>
        public static int Raise_AnnexMediaType_Doc = 349;

        /// <summary>
        /// 个人诉讼报告
        /// </summary>
        public static int Raise_AnnexType_PerSReport = 350;

        /// <summary>
        /// 三级县
        /// </summary>
        public static int AreaType_Coun = 301;

        /// <summary>
        /// 机构注册
        /// </summary>
        public static int RaiseUserSource_Mechanism = 305;

        /// <summary>
        /// 回报状态
        /// </summary>
        public static int RaiseReturnState = 225;

        /// <summary>
        /// 确认回报
        /// </summary>
        public static int RewardReturnLogType_Return = 232;

        /// <summary>
        /// 已盈利
        /// </summary>
        public static int RewardStage_Profit = 237;

        /// <summary>
        /// 运营
        /// </summary>
        public static int EquityStage_Operate = 251;

        /// <summary>
        /// 已成功
        /// </summary>
        public static int LoanInvestmenState_Success = 278;

        /// <summary>
        /// 扣除积分
        /// </summary>
        public static int scoreusetype_getDeduc = 286;

        /// <summary>
        /// 已撤回
        /// </summary>
        public static int LoadTransferState_Withdraw = 292;

        /// <summary>
        /// 审核成功
        /// </summary>
        public static int ProjectAuditState_Success = 180;

        /// <summary>
        /// 审核失败
        /// </summary>
        public static int AuditState_failure = 175;

        /// <summary>
        /// 审核状态
        /// </summary>
        public static int AuditState = 170;

        /// <summary>
        /// 公益
        /// </summary>
        public static int ModelType_Donate = 166;

        /// <summary>
        /// 已否决
        /// </summary>
        public static int auditstatus_HasVeto = 126;

        /// <summary>
        /// 信贷审核
        /// </summary>
        public static int auditlink_Creditaudit = 120;

        /// <summary>
        /// 已流标
        /// </summary>
        public static int transferstatus_HasFlowStandard = 132;

        /// <summary>
        /// 认证已通过
        /// </summary>
        public static int realname_Through = 143;

        /// <summary>
        /// 平台还款
        /// </summary>
        public static int RepaymentType_PlatformRepay = 155;

        /// <summary>
        /// 500人以上
        /// </summary>
        public static int enterprisesize_FiveHundredAbove = 42;

        /// <summary>
        /// 收费类型
        /// </summary>
        public static int feetype = 58;

        /// <summary>
        /// 已发送担保
        /// </summary>
        public static int sendstate_12103 = 103;

        /// <summary>
        /// 户口簿
        /// </summary>
        public static int documenttype_ResidenceBooklet = 46;

        /// <summary>
        /// 平台代还
        /// </summary>
        public static int repaymenstate_Replace = 29;

        /// <summary>
        /// 项目类型
        /// </summary>
        public static int projecttype = 8;

        /// <summary>
        /// 借款机构
        /// </summary>
        public static int projecttype_Lender = 11;

        /// <summary>
        /// 待担保审核
        /// </summary>
        public static int projectstate_StayGuaranteeaudit = 16;

        /// <summary>
        /// 已退回
        /// </summary>
        public static int projectstate_HasReturn = 17;

        /// <summary>
        /// 平台
        /// </summary>
        public static int projecttype_Platform = 12;

        /// <summary>
        /// 项目状态
        /// </summary>
        public static int projectstate = 13;

        /// <summary>
        /// 护照
        /// </summary>
        public static int documenttype_Passport = 47;

        /// <summary>
        /// 平台代还
        /// </summary>
        public static int RepaymentType_PlatformDaihuan = 156;

        /// <summary>
        /// 担保审核
        /// </summary>
        public static int auditlink_Guaranteaudit = 121;

        /// <summary>
        /// 项目审核状态
        /// </summary>
        public static int ProjectAuditState = 176;

        /// <summary>
        /// 审核失败
        /// </summary>
        public static int ProjectAuditState_failure = 181;

        /// <summary>
        /// 已转让
        /// </summary>
        public static int LoanInvestmenState_Transfer = 279;

        /// <summary>
        /// 盈利
        /// </summary>
        public static int EquityStage_Profit = 252;

        /// <summary>
        /// 订单失败
        /// </summary>
        public static int RewardReturnLogType_Failure = 238;

        /// <summary>
        /// 退款
        /// </summary>
        public static int RewardReturnLogType_Refund = 233;

        /// <summary>
        /// 已回报
        /// </summary>
        public static int RaiseReturnState_Return = 226;

        /// <summary>
        /// 个人收入证明
        /// </summary>
        public static int Raise_AnnexType_PerIncomeReport = 351;

        /// <summary>
        /// 投资者
        /// </summary>
        public static int Raise_AnnexCan_Investor = 339;

        /// <summary>
        /// 债权认购-冻结金额
        /// </summary>
        public static int transactiontype_SubscriptionFrazon = 370;

        /// <summary>
        /// 预热中
        /// </summary>
        public static int RaiseState_Preheating = 408;

        /// <summary>
        /// 审核
        /// </summary>
        public static int transactionstatus_check = 465;

        /// <summary>
        /// 信贷审核
        /// </summary>
        public static int LoanStepName_Credit = 481;

        /// <summary>
        /// 订单取消
        /// </summary>
        public static int RewardReturnLogType_OrderCancel = 439;

        /// <summary>
        /// 平台地图
        /// </summary>
        public static int NewsTemplate_PlatformMap = 569;

        /// <summary>
        /// 已中奖
        /// </summary>
        public static int RaiseReturnState_Win = 602;

        /// <summary>
        /// 未中奖
        /// </summary>
        public static int RaiseReturnState_NoWin = 603;

        /// <summary>
        /// 已发布
        /// </summary>
        public static int auditstatus_Publish = 550;

        /// <summary>
        /// 待回报
        /// </summary>
        public static int RaiseReturnState_Not = 441;

        /// <summary>
        /// 已收货
        /// </summary>
        public static int OrderStatus_FinancialTransType_Received = 717;

        /// <summary>
        /// 谢谢参与
        /// </summary>
        public static int PrizeLevel_Thanks = 694;

        /// <summary>
        /// 已支付
        /// </summary>
        public static int SettlementStatus_Paid = 640;

        /// <summary>
        /// 日均投资余额
        /// </summary>
        public static int deductType_leftMoneyPerdays = 614;

        /// <summary>
        /// 投资金占用天数结算
        /// </summary>
        public static int payType_investDays = 622;

        /// <summary>
        /// 投资一次结算
        /// </summary>
        public static int payType_investOne = 623;

        /// <summary>
        /// 充值金额
        /// </summary>
        public static int deductType_recharge = 615;

        /// <summary>
        /// 被退回
        /// </summary>
        public static int SettlementStatus_Returned = 641;

        /// <summary>
        /// 友情链接类型
        /// </summary>
        public static int ScoreLinkType = 718;

        /// <summary>
        /// 导航列表
        /// </summary>
        public static int NewsTemplate_Navigation = 570;

        /// <summary>
        /// 满标审核
        /// </summary>
        public static int LoanStepName_FullScale = 482;

        /// <summary>
        /// 众筹中
        /// </summary>
        public static int RaiseState_Ing = 409;

        /// <summary>
        /// 债权认购-解冻金额
        /// </summary>
        public static int transactiontype_SubscriptionThaw = 371;

        /// <summary>
        /// 个人征信报告
        /// </summary>
        public static int Raise_AnnexType_PerCreditReport = 353;

        /// <summary>
        /// 已退款
        /// </summary>
        public static int RaiseOrderState_Refund = 227;

        /// <summary>
        /// 已撤回
        /// </summary>
        public static int LoanInvestmenState_Withdrawn = 280;

        /// <summary>
        /// 话题类型
        /// </summary>
        public static int CommentType = 183;

        /// <summary>
        /// 满标审核
        /// </summary>
        public static int auditlink_Fullstandardaudit = 122;

        /// <summary>
        /// 担保、小贷公司还款
        /// </summary>
        public static int RepaymentType_GuarRepay = 157;

        /// <summary>
        /// 军官证
        /// </summary>
        public static int documenttype_GasCardin = 48;

        /// <summary>
        /// 还款状态
        /// </summary>
        public static int repaymenstate = 26;

        /// <summary>
        /// 已否决
        /// </summary>
        public static int projectstate_HasVeto = 18;

        /// <summary>
        /// 待发布
        /// </summary>
        public static int projectstate_StayRelease = 19;

        /// <summary>
        /// 结算方式
        /// </summary>
        public static int settlementway = 30;

        /// <summary>
        /// 士兵证
        /// </summary>
        public static int documenttype_SoldierCertificate = 49;

        /// <summary>
        /// 担保、小贷公司还代还
        /// </summary>
        public static int RepaymentType_GuarRepayDaihuan = 158;

        /// <summary>
        /// 项目发布
        /// </summary>
        public static int auditlink_Projectrelease = 151;

        /// <summary>
        /// 操作日志操作类型
        /// </summary>
        public static int RaiseOperType = 188;

        /// <summary>
        /// 已失败
        /// </summary>
        public static int RaiseOrderState_Failed = 228;

        /// <summary>
        /// 企业诉讼报告
        /// </summary>
        public static int Raise_AnnexType_ComSReport = 354;

        /// <summary>
        /// 债权认购-划转金额
        /// </summary>
        public static int transactiontype_SubscriptionTransfer = 372;

        /// <summary>
        /// 项目发布
        /// </summary>
        public static int LoanStepName_Release = 483;

        /// <summary>
        /// 已取消
        /// </summary>
        public static int RaiseOrderState_Cancel = 466;

        /// <summary>
        /// 满标申请
        /// </summary>
        public static int RaiseState_FullApply = 453;

        /// <summary>
        /// 借款总额
        /// </summary>
        public static int deductType_borrowSum = 616;

        /// <summary>
        /// 图片列表
        /// </summary>
        public static int NewsTemplate_PictureList = 630;

        /// <summary>
        /// 投资赎回后一次结算
        /// </summary>
        public static int payType_ransom = 624;

        /// <summary>
        /// 一次结算
        /// </summary>
        public static int payType_payOnetime = 625;

        /// <summary>
        /// 上下图文列表
        /// </summary>
        public static int NewsTemplate_UDGraphic = 631;

        /// <summary>
        /// 日均借款余额
        /// </summary>
        public static int deductType_borrowPerdays = 617;

        /// <summary>
        /// 满标划转
        /// </summary>
        public static int LoanStepName_CreditTransferred = 484;

        /// <summary>
        /// 借款维护
        /// </summary>
        public static int auditlink_LoanManage = 542;

        /// <summary>
        /// 借款还款
        /// </summary>
        public static int transactiontype_LoanRepayment = 373;

        /// <summary>
        /// 公司财务报告
        /// </summary>
        public static int Raise_AnnexType_ComIncomeReport = 355;

        /// <summary>
        /// 已划款
        /// </summary>
        public static int RaiseState_HasDrawn = 411;

        /// <summary>
        /// 交易记录类型
        /// </summary>
        public static int RecordType = 191;

        /// <summary>
        /// 强制还款
        /// </summary>
        public static int RepaymentType_Compulsoryrepay = 159;

        /// <summary>
        /// 港澳居民来往内地通行证
        /// </summary>
        public static int documenttype_Macau = 50;

        /// <summary>
        /// 招标中
        /// </summary>
        public static int projectstate_Tender = 20;

        /// <summary>
        /// 满标待审
        /// </summary>
        public static int projectstate_FullScalePending = 21;

        /// <summary>
        /// 台湾同胞来往内地通行证
        /// </summary>
        public static int documenttype_Taiwan = 51;

        /// <summary>
        /// 强制还平台代还
        /// </summary>
        public static int RepaymentType_CompulsoryrepayDaihuan = 160;

        /// <summary>
        /// 交易收益类型
        /// </summary>
        public static int ReturnType = 195;

        /// <summary>
        /// 企业征信报告
        /// </summary>
        public static int Raise_AnnexType_ComCreditReport = 356;

        /// <summary>
        /// 投资收款
        /// </summary>
        public static int transactiontype_InvestmentCollection = 374;

        /// <summary>
        /// 转让初审
        /// </summary>
        public static int LoanStepName_Assignment = 485;

        /// <summary>
        /// 转让满标审核
        /// </summary>
        public static int LoanStepName_AssignmentFullScale = 486;

        /// <summary>
        /// 项目流标-解冻金额
        /// </summary>
        public static int transactiontype_FlowThaw = 375;

        /// <summary>
        /// 符合个人投资者的条件
        /// </summary>
        public static int InvestorPeCase = 199;

        /// <summary>
        /// 投资人类型
        /// </summary>
        public static int InvestorType = 200;

        /// <summary>
        /// 外国人居留证
        /// </summary>
        public static int documenttype_Foreigner = 52;

        /// <summary>
        /// 已流标
        /// </summary>
        public static int projectstate_FlowStandard = 22;

        /// <summary>
        /// 待划转
        /// </summary>
        public static int projectstate_StayTransfer = 23;

        /// <summary>
        /// 警官证
        /// </summary>
        public static int documenttype_Police = 53;

        /// <summary>
        /// 订单状态
        /// </summary>
        public static int RaiseOrderState = 222;

        /// <summary>
        /// 回报类型
        /// </summary>
        public static int RewardReturnType = 219;

        /// <summary>
        /// 项目流标
        /// </summary>
        public static int transactiontype_ProjectFlow = 376;

        /// <summary>
        /// 已结清
        /// </summary>
        public static int RaiseState_HasSettle = 414;

        /// <summary>
        /// 众筹项目状态
        /// </summary>
        public static int RaiseState = 404;

        /// <summary>
        /// 项目支持操作日志表
        /// </summary>
        public static int RewardReturnLogType = 229;

        /// <summary>
        /// 转让满标划转
        /// </summary>
        public static int LoanStepName_AssignmentFullScaleTransferred = 487;

        /// <summary>
        /// 回报方式
        /// </summary>
        public static int RewardReturnWay = 577;

        /// <summary>
        /// 创业性质
        /// </summary>
        public static int EntreprenProp = 581;

        /// <summary>
        /// 公司性质
        /// </summary>
        public static int CompanyProp = 585;

        /// <summary>
        /// 尽职调查上传来源
        /// </summary>
        public static int RaiseDueUploadSource = 599;

        /// <summary>
        /// 投资经历回报类型
        /// </summary>
        public static int InvExpReturnType = 589;

        /// <summary>
        /// 投资经历投资角色
        /// </summary>
        public static int InvExpRole = 595;

        /// <summary>
        /// 文字列表(微信)
        /// </summary>
        public static int NewsTemplate_WXText = 670;

        /// <summary>
        /// 产品众筹项目阶段
        /// </summary>
        public static int RewardStage = 234;

        /// <summary>
        /// 审核失败
        /// </summary>
        public static int RaiseState_ExamineFailure = 415;

        /// <summary>
        /// 费用退回
        /// </summary>
        public static int transactiontype_CostReturn = 377;

        /// <summary>
        /// 其他证件
        /// </summary>
        public static int documenttype_Other = 54;

        /// <summary>
        /// 投资方式
        /// </summary>
        public static int investmentway = 55;

        /// <summary>
        /// 还款中
        /// </summary>
        public static int projectstate_Repayment = 24;

        /// <summary>
        /// 已结清
        /// </summary>
        public static int projectstate_Settled = 25;

        /// <summary>
        /// 转让审核环节
        /// </summary>
        public static int transferstatuslink = 114;

        /// <summary>
        /// 项目投资-红包或投资券
        /// </summary>
        public static int transactiontype_InvestRed = 378;

        /// <summary>
        /// 项目成功
        /// </summary>
        public static int RaiseState_Success = 416;

        /// <summary>
        /// 公告列表(微信)
        /// </summary>
        public static int NewsTemplate_WXNotice = 671;

        /// <summary>
        /// 问题列表(微信)
        /// </summary>
        public static int NewsTemplate_WXQuestion = 672;

        /// <summary>
        /// 充值
        /// </summary>
        public static int transactiontype_Recharge = 459;

        /// <summary>
        /// 项目暂停
        /// </summary>
        public static int RaiseState_Pause = 417;

        /// <summary>
        /// 股权众筹项目阶段
        /// </summary>
        public static int EquityStage = 248;

        /// <summary>
        /// 审核环节
        /// </summary>
        public static int auditlink = 117;

        /// <summary>
        /// 待小贷审核
        /// </summary>
        public static int projectstate_StaySmallloanAudit = 127;

        /// <summary>
        /// 审核状态
        /// </summary>
        public static int auditstatus = 123;

        /// <summary>
        /// 小贷已审核待发送
        /// </summary>
        public static int projectstate_SmallloanAudit = 133;

        /// <summary>
        /// 债券众筹还款方式
        /// </summary>
        public static int LoanRepaymentType = 271;

        /// <summary>
        /// 项目终止
        /// </summary>
        public static int RaiseState_Stop = 418;

        /// <summary>
        /// 提现
        /// </summary>
        public static int transactiontype_Withdrawals = 460;

        /// <summary>
        /// 服务费
        /// </summary>
        public static int transactiontype_ServiceCharge = 541;

        /// <summary>
        /// 待线下审核
        /// </summary>
        public static int projectstate_P2PStay = 673;

        /// <summary>
        /// 项目失败
        /// </summary>
        public static int RaiseState_ProjectFailure = 419;

        /// <summary>
        /// 债权众筹投资状态
        /// </summary>
        public static int LoanInvestmenState = 275;

        /// <summary>
        /// 机构类型
        /// </summary>
        public static int govtype = 128;

        /// <summary>
        /// 小贷机构
        /// </summary>
        public static int govtype_Smallloan = 129;

        /// <summary>
        /// 担保机构
        /// </summary>
        public static int govtype_Guarantee = 130;

        /// <summary>
        /// 理财加盟商
        /// </summary>
        public static int govtype_FinancialFranchisee = 131;

        /// <summary>
        /// 债权众筹还款状态
        /// </summary>
        public static int LoanRepaymentState = 284;

        /// <summary>
        /// 线下审核完成
        /// </summary>
        public static int projectstate_P2PComplete = 674;

        /// <summary>
        /// 代发工资-划转金额
        /// </summary>
        public static int transactiontype_PayrollTransfer = 685;

        /// <summary>
        /// 线下拒贷
        /// </summary>
        public static int projectstate_P2PRefuse = 675;

        /// <summary>
        /// 地区类型
        /// </summary>
        public static int AreaType = 298;

        /// <summary>
        /// 债权众筹转让申请状态
        /// </summary>
        public static int LoadTransferState = 289;

        /// <summary>
        /// 已审核待发送
        /// </summary>
        public static int projectstate_AuditTobesent = 139;

        /// <summary>
        /// 实名认证状态
        /// </summary>
        public static int realname = 140;

        /// <summary>
        /// 影像资料类型
        /// </summary>
        public static int filmsdatatype = 144;

        /// <summary>
        /// 已发布
        /// </summary>
        public static int projectstate_HasIssued = 146;

        /// <summary>
        /// 会员来源
        /// </summary>
        public static int RaiseUserSource = 302;

        /// <summary>
        /// 广告位投放位置
        /// </summary>
        public static int AdPlacement = 293;

        /// <summary>
        /// 客户放弃
        /// </summary>
        public static int projectstate_P2PGiveUp = 676;

        /// <summary>
        /// 已发送平台
        /// </summary>
        public static int projectstate_SentPlatform = 538;

        /// <summary>
        /// 广告类型
        /// </summary>
        public static int AdType = 296;

        /// <summary>
        /// 会员类型
        /// </summary>
        public static int RaiseUserType = 306;

        /// <summary>
        /// 计息方式
        /// </summary>
        public static int bearingway = 147;

        /// <summary>
        /// 还款类型
        /// </summary>
        public static int RepaymentType = 152;

        /// <summary>
        /// 会员状态
        /// </summary>
        public static int RaiseUserState = 309;

        /// <summary>
        /// 已发送担保
        /// </summary>
        public static int projectstate_AlreadySendSecurity = 539;

        /// <summary>
        /// 已发送小贷
        /// </summary>
        public static int projectstate_SentSmallLoan = 540;

        /// <summary>
        /// 积分类型
        /// </summary>
        public static int RaiseIntegralType = 312;

        /// <summary>
        /// 留言状态
        /// </summary>
        public static int InsideBox = 317;

        /// <summary>
        /// 正常
        /// </summary>
        public static int RecordState_Normal = 324;

        /// <summary>
        /// 已恢复
        /// </summary>
        public static int InsideBox_Reply = 319;

        /// <summary>
        /// 积分来源
        /// </summary>
        public static int RaiseIntegralSource = 316;

        /// <summary>
        /// 投资人类型
        /// </summary>
        public static int Raise_InvestorType = 320;

        /// <summary>
        /// 交易记录状态
        /// </summary>
        public static int RecordState = 323;

        /// <summary>
        /// 附件媒体类型
        /// </summary>
        public static int Raise_AnnexMediaType = 326;

        /// <summary>
        /// 个人附件类型
        /// </summary>
        public static int Raise_PersonAnnexType = 330;

        /// <summary>
        /// 机构附件类型
        /// </summary>
        public static int Raise_ComAnnexType = 331;

        /// <summary>
        /// 可见级别
        /// </summary>
        public static int Raise_AnnexCan = 335;

        /// <summary>
        /// 担保方式
        /// </summary>
        public static int Raise_info_GuaranteeModel = 340;

        /// <summary>
        /// 担保物类型
        /// </summary>
        public static int Raise_ProGuaranty = 344;

        /// <summary>
        /// 资金划转类型
        /// </summary>
        public static int RaiseCapTran = 424;

        /// <summary>
        /// 展期类型
        /// </summary>
        public static int RaiseFullRollType = 393;

        /// <summary>
        /// 未筹满展期
        /// </summary>
        public static int RaiseFullRollType_NotFull = 394;

        /// <summary>
        /// 众筹项目发起来源
        /// </summary>
        public static int Raise_ProSource = 357;

        /// <summary>
        /// 资金划转状态
        /// </summary>
        public static int RaiseFundsTransferState = 431;

        /// <summary>
        /// 满标方式
        /// </summary>
        public static int RaiseFullType = 434;

        /// <summary>
        /// 已发货
        /// </summary>
        public static int RaiseReturnState_Delivered = 440;

        /// <summary>
        /// 还款方式
        /// </summary>
        public static int ProPayType = 379;

        /// <summary>
        /// 实名认证状态
        /// </summary>
        public static int RealProveStatus = 399;

        /// <summary>
        /// 审核未通过
        /// </summary>
        public static int RealProveStatus_AuditNotPass = 402;

        /// <summary>
        /// 用户资金交易状态
        /// </summary>
        public static int transactionstatus = 461;

        /// <summary>
        /// 我的金融股资产超过100万元
        /// </summary>
        public static int InvestorPeCase_MoreThanMillion = 497;

        /// <summary>
        /// 身份类型
        /// </summary>
        public static int RaiseIdentityType = 475;

        /// <summary>
        /// 我们的金融股资产超过1亿
        /// </summary>
        public static int InvestComCase_MoreThanMillion = 502;

        /// <summary>
        /// 两年以上奥拓思维理财经理级以上岗位从业经验
        /// </summary>
        public static int RaiseLed_TwoYears = 507;

        /// <summary>
        /// 环节名称
        /// </summary>
        public static int LoanStepName = 478;

        /// <summary>
        /// 实名认证通过
        /// </summary>
        public static int RealProveStatus_Pass = 403;

        /// <summary>
        /// 新闻模版
        /// </summary>
        public static int NewsTemplate = 565;

        /// <summary>
        /// 小贴士类型
        /// </summary>
        public static int QaType = 571;

        /// <summary>
        /// 提成规则类型
        /// </summary>
        public static int deductRuleType = 605;

        /// <summary>
        /// 提成类型
        /// </summary>
        public static int deductType = 609;

        /// <summary>
        /// 银行卡绑定数
        /// </summary>
        public static int deductType_bankcard = 612;

        /// <summary>
        /// 支付方式
        /// </summary>
        public static int payType = 618;

        /// <summary>
        /// 计费方式
        /// </summary>
        public static int counterFee = 626;

        /// <summary>
        /// 消息通知发送类型
        /// </summary>
        public static int RaiseMsgType = 632;

        /// <summary>
        /// 加盟商提成结算状态
        /// </summary>
        public static int SettlementStatus = 636;

        /// <summary>
        /// 奖品类别
        /// </summary>
        public static int PrizeType = 686;

        /// <summary>
        /// 奖项等级
        /// </summary>
        public static int PrizeLevel = 690;

        /// <summary>
        /// 奖项状态
        /// </summary>
        public static int PrizeState = 695;

        /// <summary>
        /// 抽奖扣除积分
        /// </summary>
        public static int scoreusetype_Lotery = 702;

        /// <summary>
        /// 理财产品起息方式
        /// </summary>
        public static int InterestType = 703;

        /// <summary>
        /// 理财产品类型
        /// </summary>
        public static int FinancialType = 706;

        /// <summary>
        /// 理财产品交易类型
        /// </summary>
        public static int FinancialTransType = 709;

        /// <summary>
        /// 订单状态
        /// </summary>
        public static int OrderStatus = 713;

        /// <summary>
        /// 图片
        /// </summary>
        public static int ScoreLinkTypePic = 719;

        /// <summary>
        /// 文字
        /// </summary>
        public static int ScoreLinkTypeDoc = 720;

        /// <summary>
        /// 虚拟充值
        /// </summary>
        public static int transactiontype_VirtualRecharge = 721;

        /// <summary>
        /// 虚拟提现
        /// </summary>
        public static int transactiontype_VirtualWithdrawals = 722;

        /// <summary>
        /// 债权匹配 文件
        /// </summary>
        public static int match_Project = 723;

        /// <summary>
        /// 已逾期
        /// </summary>
        public static int projectstate_Overdue = 734;

        /// <summary>
        /// 恒丰标录入审核中
        /// </summary>
        public static int projectstate_hfentry = 736;

        /// <summary>
        /// 恒丰通过
        /// </summary>
        public static int projectstate_hfpass = 737;

        /// <summary>
        /// 恒丰拒绝
        /// </summary>
        public static int projectstate_hfrefuse = 738;

        /// <summary>
        /// 恒丰待上传证照 
        /// </summary>
        public static int projectstate_hfwaitphoto = 739;

        /// <summary>
        /// 恒丰待审核 
        /// </summary>
        public static int projectstate_hfwait = 740;
        /// <summary>
        /// 恒丰待审核证照
        /// </summary>
        public static int projectstate_hfwaitpassphoto = 741;

        /// <summary>
        /// 状态异常
        /// </summary>
        public static int projectstate_hfboom = 742;

        /// <summary>
        /// 接入恒丰接口失败
        /// </summary>
        public static int projectstate_hffail = 743;

        /// <summary>
        /// 融资转账
        /// </summary>
        public static int transactiontype_FinanceTransfer = 793;

        /// <summary>
        /// 银行正在处理中
        /// </summary>
        public static int bank_state_using = 794;

        /// <summary>
        /// 投资金额已解冻
        /// </summary>
        public static int bank_state_unfreeze = 795;

        /// <summary>
        /// 投资金额已解冻，但流标失败
        /// </summary>
        public static int bank_state_cancel_failed = 796;

        /// <summary>
        /// 还款
        /// </summary>
        public static int transactiontype_repayment = 797;
    }
}
