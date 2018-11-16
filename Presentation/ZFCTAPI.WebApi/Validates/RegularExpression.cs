using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZFCTAPI.WebApi.Validates
{
    public class RegularExpression
    {
        public const string CHS = "^[\u4e00-\u9fa5]+$";
        public const string CHS_Msg = "输入必须为汉字";


        public const string Bankcard = @"^(\d{16}|\d{19})$";
        public const string Bankcard_Msg = "无效的银行卡号";
        /// <summary>
        /// 中文字符
        /// </summary>
        public const string China = "^[\u4E00-\u9FA5]{5,100}$";
        public const string CHSS_Msg = "请输入5~100个汉字";

        /// <summary>
        /// 手机号码
        /// </summary>
        public const string Mobile = @"^[1]+\d{10}$";
        public const string Mobile_Msg = "无效的手机号码";

        /// <summary>
        /// 电话号码
        /// </summary>
        public const string Phone = @"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$";
        public const string Phone_Msg = "无效的电话号码";

        /// <summary>
        /// 金额
        /// </summary>
        public const string Money = @"^(([0-9]|([1-9][0-9]{0,9}))((\.[0-9]{1,2})?))$";
        public const string Money_Msg = "请输入有效的金额";

        /// <summary>
        /// QQ
        /// </summary>
        public const string QQ = @"^[1-9]\d{4,10}$i";
        public const string QQ_Msg = @"^[1-9]\d{4,10}$/i";

        /// <summary>
        /// 时间格式
        /// </summary>
        public const string DateTime = @"^[0-9]{4}[-][0-9]{2}[-][0-9]{2}$/i";
        public const string DateTime_Msg = "时间格式不正确";

        /// <summary>
        /// 年份
        /// </summary>
        public const string Year = @"19[\d][\d]|20[\d][\d]";
        public const string Year_Msg = "年份格式不正确";

        /// <summary>
        /// Email
        /// </summary>
        public const string Email = @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?";
        public const string Email_Msg = "电子邮箱格式不正确";

        public const string gov_no = @"[A-Za-z0-9]{2}[0-9]{6}[A-Za-z0-9]{10}|[a-zA-Z0-9]{8}-[a-zA-Z0-9]";
        public const string gov_no_Msg = "组织机构代码格式不正确！";

        public const string UserNameOrPassword = @"[a-zA-Z0-9]\d{6,20}";
        public const string UserNameOrPassword_Msg = "用户名和密码必须为6~20位以上的数字或字母！";

        /// <summary>
        /// p2p 3.0 用户名，密码
        /// </summary>
        public const string UserNameRegu = @"^[A-Za-z0-9]{5,15}$";
        public const string UserNameRegu_Msg = "用户名必须为5-15字母或数字";
        public const string PassWordRegu = @"^[A-Za-z0-9]{6,16}$";
        public const string PassWordRegu_Msg = "密码必须为6-16字母或数字";

        public const string UserName = @"^[a-zA-Z0-9]{4,15}$";
        public const string UserName_Msg = "用户名必须为4~15位以上的数字或字母！";

        public const string Password = @"^[a-zA-Z0-9]{6,25}$";
        public const string Password_Msg = "密码必须为6~25位以上的数字或字母！";

        public const string isIDCard1 = @"/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/";
        public const string isIDCard2 = @"/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/";
        public const string isIDCard_Msg = "身份证格式不正确！";

        /// <summary>
        /// 网址
        /// </summary>
        public const string URL = @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/";
        public const string URL_Msg = "输入必须为网址形式";
    }
}
