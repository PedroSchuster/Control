using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using App.Models;
using App.Services;
using Microsoft.Extensions.DependencyInjection;
using App.Enums;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using App.Views;
using Rg.Plugins.Popup.Services;
using DevExpress.XamarinForms.Core.Filtering.Helpers;
using System.Collections.Specialized;

namespace App.ViewModels
{
    public class AppointmentListViewModel : INotifyPropertyChanged
    {


        private ObservableCollection<Appointment> _appointmentsFilteredList = new ObservableCollection<Appointment>();
        public ObservableCollection<Appointment> AppointmentsFilteredList
        {
            get { return _appointmentsFilteredList; }
            set
            {
                _appointmentsFilteredList = value;
                OnPropertyChanged(nameof(AppointmentsFilteredList));
            }
        }

        private bool _nameFilterIsVisible;
        public bool NameFilterIsVisible
        {
            get { return _nameFilterIsVisible; }
            set
            {
                _nameFilterIsVisible = value;
                OnPropertyChanged(nameof(NameFilterIsVisible));
            }
        }

        private bool _dateFilterIsVisible;
        public bool DateFilterIsVisible
        {
            get { return _dateFilterIsVisible; }
            set
            {
                _dateFilterIsVisible = value;
                OnPropertyChanged(nameof(DateFilterIsVisible));
            }
        }

        private bool _statusFilterIsVisible;
        public bool StatusFilterIsVisible
        {
            get { return _statusFilterIsVisible; }
            set
            {
                _statusFilterIsVisible = value;
                OnPropertyChanged(nameof(StatusFilterIsVisible));
            }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter == null ? "Nome" : _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
            }
        }

        private string _nameFilter;
        public string NameFilter
        {
            get { return _nameFilter; }
            set
            {
                _nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
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

        private string _paymentStatus;
        public string PaymentStatus
        {
            get { return _paymentStatus; }
            set
            {
                _paymentStatus = value;
                OnPropertyChanged(nameof(PaymentStatus));
            }
        }

        private List<string> _filters = new List<string>
        {
            "Nome",
            "Data",
            "Status"
        };

        public List<string> Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                OnPropertyChanged(nameof(Filters));
            }
        }

        private bool _calendarClicked = true;
        public bool CalendarClicked
        {
            get { return _calendarClicked; }
            set
            {
                _calendarClicked = value;
                OnPropertyChanged(nameof(CalendarClicked));
            }
        }

        private bool _orderByAscending = true;
        public bool OrderByAscending
        {
            get { return _orderByAscending; }
            set
            {
                _orderByAscending = value;
            }
        }

        private string _imageSource = "up.png";
        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();

        public ICommand CalendarPopup { get; set; }

        public ICommand OrderByCommand { get; set; }

        public ICommand ResetFiltersCommand { get; set; }

        public ICommand GoBack { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isReseting;

        public AppointmentListViewModel()
        {


            CalendarPopup = new Command(async () =>
            {
                CalendarClicked = false;
                await PopupNavigation.Instance.PushAsync(new CalendarPopup(), true);
                CalendarClicked = true;
            });

            OrderByCommand = new Command(() =>
            {
                if (OrderByAscending)
                {
                    ImageSource = "down.png";
                    OrderByAscending = false;
                    OnPropertyChanged(nameof(OrderByAscending));

                }
                else
                {
                    ImageSource = "up.png";
                    OrderByAscending = true;
                    OnPropertyChanged(nameof(OrderByAscending));

                }
            });

            ResetFiltersCommand = new Command(ResetFilters);
            
            GoBack = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });

        }

        public async void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (!isReseting && (propertyName == nameof(NameFilter) && NameFilter != null || (propertyName == nameof(PaymentStatus)) ||
               (propertyName == nameof(StartDate) && DateFilterIsVisible) || propertyName == (nameof(isReseting)) ||
               (propertyName == nameof(EndDate) && DateFilterIsVisible) || propertyName == nameof(OrderByAscending)))
            {
                AppointmentsFilteredList = new ObservableCollection<Appointment>();
                await Load();
                LoadItems();
            }

            if (propertyName == nameof(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Nome":
                        NameFilterIsVisible = true;
                        DateFilterIsVisible = false;
                        StatusFilterIsVisible = false;
                        break;

                    case "Data":
                        NameFilterIsVisible = false;
                        DateFilterIsVisible = true;
                        StatusFilterIsVisible = false;
                        break;

                    case "Status":
                        NameFilterIsVisible = false;
                        DateFilterIsVisible = false;
                        StatusFilterIsVisible = true;
                        break;

                    case null:
                        NameFilter = String.Empty;
                        NameFilterIsVisible = true;
                        DateFilterIsVisible = false;
                        StatusFilterIsVisible = false;
                        break;

                }
            }
        }

        private async Task<List<Appointment>> Search()
        {
            if (StartDate == null)
            {
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            if(EndDate == null)
            {
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            }

            DateTime? endDateTime = new DateTime();
            if (EndDate.HasValue)
            {
                endDateTime = EndDate.Value.Add(new TimeSpan(23, 59, 59));
            }
            else
            {
                endDateTime = null;
            }

            if (PaymentStatus != null)
            {
                return await Startup.ServiceProvider.GetService<AppointmentService>()
                    .FilterSearchAsync(NameFilter, StartDate, endDateTime, _statusPickerOptions[PaymentStatus], null, null, null, OrderByAscending);

            }
            return  await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(NameFilter, StartDate, endDateTime, null, null, null, null, OrderByAscending);

        }

        public void ResetFilters()
        {
            isReseting = true;

            SelectedFilter = "Name";
            NameFilter = String.Empty;
            PaymentStatus = null;
            StartDate = null;
            EndDate = null;

            isReseting = false;
            OnPropertyChanged(nameof(isReseting));
        }

        public async Task Load()
        {
            appointments = new ObservableCollection<Appointment>(await Search());
        }

        public void LoadItems()
        {
            if (!IsLoading)
            {
                if (appointments.Count > 0 && appointments.Count > AppointmentsFilteredList.Count)
                {
                    IsLoading = true;
                    int offset = appointments.Count >= 10 ? 10 : appointments.Count;

                    Device.StartTimer(TimeSpan.FromSeconds(1.5), () => {
                        for (int i = 0; i < offset; i++)
                        {
                            if (AppointmentsFilteredList.Count + 1 <= appointments.Count)
                            {
                                AppointmentsFilteredList.Add(appointments[AppointmentsFilteredList.Count]);
                            }
                        }
                        OnPropertyChanged(nameof(AppointmentsFilteredList));
                        return IsLoading = false;
                    });
                }
            }
            

        }

        public void RemoveItem(Appointment appointment)
        {
            if (!IsLoading)
            {
                IsLoading = true;

                Device.StartTimer(TimeSpan.FromSeconds(1.5), () => {
                    AppointmentsFilteredList.Remove(appointment);
                    OnPropertyChanged(nameof(AppointmentsFilteredList));
                    return IsLoading = false;
                });
            }

        }


    }
}
