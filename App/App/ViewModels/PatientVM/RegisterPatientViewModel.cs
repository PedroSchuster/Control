using App.Models;
using App.Services;
using App.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class RegisterPatientViewModel : INotifyPropertyChanged
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

        private DateTime _birth = DateTime.Today;
        public DateTime Birth
        {
            get { return _birth; }
            set
            {
                _birth = value;
                OnPropertyChanged(nameof(Birth));
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

        private Label _nameError = new Label
        {
            Text = "Nome é obrigatório",
            IsVisible = false
        };
        public Label NameError
        {
            get { return _nameError; }
        }

        private Label _cpfError = new Label
        {
            Text = "Cpf inválido",
            IsVisible = false
        };
        public Label CpfError
        {
            get { return _cpfError; }
            set
            {
                _cpfError = value;
                OnPropertyChanged(nameof(CpfError));
            }
        }

        private Label _cpfNull = new Label
        {
            Text = "Cpf é obrigatório",
            IsVisible = false
        };
        public Label CpfNull
        {
            get { return _cpfNull; }
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

        private bool _checkName;
        public bool CheckName
        {
            get { return _checkName; }
            set
            {
                _checkName = value;
                OnPropertyChanged(nameof(CheckName));
            }
        }

        private bool _checkCpf;
        public bool CheckCpf
        {
            get { return _checkCpf; }
            set
            {
                _checkCpf = value;
                OnPropertyChanged(nameof(CheckCpf));
            }
        }

        private bool _checkEmail;
        public bool CheckEmail
        {
            get { return _checkEmail; }
            set
            {
                _checkEmail = value;
                OnPropertyChanged(nameof(CheckEmail));
            }
        }

        private bool _checkIsVisible;
        public bool CheckIsVisible
        {
            get { return _checkIsVisible; }
            set
            {
                _checkIsVisible = value;
                OnPropertyChanged(nameof(CheckIsVisible));
            }
        }

        public ICommand RegisterCommand { get; set; }

        public ICommand GoBack { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterPatientViewModel()
        {
            RegisterCommand = new Command(async () =>
            {
                CheckName = ValidateName(Name);

                CheckCpf = ValidateCpf(Cpf);

                CheckEmail = ValidateEmailAsync(Email);


                if (CheckName && CheckCpf && CheckEmail)
                {
                    foreach (string word in Name.Split(' '))
                    {
                        if (String.IsNullOrWhiteSpace(word))
                        {
                            Name.TrimEnd();
                        }
                        else
                        {
                            var upper = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word);
                            Name = Name.Replace(word, upper);
                        }

                    }

                    await Startup.ServiceProvider.GetService<PatientService>().InsertAsync(new Patient
                    {
                        Name = Name,
                        Cpf = Cpf,
                        Birth = Birth.Date,
                        Email = Email,
                        PhoneNumber = PhoneNumber
                    });

                    Name = string.Empty;
                    Cpf = string.Empty;
                    Birth = DateTime.Now;
                    Email = string.Empty;
                    PhoneNumber = string.Empty;

                    CheckIsVisible = false;
                }
                else
                {
                    CheckIsVisible = true;
                }
            });

            GoBack = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync(true);
            });

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Name))
            {
                if (!String.IsNullOrWhiteSpace(Name))
                {
                    if (!CheckName)
                    {
                        CheckName = ValidateName(Name);
                    }
                }
                else
                {
                    CheckName = false;
                }
            }

            if (propertyName == nameof(Cpf))
            {
                if (!String.IsNullOrWhiteSpace(Cpf))
                {
                    CpfError.IsVisible = false;
                    CpfNull.IsVisible = false;

                    if (Cpf.Length >= 11)
                    {
                        CheckCpf = ValidateCpf(Cpf);
                    }
                }
                else
                {
                    CheckCpf = false;
                }
            }

            if (propertyName == nameof(Email))
            {
                if (!String.IsNullOrWhiteSpace(Email))
                {
                    EmailError.IsVisible = false;
                    if (Email.Contains("@"))
                    {
                        CheckEmail = ValidateEmailAsync(Email);
                    }
                    else
                    {
                        CheckEmail = false;
                    }
                }
            }

        }

        private bool ValidateName(string name)
        {
            return !(NameError.IsVisible = String.IsNullOrWhiteSpace(name));
        }

        private bool ValidateCpf(string cpf)
        {
            bool isValid = false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;

            string digito;

            int soma;

            int resto;

            if (!String.IsNullOrWhiteSpace(cpf))
            {
                cpf = cpf.Trim();

                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length == 11)
                {
                    tempCpf = cpf.Substring(0, 9);

                    soma = 0;

                    for (int i = 0; i < 9; i++)

                        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                    resto = soma % 11;

                    if (resto < 2)

                        resto = 0;

                    else

                        resto = 11 - resto;

                    digito = resto.ToString();

                    tempCpf = tempCpf + digito;

                    soma = 0;

                    for (int i = 0; i < 10; i++)

                        soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                    resto = soma % 11;

                    if (resto < 2)

                        resto = 0;

                    else

                        resto = 11 - resto;

                    digito = digito + resto.ToString();

                    isValid = Cpf.EndsWith(digito);

                    if (!isValid)
                    {
                        CpfNull.IsVisible = false;
                        CpfError.IsVisible = true;
                        return false;
                    }
                    else
                    {
                        CpfNull.IsVisible = false;
                        CpfError.IsVisible = false;
                        return true;
                    }

                }
                else
                {
                    CpfNull.IsVisible = false;
                    CpfError.IsVisible = true;
                    return false;
                }

            }
            else
            {
                CpfNull.IsVisible = true;
                CpfError.IsVisible = false;
                return false;
            }
        }

        private bool ValidateEmailAsync(string email)
        {
            var emailRegex = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";

            if ((!String.IsNullOrEmpty(email) && Regex.IsMatch(email, emailRegex)) || String.IsNullOrEmpty(email))
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
