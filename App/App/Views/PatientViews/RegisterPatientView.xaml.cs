using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    public partial class RegisterPatientView : ContentPage
    {


        public RegisterPatientView()
        {
            InitializeComponent();
            BindingContext = new RegisterPatientViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}