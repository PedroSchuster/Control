using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using DevExpress.XamarinForms.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace App.Services
{
    public class ScheduleData
    {
    }


    public class ReceptionDeskData
    {
        private static int n = 0;

        public static DateTime BaseDate = DateTime.Today;

        public static Color[] AppointmentTypeColors = { Color.FromHex("#6371e8"),
                                                        Color.FromHex("#8b96f6"), };

        void CreateLabels()
        {
            ObservableCollection<AppointmentLabelType> result = new ObservableCollection<AppointmentLabelType>();

            for (int i = 0; i < 2; i++)
            {
                AppointmentLabelType appointmentType = new AppointmentLabelType();
                appointmentType.Id = i;
                appointmentType.Color = AppointmentTypeColors[i];
                result.Add(appointmentType);
            }
            Labels = result;
        }

        void GetAppointments()
        {
            ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();
            ObservableCollection<ScheduleAppointment> result = new ObservableCollection<ScheduleAppointment>();
            Task.Run(async () =>
            {
                appointments = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().ToListAsync());

            }).Wait();

            foreach (var item in appointments)
            {
                n += 1;
                if (n >= 2)
                    n = 0;
                result.Add(CreateAppointment(item.Id, item.Patient, item.Date, item.Duration));
            }

            Appointments = result;
        }


        ScheduleAppointment CreateAppointment(int appointmentId, string patientName,
                                                    DateTime start, TimeSpan duration)
        {
            ScheduleAppointment appointment = new ScheduleAppointment();
            appointment.Id = appointmentId;
            appointment.StartTime = start;
            appointment.EndTime = start.Add(duration);
            appointment.Subject = patientName + " \n" + start.ToString("HH:mm") + " até " + appointment.EndTime.ToString("HH:mm");
            appointment.LabelId = Labels[n].Id;
            return appointment;
        }

        public ObservableCollection<ScheduleAppointment> Appointments { get; private set; }
        public ObservableCollection<AppointmentLabelType> Labels { get; private set; }

        public ReceptionDeskData()
        {
            CreateLabels();

            GetAppointments();
        }
    }
}
