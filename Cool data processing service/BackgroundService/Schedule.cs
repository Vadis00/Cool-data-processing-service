using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool_data_processing_service.BackgroundService
{
    public sealed class Schedule
    {
        public TimeSpan Delay { get; }

        public Schedule(int hour, int minute)
        {
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);

            if (nextRun < now)
                nextRun = nextRun.AddDays(1);

            Delay = nextRun - now;
        }
    }
}
