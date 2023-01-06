using App.Models;
using App.Tools;
using App.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientDetailsPopup : PopupPage
    {
        public PatientDetailsPopup(Patient patient)
        {
            BindingContext = new PatientViewModel(patient);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<PatientViewModel>(this, "Reload", (p) =>
            {
                BindingContext = p;
            });

        }

    }
}