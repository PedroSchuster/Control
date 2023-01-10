using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App.Models;
using App.Services;
using App.Views;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace App.ViewModels.TabsVM
{
    public class HomeViewModel : INotifyPropertyChanged
    {
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

        public ICommand RegisterAppointmentCommand { get; set; }
        public ICommand RegisterPatientCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public HomeViewModel()
        {

            Task.Run(async () =>
            {
                ObservableCollection<Patient> patients = new ObservableCollection<Patient>(await Startup.ServiceProvider.GetService<PatientService>().ToListAsync());
                Patient lastPatient = patients.OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastPatient != null)
                    PatientText = "Último paciente cadastrado: \n" + lastPatient.Name;
                else
                    PatientText = "Não há pacientes cadastrados";

                ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, DateTime.Today, null, null, null, null, null, true));
                Appointment nextAppointment = appointments.Where(x=>x.Date > DateTime.Now).OrderBy(x => x.Date).FirstOrDefault();
                if (nextAppointment != null)
                    AppointmentText = "Próxima consulta: \n" + nextAppointment.Patient + "\n" + nextAppointment.Date.ToString("dd/MM HH:mm");
                else
                    AppointmentText = "Não há próximas consultas agendadas";

            }).Wait();

            RegisterAppointmentCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(App.RegAppV);
            });

            RegisterPatientCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(App.RegPatientV);
            });
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
