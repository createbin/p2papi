using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZFCTAPI.Core.Helpers;
using ZFCTAPI.Services.Helpers;

namespace ZFCTAPI.Services.Scheduling.Tasks
{
    public class DownloadBoHaiFileTask:IScheduledTask
    {
        private static  IBoHaiReconciliationService _reconciliationService; 

        public DownloadBoHaiFileTask(IBoHaiReconciliationService reconciliationService)
        {
            _reconciliationService = reconciliationService;
        }

        /// <summary>
        /// 每12小时请求一次结算中心ftp服务器获取对账文件
        /// </summary>
        public string Schedule => "* */12 * * *";
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _reconciliationService.DownloadHandleInvest();
            _reconciliationService.DownloadHandelRecharge();
            _reconciliationService.DownloadHandelWithDraw();
            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
        }
    }
}
