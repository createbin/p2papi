using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Services.Transaction
{
    public interface IFeeService
    {
        /// <summary>
        /// 计算提现手续费
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        decimal CalcWithdrawFee(int userId,decimal money,out bool isFrist);
    }

    public class FeeService : IFeeService
    {
        private readonly ICstTransactionService _cstTransactionService;
        public FeeService(ICstTransactionService cstTransactionService)
        {
            _cstTransactionService = cstTransactionService;
        }

        public decimal CalcWithdrawFee(int userId, decimal money,out bool isFrist)
        {
            isFrist = false;
            decimal fee = 0.00m;
            if (money <= 50000) {
                fee = 1.00m;
            }
            else {
                fee = 20.00m;
            }
            if (!_cstTransactionService.MonthIsWithdraw(userId)) {
                isFrist = true;
            }
            return fee;
        }
    }
}
