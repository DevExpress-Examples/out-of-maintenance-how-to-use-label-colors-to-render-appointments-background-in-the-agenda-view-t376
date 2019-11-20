Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing

Namespace WindowsFormsApplication1
	Partial Public Class Form1
		Inherits RibbonForm
		Public Sub New()
			InitializeComponent()
		End Sub

		Public Shared RandomInstance As New Random()

		Private CustomResourceCollection As New List(Of CustomResource)()
		Private CustomEventList As New List(Of CustomAppointment)()

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			InitResources()
			InitAppointments()
            schedulerControl1.GoToToday()
            schedulerControl1.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Resource
		End Sub

		Private Sub InitResources()
			Dim mappings As ResourceMappingInfo = Me.schedulerStorage1.Resources.Mappings
			mappings.Id = "ResID"
			mappings.Caption = "Name"

			CustomResourceCollection.Add(CreateCustomResource(1, "Max Fowler", Color.PowderBlue))
			CustomResourceCollection.Add(CreateCustomResource(2, "Nancy Drewmore", Color.PaleVioletRed))
			CustomResourceCollection.Add(CreateCustomResource(3, "Pak Jang", Color.PeachPuff))
			Me.schedulerStorage1.Resources.DataSource = CustomResourceCollection
		End Sub

		Private Function CreateCustomResource(ByVal res_id As Integer, ByVal caption As String, ByVal ResColor As Color) As CustomResource
			Dim cr As New CustomResource()
			cr.ResID = res_id
			cr.Name = caption
			Return cr
		End Function



		Private Sub InitAppointments()
			Dim mappings As AppointmentMappingInfo = Me.schedulerStorage1.Appointments.Mappings
			mappings.Start = "StartTime"
			mappings.End = "EndTime"
			mappings.Subject = "Subject"
			mappings.AllDay = "AllDay"
			mappings.Description = "Description"
			mappings.Label = "Label"
			mappings.Location = "Location"
			mappings.RecurrenceInfo = "RecurrenceInfo"
			mappings.ReminderInfo = "ReminderInfo"
			mappings.ResourceId = "OwnerId"
			mappings.Status = "Status"
			mappings.Type = "EventType"

			GenerateEvents(CustomEventList)
			Me.schedulerStorage1.Appointments.DataSource = CustomEventList
		End Sub


		Private Sub GenerateEvents(ByVal eventList As List(Of CustomAppointment))
			Dim count As Integer = schedulerStorage1.Resources.Count

			For i As Integer = 0 To count - 1
				Dim resource As Resource = schedulerStorage1.Resources(i)
				Dim subjPrefix As String = resource.Caption & "'s "
				eventList.Add(CreateEvent(subjPrefix & "meeting", resource.Id, 2, 5, 14))
				eventList.Add(CreateEvent(subjPrefix & "travel", resource.Id, 3, 6, 10))
				eventList.Add(CreateEvent(subjPrefix & "phone call", resource.Id, 0, 4, 16))
			Next i
		End Sub
		Private Function CreateEvent(ByVal subject As String, ByVal resourceId As Object, ByVal status As Integer, ByVal label As Integer, ByVal sHour As Integer) As CustomAppointment
			Dim apt As New CustomAppointment()
			apt.Subject = subject
			apt.OwnerId = resourceId
			Dim rnd As Random = RandomInstance
            apt.StartTime = DateTime.Today.AddHours(sHour)
            apt.EndTime = apt.StartTime.AddHours(1)
			apt.Status = status
			apt.Label = label
			Return apt
		End Function

		Private Sub schedulerControl1_DateNavigatorQueryActiveViewType(ByVal sender As Object, ByVal e As DateNavigatorQueryActiveViewTypeEventArgs) Handles schedulerControl1.DateNavigatorQueryActiveViewType
			If e.OldViewType = SchedulerViewType.Agenda AndAlso e.NewViewType <> SchedulerViewType.Agenda Then
				e.NewViewType = SchedulerViewType.Agenda
			End If
		End Sub

		Private Sub barToggleSwitchItem1_CheckedChanged(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles barToggleSwitchItem1.CheckedChanged
			Dim isItemChecked As Boolean = (TryCast(e.Item, DevExpress.XtraBars.BarToggleSwitchItem)).Checked
			e.Item.Caption = If(isItemChecked, "Label mode (Background)", "Label mode (Bubble)")
			schedulerControl1.SetLabelDrawingMode(If(isItemChecked, LabelDrawingMode.Background, LabelDrawingMode.Bubble))
			schedulerControl1.Refresh()
		End Sub
	End Class
End Namespace
