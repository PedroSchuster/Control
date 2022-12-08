using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using App.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views.AppointmentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScheduleView : ContentPage
    {
        public ScheduleView()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new ScheduleViewModel();

        }

    }
}