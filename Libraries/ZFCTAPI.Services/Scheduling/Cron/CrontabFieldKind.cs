using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Services.Scheduling.Cron
{
    [Serializable]
    public enum CrontabFieldKind
    {
        Minute,
        Hour,
        Day,
        Month,
        DayOfWeek
    }
}
