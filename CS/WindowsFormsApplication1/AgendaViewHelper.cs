using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.XtraScheduler;

namespace WindowsFormsApplication1 {
    public enum LabelDrawingMode {
        Bubble,
        Background
    }

    public static class AgendaViewHelper {
        public static void SetLabelDrawingMode(this SchedulerControl scheduler, LabelDrawingMode mode) {
            if(mode == LabelDrawingMode.Background) {
                scheduler.AgendaView.AppointmentDisplayOptions.ShowLabel = false;
                scheduler.CustomDrawAppointment += scheduler_CustomDrawAppointment;
                scheduler.AppointmentViewInfoCustomizing += scheduler_AppointmentViewInfoCustomizing;
            }
            else {
                scheduler.AgendaView.AppointmentDisplayOptions.ShowLabel = true;
                scheduler.CustomDrawAppointment -= scheduler_CustomDrawAppointment;
                scheduler.AppointmentViewInfoCustomizing -= scheduler_AppointmentViewInfoCustomizing; 
            }
        }

        static void scheduler_CustomDrawAppointment(object sender, CustomDrawObjectEventArgs e) {
            DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo agendaViewInfo = e.ObjectInfo as DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo;
            if(agendaViewInfo != null && agendaViewInfo.Selected) {
                e.DrawDefault();
                e.Cache.DrawRectangle(Pens.Black, e.Bounds);
                e.Handled = true;
            }
        }

        static void scheduler_AppointmentViewInfoCustomizing(object sender, AppointmentViewInfoCustomizingEventArgs e) {
            SchedulerControl scheduler = sender as SchedulerControl;
            DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo agendaViewInfo = e.ViewInfo as DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo;
            if(agendaViewInfo != null) {
                agendaViewInfo.Appearance.BackColor = scheduler.DataStorage.GetLabelColor(agendaViewInfo.Appointment.LabelKey);
            }            
        }
    }
}
