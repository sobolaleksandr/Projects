using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Domain.Models
{
    public class HistogramColumn
    {
        public DateTime Day { get; set; }
        public double Value { get; set; }

        public HistogramColumn(double returned, double installed, DateTime day)
        {
            this.Value = 0;
            this.Day = day;

            if (returned != 0 && installed != 0)
                this.Value = returned / installed * 100;
        }
    }
}
