using BlazorScheduler.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;

namespace BlazorScheduler.Internal.Components
{
    public partial class SchedulerDay<T> where T : IAppointment, new() 
	{
        [CascadingParameter] public Scheduler<T> Scheduler { get; set; }

        [Parameter] public DateTime Day { get; set; }

        private bool IsDiffMonth => Day.Month != Scheduler.CurrentDate.Month;
        private string DateText => (IsDiffMonth && Day.Day == 1) ? Day.ToString("MMM d") : Day.Day.ToString();
        private bool isDropTarget = false;
		private IEnumerable<string> Classes {
			get {
				if (Day == DateTime.Today)
                {
                    yield return "today";
                }

                if (IsDiffMonth)
                {
                    yield return "diff-month";
                }

                if (isDropTarget)
                {
                    yield return "drop-target";
                }
            }
		}

		private void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == 0)
			{
				Scheduler.BeginDrag(this);
			}
		}

        private void HandleDragEnter()
        {
            if (Scheduler.IsDragInProgress)
            {
                isDropTarget = true;
                UpdateDraggedAppointment();
            }            
        }

        private void HandleDragLeave()
        {
            isDropTarget = false;
        }

        private void HandleDrop()
        {
            isDropTarget = false;

            UpdateDraggedAppointment();
        }

        private void UpdateDraggedAppointment()
        {
            if (Scheduler.IsDragInProgress)
            {
                if (Scheduler.DraggedAppointment.Start > Day)
                {
                    int dayDiff = (Day - Scheduler.DraggedAppointment.Start).Days;
                    Scheduler.DraggedAppointment.Start = Scheduler.DraggedAppointment.Start.AddDays(dayDiff);
                    Scheduler.DraggedAppointment.End = Scheduler.DraggedAppointment.End.AddDays(dayDiff);
                }

                if (Scheduler.DraggedAppointment.Start < Day)
                {
                    int dayDiff = (Day - Scheduler.DraggedAppointment.Start).Days;
                    Scheduler.DraggedAppointment.Start = Scheduler.DraggedAppointment.Start.AddDays(dayDiff);
                    Scheduler.DraggedAppointment.End = Scheduler.DraggedAppointment.End.AddDays(dayDiff);
                }

                Scheduler.Refresh();
            }
        }
    }
}
