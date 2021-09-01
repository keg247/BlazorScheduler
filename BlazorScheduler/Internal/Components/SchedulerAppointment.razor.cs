using BlazorScheduler.Core;
using BlazorScheduler.Internal.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorScheduler.Internal.Components
{
    public partial class SchedulerAppointment<T> where T : IAppointment, new()
	{
		[CascadingParameter] public Scheduler<T> Scheduler { get; set; }

		[Parameter] public T Appointment { get; set; }
		[Parameter] public int Order { get; set; }

		private int Start => (int)Appointment.Start.DayOfWeek;
		private bool IsTimedAppointment => Appointment.Start.Date == Appointment.End.Date && Appointment.Start != Appointment.End;
		private string BackgroundColor => Appointment.Color.ToRgbString();

        private bool isDragged = false;

        private IEnumerable<string> Classes
        {
            get
            {
                if (ReferenceEquals(Appointment, Scheduler.NewAppointment))
                {
                    yield return "new-appointment";
                }
                if (isDragged)
                {
                    yield return "dragged";
                }
            }
        }

        private void HandleDragStart()
        {
            Scheduler.DraggedAppointment = Appointment;
            Scheduler.IsDragInProgress = true;
            isDragged = true;
        }

        private void HandleDragStop()
        {
            Scheduler.DraggedAppointment = default;
            Scheduler.IsDragInProgress = false;
            isDragged = false;
        }
    }
}
