using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using App.Models;
using App.Services;
using App.Views;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class PatientListViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<Patient> _patientsFilteredList = new ObservableCollection<Patient>();
        public ObservableCollection<Patient> PatientsFilteredList
        {
            get { return _patientsFilteredList; }
            set
            {
                _patientsFilteredList = value;
                OnPropertyChanged(nameof(PatientsFilteredList));
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

        private bool _cpfFilterIsVisible;
        public bool CpfFilterIsVisible
        {
            get { return _cpfFilterIsVisible; }
            set
            {
                _cpfFilterIsVisible = value;
                OnPropertyChanged(nameof(CpfFilterIsVisible));
            }
        }

        private bool _emailFilterIsVisible;
        public bool EmailFilterIsVisible
        {
            get { return _emailFilterIsVisible; }
            set
            {
                _emailFilterIsVisible = value;
                OnPropertyChanged(nameof(EmailFilterIsVisible));
            }
        }

        private bool _phoneNumberFilterIsVisible;
        public bool PhoneNumberFilterIsVisible
        {
            get { return _phoneNumberFilterIsVisible; }
            set
            {
                _phoneNumberFilterIsVisible = value;
                OnPropertyChanged(nameof(PhoneNumberFilterIsVisible));
            }
        }

        private string _selectedFilter = "Nome";
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
            }
        }

        private List<string> _filters = new List<string>
        {
            "Nome",
            "Cpf",
            "Email",
            "Telefone"
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

        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
            }
        }

        private string _filter = null;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
            }
        }

        private Keyboard _keyboard;
        public Keyboard Keyboard
        {
            get { return _keyboard; }
            set
            {
                _keyboard = value;
                OnPropertyChanged(nameof(Keyboard));
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

        public ObservableCollection<Patient> patients = new ObservableCollection<Patient>();

        public ICommand GoBack { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public PatientListViewModel()
        {

            GoBack = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public async void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Filter) && Filter != null)
            {
                await Load();
                PatientsFilteredList = patients;
            }
            if (propertyName == nameof(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Nome":
                        FilterText = "Nome...";
                        this.Keyboard = Keyboard.Default;
                        NameFilterIsVisible = true;
                        CpfFilterIsVisible = false;
                        EmailFilterIsVisible = false;
                        PhoneNumberFilterIsVisible = false;
                        break;

                    case "Cpf":
                        FilterText = "Cpf...";
                        this.Keyboard = Keyboard.Numeric;
                        NameFilterIsVisible = false;
                        CpfFilterIsVisible = true;
                        EmailFilterIsVisible = false;
                        PhoneNumberFilterIsVisible = false;
                        break;

                    case "Email":
                        FilterText = "Email...";
                        this.Keyboard = Keyboard.Email;
                        NameFilterIsVisible = false;
                        CpfFilterIsVisible = false;
                        EmailFilterIsVisible = true;
                        PhoneNumberFilterIsVisible = false;
                        break;

                    case "Telefone":
                        FilterText = "Telefone...";
                        Filter = String.Empty;
                        this.Keyboard = Keyboard.Telephone;
                        NameFilterIsVisible = false;
                        CpfFilterIsVisible = false;
                        EmailFilterIsVisible = false;
                        PhoneNumberFilterIsVisible = true;
                        break;

                    case null:
                        FilterText = "Nome...";
                        Filter = String.Empty;
                        this.Keyboard = Keyboard.Default;
                        NameFilterIsVisible = true;
                        CpfFilterIsVisible = false;
                        EmailFilterIsVisible = false;
                        PhoneNumberFilterIsVisible = false;
                        break;

                }
            }
        }

        private async Task<List<Patient>> Search()
        {
            if (String.IsNullOrWhiteSpace(Filter))
            {
                return await Startup.ServiceProvider.GetService<PatientService>().ToListAsync();
            }

            if (NameFilterIsVisible)
            {
                return await Startup.ServiceProvider.GetService<PatientService>().FilterSearchAsync(Filter, null, null, null);
            }

            else if (CpfFilterIsVisible)
            {
                return await Startup.ServiceProvider.GetService<PatientService>().FilterSearchAsync(null, Filter, null, null);
            }
            else if (EmailFilterIsVisible)
            {
                return await Startup.ServiceProvider.GetService<PatientService>().FilterSearchAsync(null, null, Filter, null);
            }
            else if (PhoneNumberFilterIsVisible)
            {
                return await Startup.ServiceProvider.GetService<PatientService>().FilterSearchAsync(null, null, null, Filter);
            }

            return new List<Patient>();
        }

        public async Task Load()
        {
            patients = new ObservableCollection<Patient>(await Search());

        }

        public void LoadItems()
        {
            if (!IsLoading)
            {
                if (patients.Count > 0)
                {
                    IsLoading = true;
                    int offset = patients.Count >= 10 ? 10 : patients.Count;
                    
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        for (int i = 0; i < offset; i++)
                        {
                            if (PatientsFilteredList.Count + 1 <= patients.Count)
                            {
                                PatientsFilteredList.Add(patients[PatientsFilteredList.Count]);
                            }
                        }
                        OnPropertyChanged(nameof(PatientsFilteredList));
                        return IsLoading = false;
                    });
                }
            }
           
        }

    }

}

