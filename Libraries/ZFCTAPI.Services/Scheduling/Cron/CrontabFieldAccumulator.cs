using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Services.Scheduling.Cron
{
    public delegate void CrontabFieldAccumulator(int start, int end, int interval);
}
