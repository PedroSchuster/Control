using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterAppointmentView : ContentPage
    {
        public RegisterAppointmentView()
        {
            InitializeComponent();
            BindingContext = new RegisterAppointmentViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            MessagingCenter.Subscribe<Patient>(this, "SelectedPatient", (p) =>
            {
                var vm = this.BindingContext as RegisterAppointmentViewModel;
                vm.Patient = p.Name;
            });
        }
    }
}