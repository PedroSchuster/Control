using App.Data;
using App.Enums;
using App.Models;
using App.Services;
using App.Views;
using App.Views.Tabs;
using Microcharts;
using Microcharts.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels.TabsVM
{
    public class ChartViewModel : INotifyPropertyChanged
    {

        private const int minYear = 2022;
        private int maxYear = DateTime.Today.Year + 15;
        private int count = 0;
        private float totalPaid = 0;
        private float totalNotPaid = 0;


        PieChart _chart;
        public PieChart Chart
        {
            get { return _chart; }
            set
            {
                _chart = value;
                OnPropertyChanged(nameof(Chart));
            }
        }

        private static Dictionary<string, int> _months = new Dictionary<string, int>
            {
                { "Janeiro" , 1 },
                { "Fevereiro", 2 },
                { "Março", 3 },
                { "Abril", 4 },
                { "Maio", 5 },
                { "Junho", 6 },
                { "Julho", 7 } ,
                { "Agosto", 8 },
                { "Setembro", 9 },
                { "Outubro", 10 },
                { "Novembro", 11 },
                { "Dezembro", 12 }
            };
        public List<string> Months
        {
            get { return _months.Keys.ToList(); }
        }

        bool _monthIsVisible = false;
        public bool MonthIsVisible
        {
            get { return _monthIsVisible; }
            set
            {
                _monthIsVisible = value;
                OnPropertyChanged(nameof(MonthIsVisible));
            }
        }

        List<int> _years = new List<int>();
        public List<int> Years
        {
            get { return _years; }
            set
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }

        bool _yearIsVisible = false;
        public bool YearIsVisible
        {
            get { return _yearIsVisible; }
            set
            {
                _yearIsVisible = value;
                OnPropertyChanged(nameof(YearIsVisible));
            }
        }

        bool _dateIsVisible = false;
        public bool DateIsVisible
        {
            get { return _dateIsVisible; }
            set
            {
                _dateIsVisible = value;
                OnPropertyChanged(nameof(DateIsVisible));
            }
        }

        private bool _totalPaidContainer = false;
        public bool TotalPaidContainer
        {
            get { return _totalPaidContainer; }
            set
            {
                _totalPaidContainer = value;
                OnPropertyChanged(nameof(TotalPaidContainer));
            }
        }

        private bool _totalNotPaidContainer = false;
        public bool TotalNotPaidContainer
        {
            get { return _totalNotPaidContainer; }
            set
            {
                _totalNotPaidContainer = value;
                OnPropertyChanged(nameof(TotalNotPaidContainer));
            }
        }

        private bool _totalFoundContainer = false;
        public bool TotalFoundContainer
        {
            get { return _totalFoundContainer; }
            set
            {
                _totalFoundContainer = value;
                OnPropertyChanged(nameof(TotalFoundContainer));
            }
        }
        private List<string> _filters = new List<string>() { "Mês", "Ano", "Período" };
        public List<string> Filters
        {
            get { return _filters; }
        }
        private string _selectedFilter = "Mês";
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
            }
        }

        private string _selectedMonth;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        private int _selectedYear = DateTime.Today.Year;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        private DateTime? _startDate = DateTime.Today;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? _endDate = DateTime.Today;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private string _totalFoundText;
        public string TotalFoundText
        {
            get { return _totalFoundText; }
            set
            {
                _totalFoundText = value;
                OnPropertyChanged(nameof(TotalFoundText));
            }
        }

        private string _paidText;
        public string PaidText
        {
            get { return _paidText; }
            set
            {
                _paidText = value;
                OnPropertyChanged(nameof(PaidText));
            }
        }


        private string _notPaidText;
        public string NotPaidText
        {
            get { return _notPaidText; }
            set
            {
                _notPaidText = value;
                OnPropertyChanged(nameof(NotPaidText));
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

        public ICommand CalendarPopup { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;



        public ChartViewModel()
        {
            for (int i = minYear; i <= maxYear; i++)
            {
                Years.Add(i);
            };


            CalendarPopup = new Command(async () =>
            {
                CalendarClicked = false;
                await PopupNavigation.Instance.PushAsync(new CalendarPopup(), true);
                CalendarClicked = true;
            });



        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case ("Mês"):
                        DateIsVisible = false;
                        MonthIsVisible = true;
                        YearIsVisible = false;
                        break;
                    case ("Ano"):
                        DateIsVisible = false;
                        MonthIsVisible = false;
                        YearIsVisible = true;
                        break;
                    case ("Período"):
                        DateIsVisible = true;
                        MonthIsVisible = false;
                        YearIsVisible = false;
                        break;
                }
            }

            if ((propertyName == nameof(SelectedMonth) && MonthIsVisible) || (propertyName == nameof(MonthIsVisible) && MonthIsVisible))
            {
                if (SelectedMonth == null)
                {
                    SelectedMonth = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Today.ToString("MMMM", CultureInfo.CurrentCulture));
                }

                Task.Run(() => LoadData(null, null, null, _months[SelectedMonth])).Wait();

            }
            else if ((propertyName == nameof(SelectedYear) && YearIsVisible) || (propertyName == nameof(YearIsVisible) && YearIsVisible))
            {
                Task.Run(() => LoadData(null, null, SelectedYear, null)).Wait();
            }
            else if ((propertyName == nameof(StartDate) && DateIsVisible) ||
                (propertyName == nameof(EndDate) && DateIsVisible) || (propertyName == nameof(DateIsVisible) && DateIsVisible))
            {
                DateTime endDateTime = new DateTime();
                endDateTime = EndDate.Value.Add(new TimeSpan(23, 59, 59));

                Task.Run(() => LoadData(StartDate, endDateTime, null, null)).Wait();
            }

        }

        public async void LoadData(DateTime? startDate, DateTime? endDate, int? selectedYear, int? selectedMonth)
        {
            if (selectedYear == null)
                selectedYear = DateTime.Today.Year;

            Chart = await Startup.ServiceProvider.GetService<GenerateChartData>().GenerateChartAsync(selectedMonth, selectedYear, startDate, endDate);

            count = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().
                FilterSearchAsync(null, startDate, endDate, null, selectedYear, selectedMonth, null, true)).Count;
            TotalFoundText = "Consultas: " + count;

            totalPaid = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().
                FilterSearchAsync(null, startDate, endDate, Status.Paid, selectedYear, selectedMonth, null, true)).Sum(x => x.Price);
            PaidText = "Valor recebido: " + totalPaid.ToString("F2");

            totalNotPaid = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().
                FilterSearchAsync(null, startDate, endDate, Status.NotPaid, selectedYear, selectedMonth, null, true)).Sum(x => x.Price);
            NotPaidText = "Valor faltante: " + totalNotPaid.ToString("F2");

        }

    }
}

