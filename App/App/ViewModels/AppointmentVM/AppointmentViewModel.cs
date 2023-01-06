using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Globalization;
using App.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using App.Views;
using App.Services;
using Microsoft.Extensions.DependencyInjection;
using App.Enums;
using System.Drawing;
using SkiaSharp;
using Xamarin.Essentials;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace App.ViewModels
{
    public class AppointmentViewModel : INotifyPropertyChanged
    {
        private float _price;
        public float Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string _dateString;
        public string DateString
        {
            get { return Date.ToString("dd/MM/yyyy"); }
            set
            {
                _dateString = value;
                OnPropertyChanged(nameof(DateString));
            }
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private string _timeString;
        public string TimeString
        {
            get { return Time.ToString(@"hh\:mm"); }
            set
            {
                _timeString = value;
                OnPropertyChanged(nameof(TimeString));
            }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
        private string _durationString;
        public string DurationString
        {
            get { return Duration.ToString(@"hh\:mm"); }
            set
            {
                _durationString = value;
                OnPropertyChanged(nameof(DurationString));
            }
        }
        private int _durationHour;
        public int DurationHour
        {
            get { return _durationHour; }
            set
            {
                _durationHour = value;
                OnPropertyChanged(nameof(DurationHour));
            }
        }

        private int _durationMin;
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
        private static Dictionary<Status, string> _statusOptions = new Dictionary<Status, string>
        {
            {Status.NotPaid, "Pendente" },
            {Status.Paid, "Pago" }
        };

        private Status _paymentStatus;
        public Status PaymentStatus
        {
            get { return _paymentStatus; }
            set
            {
                _paymentStatus = value;
                OnPropertyChanged(nameof(PaymentStatus));
            }
        }

        private string _paymentStatusString;
        public string PaymentStatusString
        {
            get { return _paymentStatusString; }
            set
            {
                _paymentStatusString = value;
                OnPropertyChanged(nameof(PaymentStatusString));
            }
        }


        private string _patient;
        public string Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }

        private bool _isToggled;
        public bool IsToggled
        {
            get { return _isToggled; }
            set
            {
                _isToggled = value;
                OnPropertyChanged(nameof(_isToggled));
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

        private bool _editEnable;
        public bool EditEnable
        {
            get { return _editEnable; }
            set
            {
                _editEnable = value;
                OnPropertyChanged(nameof(EditEnable));
            }
        }

        public ICommand CloseWindow { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Delete { get; set; }

        public ICommand Pay { get; set; }

        public ICommand Confirm { get; set; }


        public AppointmentViewModel(Appointment appointment)
        {

            Price = appointment.Price;
            Date = appointment.Date;
            Time = appointment.Date.TimeOfDay;
            Duration = appointment.Duration;
            DurationHour = appointment.Duration.Hours;
            DurationMin = appointment.Duration.Minutes;
            PaymentStatus = appointment.PaymentStatus;
            Patient = appointment.Patient;
            IsToggled = appointment.PaymentStatus == Status.NotPaid ? false : true;

            for (int i = 0; i < 4; i++)
            {
                _hoursOptions.Add(i);
            }

            for (int i = 0; i < 6; i++)
            {
                _minsOptions.Add(i * 10);
            }


            CloseWindow = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync(true);
            });

            Delete = new Command(async () =>
            {
                await Startup.ServiceProvider.GetService<AppointmentService>().DeleteAsync(appointment);

                MessagingCenter.Send(appointment, "Reload");
                await PopupNavigation.Instance.PopAsync(true);

            });

            Edit = new Command(() =>
            {
                EditEnable = true;
            });

            Pay = new Command(async () =>
            {
                PaymentStatus = IsToggled == false ? Status.NotPaid : Status.Paid;
            });

            Confirm = new Command(async () =>
            {
                var duration = new TimeSpan(DurationHour, DurationMin, 0);
                //var date = Date.Add(new TimeSpan(Time.Hours, Time.Minutes, 0));
                var date = Date.Add(-Date.TimeOfDay + Time);
                var appVerify = await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, date, null, null, null, null, duration, true);
                if (DurationHour != 0 || DurationMin != 0)
                {
                    if (appVerify != null && !appVerify.Any(x => x.Id != appointment.Id))
                    {
                        appointment.Duration = duration;
                        appointment.Date = date;
                        appointment.Price = Price;
                        appointment.PaymentStatus = PaymentStatus;

                        await Startup.ServiceProvider.GetService<AppointmentService>().UpdateAsync(appointment);
                        MessagingCenter.Send(appointment, "Reload");

                        EditEnable = false;
                        TimeSpanError = false;
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
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
 
            if (propertyName == nameof(PaymentStatus))
            {
                PaymentStatusString = _statusOptions[PaymentStatus];
            }
            
            if (propertyName == nameof(Date))
            {
                DateString = Date.ToString("dd/MM/yyyy");
            }

            if (propertyName == nameof(Time))
            {
                TimeString = Time.ToString(@"hh\:mm");
            }

            if (propertyName == nameof(DurationHour) || propertyName == nameof(DurationMin))
            {
                Duration = new TimeSpan(DurationHour,DurationMin,0);
                DurationString = Duration.ToString(@"hh\:mm");
            }

            if (propertyName == nameof(DurationHour) && DurationHour != 0)
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
