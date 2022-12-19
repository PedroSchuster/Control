using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using App.Models;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.ComponentModel;
using App.Views;
using Microsoft.EntityFrameworkCore.Metadata;
using App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using System.Text.RegularExpressions;

namespace App.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private Label _nameError = new Label
        {
            Text = "Nome é obrigatório",
            IsVisible = false
        };
        public Label NameError
        {
            get { return _nameError; }
        }

        private string _cpf;
        public string Cpf
        {
            get { return _cpf; }
            set
            {
                _cpf = value;
                OnPropertyChanged(nameof(Cpf));
            }
        }

        private DateTime _birth;
        public DateTime Birth
        {
            get { return _birth; }
            set
            {
                _birth = value;
                OnPropertyChanged(nameof(Birth));
            }
        }

        public string BirthString
        {
            get { return Birth.ToString("dd/MM/yyyy"); }
            set
            {
                OnPropertyChanged(nameof(BirthString));
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
            
        }

        private Label _emailError = new Label
        {
            Text = "Email inválido",
            IsVisible = false
        };
        public Label EmailError
        {
            get { return _emailError; }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
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

        public ICommand Confirm { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PatientViewModel(Patient patient)
        {

            Name = patient.Name;

            Cpf = patient.Cpf;
            Cpf = Cpf.Insert(9, "-");

            Birth = patient.Birth;

            Email = patient.Email;

            PhoneNumber = patient.PhoneNumber;

            CloseWindow = new Command(async () =>
            {
                MessagingCenter.Send(patient, "Reload");
                await PopupNavigation.Instance.PopAsync(true);
            });

            Edit = new Command( () =>
            {
                EditEnable = true;
            });

            Delete = new Command(async () =>
            {
                var appointmentsList = await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(patient.Name, null, null, null, null, null, null, true);

                foreach (var item in appointmentsList)
                {
                    await Startup.ServiceProvider.GetService<AppointmentService>().DeleteAsync(item);
                }
                await Startup.ServiceProvider.GetService<PatientService>().DeleteAsync(patient);

                MessagingCenter.Send(patient, "Reload");
                await PopupNavigation.Instance.PopAsync(true);

            });

            Confirm = new Command( async() =>
            {
                patient.Name = Name;
                patient.Birth = Birth;
                patient.Email = Email;
                patient.PhoneNumber = PhoneNumber;

                var checkName = ValidateName(Name);

                var checkEmail = ValidateEmailAsync(Email);

                if (checkName && checkEmail)
                {
                    await Startup.ServiceProvider.GetService<PatientService>().UpdateAsync(patient);
                    MessagingCenter.Send(patient, "Reload");
                    EditEnable = false;
                }

            });

        }


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if(propertyName == nameof(Birth))
            {
                BirthString = Birth.ToString("dd/MM/yyyy");
            }
        }

        private bool ValidateName(string name)
        {
            return !(NameError.IsVisible = String.IsNullOrWhiteSpace(name));
        }

        private bool ValidateEmailAsync(string email)
        {
            var emailRegex = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";

            if (Regex.IsMatch(email, emailRegex) || String.IsNullOrEmpty(email))
            {
                return !(EmailError.IsVisible = false);
            }
            else
            {
                return !(EmailError.IsVisible = true);
            }
        }
    }
}