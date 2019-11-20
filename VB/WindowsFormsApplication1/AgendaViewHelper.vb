Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports DevExpress.XtraScheduler

Namespace WindowsFormsApplication1
	Public Enum LabelDrawingMode
		Bubble
		Background
	End Enum

	Public Module AgendaViewHelper
        <System.Runtime.CompilerServices.Extension> _
        Public Sub SetLabelDrawingMode(ByVal scheduler As SchedulerControl, ByVal mode As LabelDrawingMode)
            If mode = LabelDrawingMode.Background Then
                scheduler.AgendaView.AppointmentDisplayOptions.ShowLabel = False
                AddHandler scheduler.CustomDrawAppointment, AddressOf scheduler_CustomDrawAppointment
                AddHandler scheduler.AppointmentViewInfoCustomizing, AddressOf scheduler_AppointmentViewInfoCustomizing
            Else
                scheduler.AgendaView.AppointmentDisplayOptions.ShowLabel = True
                RemoveHandler scheduler.CustomDrawAppointment, AddressOf scheduler_CustomDrawAppointment
                RemoveHandler scheduler.AppointmentViewInfoCustomizing, AddressOf scheduler_AppointmentViewInfoCustomizing
            End If
        End Sub

        Private Sub scheduler_CustomDrawAppointment(ByVal sender As Object, ByVal e As CustomDrawObjectEventArgs)
            Dim agendaViewInfo As DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo = TryCast(e.ObjectInfo, DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo)
            If agendaViewInfo IsNot Nothing AndAlso agendaViewInfo.Selected Then
                e.DrawDefault()
                e.Cache.DrawRectangle(Pens.Black, e.Bounds)
                e.Handled = True
            End If
        End Sub

        Private Sub scheduler_AppointmentViewInfoCustomizing(ByVal sender As Object, ByVal e As AppointmentViewInfoCustomizingEventArgs)
            Dim scheduler As SchedulerControl = CType(sender, SchedulerControl)
            Dim agendaViewInfo As DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo = TryCast(e.ViewInfo, DevExpress.XtraScheduler.Drawing.AgendaAppointmentViewInfo)
            If agendaViewInfo IsNot Nothing Then
                agendaViewInfo.Appearance.BackColor = scheduler.DataStorage.GetLabelColor(agendaViewInfo.Appointment.LabelKey)
            End If
        End Sub
    End Module
End Namespace
