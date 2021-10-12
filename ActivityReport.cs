using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelitietokantaAnalytic
{
    struct ActivityReport
    {
        public double spentDailyAverage;
        public double spentDailyMedian;
        public float spentHourlyAverageIncrease;

        public int onlineDailyAverageMs;
        public int onlineDailyMedianMs;
        public int onlineHourlyAverageIncreaseMs;

        public double purchasesDailyAverage;
        public double purchasesDailyMedian;
        public float purchasesHourlyAverageIncrease;
    }
}
