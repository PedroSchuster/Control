using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using App.Models;
using App.Services;
using App.Views;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using App.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.MaterialDesignControls;
using System.Threading.Tasks;
using Plugin.MaterialDesignControls.Implementations;

namespace App.ViewModels
{
    public class RegisterAppointmentViewModel : INotifyPropertyChanged
    {

        float _price;
        public float Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        DateTime _date = DateTime.Today;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private TimeSpan _time = DateTime.Now.TimeOfDay;
        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private int _durationHour = 0;
        public int DurationHour
        {
            get { return _durationHour; }
            set
            {
                _durationHour = value;
                OnPropertyChanged(nameof(DurationHour));
            }
        }

        private int _durationMin = 0;
        public int DurationMin
        {
            get { return _durationMin; }
            set
            {
                _durationMin = value;
                OnPropertyChanged(nameof(DurationMin));
            }
        }

        private List<int> _hoursOptions = new List<int>();
        public List<int> HoursOptions
        {
            get { return _hoursOptions; }
            set
            {
                _hoursOptions = value;
                OnPropertyChanged(nameof(HoursOptions));
            }
        }

        private List<int> _minsOptions = new List<int>();
        public List<int> MinOptions
        {
            get { return _minsOptions; }
            set
            {
                _minsOptions = value;
                OnPropertyChanged(nameof(MinOptions));
            }
        }

        private static Dictionary<string, Status> _statusPickerOptions = new Dictionary<string, Status>
        {
            {"Pendente", Status.NotPaid },
            {"Pago", Status.Paid }
        };

        public List<string> StatusPickerOptions
        {
            get { return _statusPickerOptions.Keys.ToList(); }
        }

        string _paymentStatus = "Pendente";
        public string PaymentStatus
        {
            get { return _paymentStatus; }
            set
            {
                _paymentStatus = value;
                OnPropertyChanged(nameof(PaymentStatus));
            }
        }


        string _patient;
        public string Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }

        private bool _patientNull;
        public bool PatientNull
        {
            get { return _patientNull; }
            set
            {
                _patientNull = value;
                OnPropertyChanged(nameof(PatientNull));
            }
        }

        private bool _timeSpanNull;
        public bool TimeSpanNull
        {
            get { return _timeSpanNull; }
            set
            {
                _timeSpanNull = value;
                OnPropertyChanged(nameof(TimeSpanNull));
            }
        }

        private bool _timeSpanError;
        public bool TimeSpanError
        {
            get { return _timeSpanError; }
            set
            {
                _timeSpanError = value;
                OnPropertyChanged(nameof(TimeSpanError));
            }
        }

        private bool _errorIsVisible;
        public bool ErrorIsVisible
        {
            get { return _errorIsVisible; }
            set
            {
                _errorIsVisible = value;
                OnPropertyChanged(nameof(ErrorIsVisible));
            }
        }
        public ObservableCollection<string> Patients { get; set; }


        public ICommand RegisterCommand { get; set; }

        public ICommand GetPatients { get; set; }

        public ICommand GoBack { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterAppointmentViewModel()
        {


            for (int i = 0; i < 4; i++)
            {
                HoursOptions.Add(i);
            }

            for (int i = 0; i < 6; i++)
            {
                MinOptions.Add(i * 10);
            }


            RegisterCommand = new Command(async () =>
            {
                ErrorIsVisible = true;

                if (!String.IsNullOrWhiteSpace(Patient))
                {
                    if (DurationHour != 0 || DurationMin != 0)
                    {
                        var duration = new TimeSpan(DurationHour, DurationMin, 0);
                        var date = Date.Add(new TimeSpan(Time.Hours, Time.Minutes, 0));
                        var appVerify = (await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, date, null, null, null, null, duration, true)).FirstOrDefault();

                        if (appVerify == null)
                        {
                            await Startup.ServiceProvider.GetService<AppointmentService>().InsertAsync(new Appointment
                            {
                                Price = Price,
                                Duration = duration,
                                Date = date,
                                PaymentStatus = _statusPickerOptions[PaymentStatus],
                                Patient = Patient
                            });

                            Price = 0;
                            Date = DateTime.Today;
                            Time = DateTime.Now.TimeOfDay;
                            DurationHour = 0;
                            DurationMin = 0;
                            PaymentStatus = "Pendente";
                            Patient = string.Empty;
                            ErrorIsVisible = false;
                            PatientNull = false;
                            TimeSpanError = false;
                            TimeSpanNull = false;

                        }
                        else
                        {
                            TimeSpanError = true;
                        }
                    }
                    else
                    {
                        TimeSpanNull = true;
                    }
                }
                else
                {
                    PatientNull = true;
                }

            });

            GetPatients = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PatientListViewDetail(true), true);
            });

            GoBack = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync(true);
            });
        }


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Patient) && !String.IsNullOrWhiteSpace(Patient))
            {
                PatientNull = false;
            }
            else if (propertyName == nameof(DurationHour) && DurationHour != 0)
            {
                TimeSpanNull = false;
            }
            else if (propertyName == nameof(DurationMin) && DurationMin != 0)
            {
                TimeSpanNull = false;
            }
        }
    }
}
