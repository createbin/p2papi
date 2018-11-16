using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.PRO;
using ZFCTAPI.Services.Repositorys;

namespace ZFCTAPI.Services.Transfer
{
    public interface IProLoanFlowService : IRepository<PRO_loan_flow>
    {
        /// <summary>
        /// 项目跟踪插入公共方法
        /// </summary>
        /// <param name="pro_loan_id">贷款信息ID（不是贷款信息的跟踪记录输入“0”）</param>
        /// <param name="pro_transfer_id">转让申请ID（不是转让申请的跟踪记录输入“0”）</param>
        /// <param name="pro_loan_state">项目状态</param>
        /// <param name="pro_step_name">环节名称</param>
        /// <param name="pro_flow_mark">备注</param>
        /// <param name="pro_operate_emp">添加人</param>
        void FlowAdd(int pro_loan_id, int pro_transfer_id, int pro_loan_state, int pro_step_name, string pro_flow_mark, int pro_operate_emp = 0);
    }

    public class ProLoanFlowService : Repository<PRO_loan_flow>, IProLoanFlowService
    {
        public void FlowAdd(int pro_loan_id, int pro_transfer_id, int pro_loan_state, int pro_step_name, string pro_flow_mark, int pro_operate_emp = 0)
        {
            PRO_loan_flow loanFlow = new PRO_loan_flow();
            if (pro_loan_id != 0)
            {
                loanFlow.pro_loan_id = pro_loan_id;
            }
            if (pro_transfer_id != 0)
            {
                loanFlow.pro_transfer_id = pro_transfer_id;
            }
            loanFlow.pro_flow_mark = pro_flow_mark;
            loanFlow.pro_step_id = pro_step_name;
            loanFlow.pro_loan_stateId = pro_loan_state;
            if (pro_operate_emp != 0)
            {
                loanFlow.pro_operate_emp = pro_operate_emp;
            }
            loanFlow.pro_operate_date = DateTime.Now;
            this.Add(loanFlow);
        }
    }
}