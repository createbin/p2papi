using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;
using ZFCTAPI.Data.Customers;

namespace ZFCTAPI.Data.PRO
{
    public partial class P2PInfo
    {
        private Customer _customer;

        private CST_user_info _userInfo;
        private PRO_loan_info _loanInfo;
        private PRO_loan_plan _loanPlan;
        private PRO_invester_plan _investPlan;

        /// <summary>
        /// 登录用户信息  必填项
        /// </summary>
        public Customer CustomerInfo
        {
            set { _customer = value; }
            get { return _customer ?? new Customer(); }
        }

        /// <summary>
        /// P2P用户信息  必填项
        /// </summary>
        public CST_user_info UserInfo
        {
            set { _userInfo = value; }
            get { return _userInfo ?? new CST_user_info(); }
        }

        /// <summary>
        /// 贷款项目信息
        /// </summary>
        public PRO_loan_info LoanInfo
        {
            set { _loanInfo = value; }
            get { return _loanInfo ?? new PRO_loan_info(); }
        }

        /// <summary>
        /// 还款计划
        /// </summary>
        public PRO_loan_plan LoanPlan
        {
            set { _loanPlan = value; }
            get { return _loanPlan ?? new PRO_loan_plan(); }
        }

        public PRO_invester_plan InvestPlan
        {
            set { _investPlan = value; }
            get { return _investPlan ?? new PRO_invester_plan(); }
        }
    }
}