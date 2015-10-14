using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class ScheduleItemEditViewmodel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int ScheduleId { get; set; }

        public IList<ScheduleItemWeekday> WeekDays { get; set; }

        public ScheduleItemEditViewmodel() { }
        public ScheduleItemEditViewmodel(ScheduleItem that)
        {
            this.Id = that.Id;
            this.StartTime = that.StartTime;
            this.EndTime = that.EndTime;
            this.ScheduleId = that.ScheduleId;
            this.WeekDays = new List<ScheduleItemWeekday>();
        }
    }

    public class ScheduleItemWeekday
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string Name { get; set; }
    }
}