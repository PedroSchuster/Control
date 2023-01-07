using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App.Models;
using App.Services;
using App.Views;
using App.Views.AppointmentViews;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace App.ViewModels.TabsVM
{
    public class HubViewModel : INotifyPropertyChanged
    {
        private int totalAppointmentPerDay;
        private int totalAppointmentPerWeek;
        private int totalAppointmentPerMonth;

        private int counter = 0;
        private string _appointmentText;
        public string AppointmentText
        {
            get { return _appointmentText; }
            set
            {
                _appointmentText = value;
                OnPropertyChanged(nameof(AppointmentText));
            }
        }

        private string _patientText;
        public string PatientText
        {
            get { return _patientText; }
            set
            {
                _patientText = value;
                OnPropertyChanged(nameof(PatientText));
            }
        }
        public ICommand AppointmentListCommand { get; set; }

        public ICommand AppointmentScheduleCommand { get; set; }

        public ICommand PatientListCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public HubViewModel()
        {

            AppointmentListCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AppointmentListViewDetail(), true);
            });

            AppointmentScheduleCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(App.SchV);
            });

            PatientListCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PatientListViewDetail());
            });

            Task.Run(async () =>
            {
                totalAppointmentPerDay = new List<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, DateTime.Today, DateTime.Today.Add(new TimeSpan(23,59,59)), null, null, null, null, true)).Count;
                
                DateTime baseDate = DateTime.Today;
                DateTime thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                
                totalAppointmentPerWeek = new List<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, thisWeekStart, thisWeekEnd, null, null, null, null, true)).Count;
                totalAppointmentPerMonth = new List<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, null, null, null, DateTime.Today.Year, DateTime.Today.Month, null, true)).Count;
                
                int patientCount = new List<Patient>(await Startup.ServiceProvider.GetService<PatientService>().ToListAsync()).Count;
                PatientText = "Total de pacientes: " + patientCount;
            });


        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeAppointmentTextValue()
        {
            var textOptions = new List<string> { "Total de consultas nesse dia: " + totalAppointmentPerDay,
                "Total de consultas nessa semana: " + totalAppointmentPerWeek,
                "Total de consultas nesse mês: " + totalAppointmentPerMonth
            };

            AppointmentText = textOptions[counter];
            counter += 1;
            if (counter >= 3)
            {
                counter = 0;
            }
        }
    }
}
