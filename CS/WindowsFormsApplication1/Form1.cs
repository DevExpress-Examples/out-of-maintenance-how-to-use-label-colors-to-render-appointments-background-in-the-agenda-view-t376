using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;

namespace WindowsFormsApplication1 {
    public partial class Form1 : RibbonForm {
        public Form1() {
            InitializeComponent();
        }

        public static Random RandomInstance = new Random();

        private List<CustomResource> CustomResourceCollection = new List<CustomResource>();
        private List<CustomAppointment> CustomEventList = new List<CustomAppointment>();

        private void Form1_Load(object sender, EventArgs e) {
            InitResources();
            InitAppointments();
            schedulerControl1.Start = DateTime.Now.AddDays(-5);
            schedulerControl1.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Resource;
        }

        private void InitResources() {
            ResourceMappingInfo mappings = this.schedulerStorage1.Resources.Mappings;
            mappings.Id = "ResID";
            mappings.Caption = "Name";

            CustomResourceCollection.Add(CreateCustomResource(1, "Max Fowler", Color.PowderBlue));
            CustomResourceCollection.Add(CreateCustomResource(2, "Nancy Drewmore", Color.PaleVioletRed));
            CustomResourceCollection.Add(CreateCustomResource(3, "Pak Jang", Color.PeachPuff));
            this.schedulerStorage1.Resources.DataSource = CustomResourceCollection;
        }

        private CustomResource CreateCustomResource(int res_id, string caption, Color ResColor) {
            CustomResource cr = new CustomResource();
            cr.ResID = res_id;
            cr.Name = caption;
            return cr;
        }



        private void InitAppointments() {
            AppointmentMappingInfo mappings = this.schedulerStorage1.Appointments.Mappings;
            mappings.Start = "StartTime";
            mappings.End = "EndTime";
            mappings.Subject = "Subject";
            mappings.AllDay = "AllDay";
            mappings.Description = "Description";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "OwnerId";
            mappings.Status = "Status";
            mappings.Type = "EventType";

            GenerateEvents(CustomEventList);
            this.schedulerStorage1.Appointments.DataSource = CustomEventList;
        }


        private void GenerateEvents(List<CustomAppointment> eventList) {
            int count = schedulerStorage1.Resources.Count;

            for(int i = 0; i < count; i++) {
                Resource resource = schedulerStorage1.Resources[i];
                string subjPrefix = resource.Caption + "'s ";
                eventList.Add(CreateEvent(subjPrefix + "meeting", resource.Id, 2, 5, 14));
                eventList.Add(CreateEvent(subjPrefix + "travel", resource.Id, 3, 6, 10));
                eventList.Add(CreateEvent(subjPrefix + "phone call", resource.Id, 0, 4, 16));
            }
        }
        private CustomAppointment CreateEvent(string subject, object resourceId, int status, int label, int sHour) {
            CustomAppointment apt = new CustomAppointment();
            apt.Subject = subject;
            apt.OwnerId = resourceId;
            Random rnd = RandomInstance;
            int rangeInMinutes = 60 * 24;
            apt.StartTime = DateTime.Today.AddHours(sHour);
            apt.EndTime = apt.StartTime.AddHours(1);
            apt.Status = status;
            apt.Label = label;
            return apt;
        }

        private void schedulerControl1_DateNavigatorQueryActiveViewType(object sender, DateNavigatorQueryActiveViewTypeEventArgs e) {
            if(e.OldViewType == SchedulerViewType.Agenda && e.NewViewType != SchedulerViewType.Agenda) {
                e.NewViewType = SchedulerViewType.Agenda;
            }
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            bool isItemChecked = (e.Item as DevExpress.XtraBars.BarToggleSwitchItem).Checked;
            e.Item.Caption = isItemChecked ? "Label mode (Background)" : "Label mode (Bubble)";
            schedulerControl1.SetLabelDrawingMode(isItemChecked ? LabelDrawingMode.Background : LabelDrawingMode.Bubble);
            schedulerControl1.Refresh();
        }
    }
}
