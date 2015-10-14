using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personalsystem.Models
{
    public class ScheduleDetailsViewmodel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int DepartmentId { get; set; }
        public string Department { get; set; }

        public int? GroupId { get; set; }
        public string Group { get; set; }

        public ICollection<ScheduleDetailsWeekDayViewmodel> ScheduleItems { get; set; }

        public ScheduleDetailsViewmodel() { }

        public ScheduleDetailsViewmodel(Schedule that)
        {
            this.Id = that.Id;
            this.StartTime = that.StartTime;
            this.EndTime = that.EndTime;
            this.DepartmentId = that.DepartmentId;
            this.Department = that.Department.Name;
            this.GroupId = that.GroupId;
            this.Group = that.Group == null ? null : that.Group.Name;

            var weekDays = that.ScheduleItems.SelectMany(q => q.WeekDays).Distinct().ToList();
            this.ScheduleItems = weekDays.Select(q => new ScheduleDetailsWeekDayViewmodel(q)).ToList();

        }

    }

    public class ScheduleDetailsWeekDayViewmodel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<ScheduleDetailsWeekDayItemViewmodel> Items { get; set; }

        public ScheduleDetailsWeekDayViewmodel() { }

        public ScheduleDetailsWeekDayViewmodel(ScheduleDayOfWeek that)
        {
            this.Id = that.Id;
            this.Description = that.Description;
            this.Items = that.ScheduleItems.Select(q => new ScheduleDetailsWeekDayItemViewmodel() { Id = q.Id, StartTime = q.StartTime, EndTime = q.EndTime, Description = q.Description }).ToList();
        }

    }
    public class ScheduleDetailsWeekDayItemViewmodel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }
    }


    public class ScheduleItemListitemViewmodel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }

        public string WeekDays { get; set; }

        public ScheduleItemListitemViewmodel() { }

        public ScheduleItemListitemViewmodel(ScheduleItem that) 
        {
            this.Id = that.Id;
            this.StartTime = that.StartTime;
            this.EndTime = that.EndTime;
            this.Description = that.Description;
            this.WeekDays = "";
            foreach (var day in that.WeekDays)
            {
                this.WeekDays += day.Description.Substring(0, 3) + ", ";
            }
        }
    }

    public class ScheduleItemEditViewmodel
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }

        public int ScheduleId { get; set; }

        public IList<ScheduleItemWeekday> WeekDays { get; set; }

        public ScheduleItemEditViewmodel() { }
        public ScheduleItemEditViewmodel(ScheduleItem that)
        {
            this.Id = that.Id;
            this.StartTime = that.StartTime;
            this.EndTime = that.EndTime;
            this.Description = that.Description;
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