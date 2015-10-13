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

        public ICollection<ScheduleItemWeekday> WeekDays { get; set; }
    }

    public class ScheduleItemWeekday
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
    }
}